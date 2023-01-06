using Autofac;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Piece;
using Theater.Core.Authorization;
using Theater.Core.Theater;
using Theater.Sql.Repositories;

namespace Theater.Core
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AuthorizationRepository>()
                .As<IAuthorizationRepository>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<AuthorizationService>()
                .As<IAuthorizationService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JwtHelper>()
                .As<IJwtHelper>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceService>()
                .As<IPieceService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PieceRepository>()
                .As<IPieceRepository>()
                .InstancePerLifetimeScope();
        }
    }
}