using Autofac;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Piece;
using Theater.Abstractions.TheaterWorker;
using Theater.Abstractions.Ticket;
using Theater.Abstractions.UserAccount;
using Theater.Core.Authorization;
using Theater.Core.Theater;
using Theater.Core.Ticket;
using Theater.Core.UserAccount;
using Theater.Sql.Repositories;

namespace Theater.Core
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtHelper>()
                .As<IJwtHelper>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceService>()
                .As<IPieceService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PieceRepository>()
                .As<IPieceRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<UserAccountService>()
                .As<IUserAccountService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<UserAccountRepository>()
                .As<IUserAccountRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TheaterWorkerService>()
                .As<ITheaterWorkerService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TheaterWorkerRepository>()
                .As<ITheaterWorkerRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TicketService>()
                .As<ITicketService>()
                .InstancePerLifetimeScope(); 
            
            builder.RegisterType<TicketRepository>()
                .As<ITicketRepository>()
                .InstancePerLifetimeScope();
        }
    }
}