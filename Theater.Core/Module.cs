using Autofac;
using Theater.Abstractions.Authorization;
using Theater.Core.Authorization;
using Theater.Sql.Repositories;

namespace Theater.Core
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AuthorizationRepository>()
                .As<IAuthorizationRepository>();

            builder
                .RegisterType<AuthorizationService>()
                .As<IAuthorizationService>();
        }
    }
}
