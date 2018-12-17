using DryIoc;
using KD.PKOL.Db;
using KD.PKOL.Db.EF;

namespace KD.PKOL.Services
{
    public class DependencyContainer
    {
        private static DependencyContainer instance;
        public static DependencyContainer INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new DependencyContainer();
                }
                return instance;
            }
        }

        private Container Container { get; }

        private DependencyContainer()
        {
            this.Container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            this.RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            this.Container.Register<IDbContext, MessageDbContext>();
        }

        public TType Resolve<TType>()
        {
            return this.Container.Resolve<TType>();
        }
    }
}