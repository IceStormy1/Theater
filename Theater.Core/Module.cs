using Autofac;
using Theater.Abstractions;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Piece;
using Theater.Abstractions.PieceDates;
using Theater.Abstractions.PieceWorkers;
using Theater.Abstractions.TheaterWorker;
using Theater.Abstractions.Ticket;
using Theater.Abstractions.UserAccount;
using Theater.Contracts.Theater;
using Theater.Core.Authorization;
using Theater.Core.Theater;
using Theater.Core.Theater.PieceWorkers;
using Theater.Core.Theater.Validators;
using Theater.Core.Ticket;
using Theater.Core.UserAccount;
using Theater.Sql;
using Theater.Sql.QueryBuilders;
using Theater.Sql.Repositories;
using IndexReader = Theater.Sql.IndexReader<Theater.Entities.Theater.PieceEntity, Theater.Abstractions.Filter.PieceFilterSettings>;

namespace Theater.Core
{
    public sealed class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // TODO: убрать Autofac и перейти на ServiceCollection
            // TODO: после доработать AddCrudServices 

            builder.RegisterType<JwtHelper>()
                .As<IJwtHelper>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceService>()
                .As<IPieceService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PieceRepository>()
                .As<IPieceRepository>()
                .InstancePerLifetimeScope(); 
            
            builder.RegisterType<PieceDateRepository>()
                .As<IPieceDateRepository>()
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
            
            builder.RegisterType<PieceWorkersRepository>()
                .As<IPieceWorkersRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceTicketService>()
                .As<IPieceTicketService>()
                .InstancePerLifetimeScope(); 
            
            builder.RegisterType<TicketRepository>()
                .As<ITicketRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceDateService>()
                .As<IPieceDateService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceWorkersService>()
                .As<IPieceWorkersService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PiecesDateValidator>()
                .As<IDocumentValidator<PieceDateParameters>>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PieceWorkersValidator>()
                .As<IDocumentValidator<PieceWorkerParameters>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PieceQueryBuilder>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .Register(p => new IndexReader(
                    p.Resolve<TheaterDbContext>(), 
                    p.Resolve<PieceQueryBuilder>(), 
                    p.Resolve<IPieceRepository>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}