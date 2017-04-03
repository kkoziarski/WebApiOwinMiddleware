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
            this.DatabaseFilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + ConfigurationProvider.DBFileName);
        }

        public TEntity FindById(Guid id)
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                return db.GetCollection<TEntity>().FindById(id);
            }
        }

        public bool Exists(Guid id)
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                return db.GetCollection<TEntity>().FindById(id) != null;
            }
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
                var addResult = db.GetCollection<TEntity>().Insert(entity);
                if (addResult == null)
                {
                    throw new Exception("Entity not added");
                }
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

        public void Delete(Guid id)
        {
            using (var db = new LiteDatabase(this.DatabaseFilePath))
            {
                var deleteResult = db.GetCollection<TEntity>().Delete(id);
                if (!deleteResult)
                {
                    throw new Exception("Entity not found");
                }
            }
        }

        public LiteDatabase CreateDatabaseConnection()
        {
            return new LiteDatabase(this.DatabaseFilePath);
        }
    }
}