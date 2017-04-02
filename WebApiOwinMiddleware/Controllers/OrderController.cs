using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApiOwinMiddleware.Controllers
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public Guid DisplayGuid { get; set; }
    }

    [Authorize]
    public class OrderController : ApiController
    {
        private static readonly List<Order> Orders = new List<Order>
        {
            new Order { Id = 1, Name = "Order1", Category = "C1", DisplayGuid = Guid.NewGuid() },
            new Order { Id = 2, Name = "Order2", Category = "C2", DisplayGuid = Guid.NewGuid() },
            new Order { Id = 3, Name = "Order3", Category = "C3", DisplayGuid = Guid.NewGuid() },
            new Order { Id = 4, Name = "Order4", Category = "C4", DisplayGuid = Guid.NewGuid() }
        };

        // GET api/<controller>
        [HttpGet]
        public List<Order> Get()
        {
            return Orders;
        }

        // GET api/<controller>/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var order = Orders.SingleOrDefault(t => t.Id == id);
            if (order != null)
            {
                return this.Ok(order);
            }

            return this.NotFound();
        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Create([FromBody]Order order)
        {
            order.Id = Orders.Max(x => x.Id) + 1;
            order.DisplayGuid = Guid.NewGuid();
            Orders.Add(order);
            bool added = true;
            if (added)
            {
                return this.Created<Order>(this.Request.RequestUri + "/" + order.Id.ToString(), order);

                /*
                var msg = new HttpResponseMessage(HttpStatusCode.Created);
                msg.Headers.Location = new Uri(this.Request.RequestUri + order.Id.ToString());
                return msg; 
                */
            }
            else
            {
                return this.Conflict();
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]Order order)
        {
            var foundOrder = Orders.FirstOrDefault(t => t.Id == id);
            if (foundOrder != null)
            {
                foundOrder.Category = order.Category;
                foundOrder.Name = order.Name;
                return this.Ok();
            }

            return this.NotFound();
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var foundOrder = Orders.FirstOrDefault(t => t.Id == id);
            if (foundOrder != null)
            {
                Orders.Remove(foundOrder);
                return this.Ok();
            }

            return this.NotFound();
        }
    }
}