namespace WebApiOwinMiddleware.Controllers
{
    using System;
    using System.Web.Http;
    using Models;
    using Repositories;

    [Authorize]
    public class CustomersController : ApiController
    {
        private readonly IRepository<Customer> customerRepository;

        public CustomersController()
        {
            this.customerRepository = new LocalRepository<Customer>();
        }

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var customers = this.customerRepository.GetAll();

            return this.Ok(customers);
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(Guid id)
        {
            var customer = this.customerRepository.FindById(id);
            if (customer != null)
            {
                return this.Ok(customer);
            }

            return this.NotFound();
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}