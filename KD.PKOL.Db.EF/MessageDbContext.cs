using KD.PKOL.Models.Dtos;
using System.Data.Entity;
using System.Linq;

namespace KD.PKOL.Db.EF
{
    public class MessageDbContext : DbContext, IDbContext
    {
        public DbSet<MessageDto> Messages { get; set; }

        public MessageDbContext() : base("messageConnectionString")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MessageDbContext>());
        }

        public void Create<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> Get<TEntity>()
            where TEntity : class
        {
            return this.Set<TEntity>();
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}
