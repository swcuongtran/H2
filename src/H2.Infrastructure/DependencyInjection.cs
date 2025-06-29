using Amazon.S3;
using Amazon;
using H2.Infrastructure.Cloud;
using H2.Infrastructure.Persistence.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using H2.Application.Interfaces.Cloud;
using H2.Application.Interfaces.Repositories;
using H2.Infrastructure.Persistence.Repositories;
using H2.Application.Interfaces.Messaging;
using H2.Infrastructure.Messaging;
namespace H2.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<H2DbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.Configure<AwsS3Settings>(configuration.GetSection("AwsS3"));
            var awsConfig = configuration.GetSection("AwsS3").Get<AwsS3Settings>();
            services.AddSingleton<IAmazonS3>(sp =>
                new AmazonS3Client(awsConfig.AccessKey, awsConfig.SecretKey, RegionEndpoint.GetBySystemName(awsConfig.Region)));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
            services.AddSingleton<IMessageQueue, RabbitMqPublisher>();
            // Register repositories and other services here
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ISensorDataRepository, SensorDataRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IThermalImageRepository, ThermalImageRepository>();
            services.AddScoped<IS3Uploader, AwsS3Uploader>();

            return services;
        }
    }
}
