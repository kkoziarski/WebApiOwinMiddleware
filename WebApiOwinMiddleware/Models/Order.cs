namespace WebApiOwinMiddleware.Models
{
    using System;

    public class Order : Entity
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
