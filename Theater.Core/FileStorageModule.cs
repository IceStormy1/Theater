using Amazon.S3;
using Autofac;
using Microsoft.Extensions.Options;
using Theater.Abstractions.FileStorage;
using Theater.Configuration;
using Theater.Core.FileStorage;

namespace Theater.Core;

public class FileStorageModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileStorageService>().As<IFileStorageService>().InstancePerLifetimeScope();

        builder.Register(context =>
        {
            var options = context.Resolve<IOptions<FileStorageOptions>>();
             
            var config = new AmazonS3Config
            {
                AuthenticationRegion = options.Value.Region, // Should match the `MINIO_REGION` environment variable.
                ServiceURL = options.Value.ServiceInnerUrl,
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };

            return new AmazonS3Client(options.Value.AccessKey, options.Value.SecretKey, config);
        }).As<IAmazonS3>().SingleInstance();
    }
}