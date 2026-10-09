using Autofac;
using BasitDestekTalebiSistemi.Business.Abstract;
using BasitDestekTalebiSistemi.Business.Concrete;
using BasitDestekTalebiSistemi.DataAccess.Abstract;
using BasitDestekTalebiSistemi.DataAccess.Concrete.EntityFramework;

namespace BasitDestekTalebiSistemi.Business.DependencyResolvers.Autofac
{

    public class AutofacBusinessModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<TicketManager>().As<ITicketService>().InstancePerLifetimeScope();

            builder.RegisterType<EfTicketDal>().As<ITicketDal>().InstancePerLifetimeScope();

            builder.RegisterType<ReplyManager>().As<IReplyService>().InstancePerLifetimeScope();

            builder.RegisterType<EfReplyDal>().As<IReplyDal>().InstancePerLifetimeScope();

            builder.RegisterType<AppDbContext>().InstancePerLifetimeScope();
        }
    }
}
