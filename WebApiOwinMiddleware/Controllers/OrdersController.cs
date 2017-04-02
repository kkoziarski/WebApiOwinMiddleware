using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApiOwinMiddleware.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private static readonly List<Order> Orders = new List<Order>
        {
            new Order { Id = 1, Name = "Order1", Category = "C1", DisplayGuid = new Guid("30049d31-f29e-447e-a6c3-cff767b2dc6d") },
            new Order { Id = 2, Name = "Order2", Category = "C2", DisplayGuid = new Guid("145e9a65-7594-403d-a829-d20c089cafa0") },
            new Order { Id = 3, Name = "Order3", Category = "C3", DisplayGuid = new Guid("ff74d120-36ee-4c96-9d48-3a6cd8804d94") },
            new Order { Id = 4, Name = "Order4", Category = "C4", DisplayGuid = new Guid("4954c83c-0f22-4e41-a303-1e359d00d9b1") }
        };

        // GET api/<controller>
        [HttpGet]
        public List<Order> Get()
        {
            return Orders;
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var order = Orders.SingleOrDefault(t => t.Id == id);
            if (order != null)
            {
                return this.Ok(order);
            }

            return this.NotFound();
        }

        [HttpGet]
        [Route("{guid:Guid}")]
        public IHttpActionResult Get(Guid guid)
        {
            var order = Orders.SingleOrDefault(t => t.DisplayGuid == guid);
            if (order != null)
            {
                return this.Ok(order);
            }

            return this.NotFound();
        }

        [HttpHead]
        [Route("{id:int}")]
        public IHttpActionResult Head(int id) // doesn't work yet
        {
            var orderExists = Orders.Any(t => t.Id == id);

            if (orderExists)
            {
                return this.Ok();
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
                string url = this.Url.Link("DefaultApi", new { controller = "Orders", id = order.Id.ToString() });
                return this.Created<Order>(url, order);
                //return this.Created<Order>(this.Request.RequestUri + "/" + order.Id.ToString(), order);

                /*
                 * Equivalent
                var msg = new HttpResponseMessage(HttpStatusCode.Created);
                msg.Headers.Location = new Uri(this.Request.RequestUri + "/" + order.Id.ToString());
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
        [Route("{id:int}")]
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
        [Route("{id:int}")]
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

        [HttpPatch]
        [Route("name/{id}")]
        public IHttpActionResult UpdateName(int id, [FromBody]string name)
        {
            // TODO: finish
            return this.Ok(name);
        }

        [HttpPost]
        [Route("createLazy")]
        public HttpResponseMessage CreateLazy([FromBody] Order order)
        {
            order.Id = Orders.Max(x => x.Id) + 1;
            order.DisplayGuid = Guid.NewGuid();
            Orders.Add(order);
            bool added = true;
            if (added)
            {
                new System.Threading.Tasks.Task(
                    () =>
                        {
                            // long creating operation and object is not created here yet
                            System.Threading.Thread.Sleep(5000);

                        }).Start();

                // return immediately with order location where it will be when created
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Accepted);
                string url = this.Url.Link("DefaultApi", new { controller = "Orders", id = order.DisplayGuid.ToString() });
                responseMessage.Headers.Location = new Uri(url);
                return responseMessage;

                //string url = this.Url.Route("DefaultApi", new { controller = "Orders", id = order.DisplayGuid.ToString() });
                //responseMessage.Headers.Location = new Uri(this.Request.RequestUri + "/" + order.DisplayGuid.ToString());
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            /*
            202 Accepted
            POST http://localhost:5000/api/documents HTTP/1.1
            Content-type: application/json
            Host: localhost:5000
            Content-Length: 47

            {
             "author": "John Doe",
             "content" : "..."
            }
            -------------------------------------------------------
            HTTP/1.1 202 Accepted
            Date: Sun, 09 Oct 2016 19:19:54 GMT
            Location: api/documentsqueue/cf67eca7-e822-44bb-a185-bdc262fbe52b
            Server: Kestrel
            */
        }
    }

    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public Guid DisplayGuid { get; set; }
    }
}