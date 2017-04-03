using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiOwinMiddleware.Controllers
{
    using Models;
    using Repositories;

    public class CustomersController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var repo = new CustomerRepository();
            var customers = repo.GetAll();

            return this.Ok(customers);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            var repo = new CustomerRepository();
            var customer = new Customer
            {
                Name = "John Doe" + id,
                Phones = new string[] { "8000-0000", "9000-0000" },
                IsActive = true
            };

            repo.Add(customer);

            return "value";
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