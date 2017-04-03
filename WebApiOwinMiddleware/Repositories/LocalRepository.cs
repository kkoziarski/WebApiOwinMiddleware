namespace WebApiOwinMiddleware.Repositories
{
    using System;
    using System.Linq;
    using Configuration;
    using LiteDB;
    using Models;

    public class LocalRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public readonly string DatabaseFilePath;

        public LocalRepository()
        {
            this.DatabaseFilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + SettingsProvider.DBFileName);
        }

        public TEntity[] GetAll()
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                return db.GetCollection<TEntity>().FindAll().ToArray();
            }
        }

        public void Add(TEntity entity)
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                db.GetCollection<TEntity>().Insert(entity);
            }
        }

        public void Update(TEntity entity)
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                var updateResult = db.GetCollection<TEntity>().Update(entity);
                if (!updateResult)
                {
                    throw new Exception("Entity not found");
                }
            }
        }

        public void Delete(TEntity entity)
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                var deleteResult = db.GetCollection<TEntity>().Delete(entity.Id);
                if (!deleteResult)
                {
                    throw new Exception("Entity not found");
                }
            }
        }
    }
}