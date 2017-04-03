namespace WebApiOwinMiddleware.Models
{
    public class Customer : Entity
    {
        public string Name { get; set; }

        public string[] Phones { get; set; }

        public bool IsActive { get; set; }
    }
}