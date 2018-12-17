using System.Linq;

namespace KD.PKOL.Db
{
    public interface IDbContext
    {
        void Create<TEntity>(TEntity entity) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Save();
    }
}
