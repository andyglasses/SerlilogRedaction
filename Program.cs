using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Serilog;
using Serilog_redaction;
using Serilog_redaction.Logging.Redaction;
using System.Globalization;
using Serilog.Exceptions;
using Serilog.Sinks.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRedaction(x =>
{
    x.SetRedactor<ErasingRedactor>(DataClassificationSet.FromDataClassification(Taxonomy.Secret));
    x.SetHmacRedactor(hmacOpts =>
    {
        hmacOpts.Key = Convert.ToBase64String("Some super secret key that is really long for security"u8.ToArray()); // read from app settings
        hmacOpts.KeyId = 1066;
    }, DataClassificationSet.FromDataClassification(Taxonomy.Personal));
    x.SetFallbackRedactor<NullRedactor>();
});

var loggingCulture = CultureInfo.ReadOnly(new CultureInfo("en-US"));

builder.Host.UseSerilog((ctx, services, lc) =>
{
    lc.Enrich.WithExceptionDetails()
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration);

    lc.Destructure.WithRedaction(services.GetRequiredService<IRedactorProvider>());
    lc.WriteTo.Console(formatProvider: loggingCulture, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}{NewLine}");
    lc.WriteTo.Http(
        requestUri: "https://webhook.site/a0d2ff97-e411-4c77-b15d-ec9ff73fb7dc",
        queueLimitBytes: 10 * ByteSize.MB);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("test", (Payload payload, ILogger<Program> logger) =>
{
    LoggedMessages.ProcessedMessage(logger, payload.Name, payload);
    LoggedMessages.ProcessedMessage2(logger, payload);
});

app.Run();


