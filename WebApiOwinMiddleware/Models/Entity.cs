namespace WebApiOwinMiddleware.Models
{
    using System;
    using LiteDB;

    public abstract class Entity
    {
        [BsonId]
        public Guid Id { get; set; }

        protected Entity()
        {
            this.Id = Guid.NewGuid();
        }
    }
}