namespace WebApiOwinMiddleware.Repositories
{
    using System;

    using LiteDB;

    using Models;

    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity[] GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        LiteDatabase CreateDatabaseConnection();

        TEntity FindById(Guid id);

        bool Exists(Guid id);
        void Delete(Guid id);
    }
}