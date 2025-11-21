var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to support JWT bearer auth
builder.Services.AddSwaggerGen();

// Register the reverse proxy services - Yarp (Yet Another Reverse Proxy)
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(transforms =>
    {
        // Log each incoming request
        transforms.AddRequestTransform(context =>
        {
            var logger = context.HttpContext.RequestServices
                           .GetRequiredService<ILoggerFactory>()
                           .CreateLogger("Yarp.Request");

            logger.LogInformation(
                "Proxying {Method} {OriginalPath} → {DestUri}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path + context.HttpContext.Request.QueryString,
                context.ProxyRequest.RequestUri);

            return default;
        });

        // Log each outgoing response
        transforms.AddResponseTransform(async context =>
        {
            var logger = context.HttpContext.RequestServices
                           .GetRequiredService<ILoggerFactory>()
                           .CreateLogger("Yarp.Response");

            var status = (int)context.ProxyResponse!.StatusCode;
            logger.LogInformation(
                "Received {StatusCode} for {Method} {OriginalPath}",
                status,
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path + context.HttpContext.Request.QueryString);
        });
    });

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opts =>
  {
      opts.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = "https://localhost:5001",
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("super_secret_key_123!4567890_secure_long_key"))
      };
  });

// Rate limitng based on IP address
builder.Services.AddMemoryCache();

builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
// Rate limitng based on IP address

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API");
        c.SwaggerEndpoint("/customer/swagger/v1/swagger.json", "Customer Service");
        c.SwaggerEndpoint("/order/swagger/v1/swagger.json", "Order Service");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy();

// Rate limiter
app.UseIpRateLimiting();

app.Run();
