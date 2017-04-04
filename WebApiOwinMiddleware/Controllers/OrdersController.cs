namespace WebApiOwinMiddleware.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using WebApiOwinMiddleware.Models;
    using WebApiOwinMiddleware.Repositories;

    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IRepository<Order> ordersRepository;

        public OrdersController()
        {
            this.ordersRepository = new LocalRepository<Order>();
        }

        // GET api/<controller>
        [HttpGet]
        public Order[] Get()
        {
            return this.ordersRepository.GetAll();
        }

        // GET api/<controller>/30049d31-f29e-447e-a6c3-cff767b2dc6d
        [HttpGet]
        [Route("{id:Guid}")]
        public IHttpActionResult Get(Guid id)
        {
            var order = this.ordersRepository.FindById(id);
            if (order != null)
            {
                return this.Ok(order);
            }

            return this.NotFound();
        }

        // HEAD api/<controller>/30049d31-f29e-447e-a6c3-cff767b2dc6d
        [HttpHead]
        [Route("{id:Guid}")]
        public IHttpActionResult Head(Guid id)
        {
            var orderExists = this.ordersRepository.Exists(id);
            if (orderExists)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        //// POST api/<controller>
        [HttpPost]
        public IHttpActionResult Create([FromBody]Order order)
        {
            try
            {
                order.DateCreated = DateTime.Now;
                this.ordersRepository.Add(order);

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
            catch
            {
                return this.Conflict();
            }
        }

        // PUT api/<controller>/30049d31-f29e-447e-a6c3-cff767b2dc6d
        [HttpPut]
        [Route("{id:Guid}")]
        public IHttpActionResult Update(Guid id, [FromBody]Order order)
        {
            var foundOrder = this.ordersRepository.FindById(id);
            if (foundOrder != null)
            {
                foundOrder.Category = order.Category;
                foundOrder.Name = order.Name;
                this.ordersRepository.Update(foundOrder);
                return this.Ok();
            }

            return this.NotFound();
        }

        // DELETE api/<controller>/30049d31-f29e-447e-a6c3-cff767b2dc6d
        [HttpDelete]
        [Route("{id:Guid}")]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                this.ordersRepository.Delete(id);
                return this.Ok();
            }
            catch
            {
                return this.NotFound();
            }
        }

        // PATCH api/<controller>30049d31-f29e-447e-a6c3-cff767b2dc6d/category
        [HttpPatch]
        [Route("{id:Guid}/{field}")]
        public IHttpActionResult UpdateName(Guid id, string field, [FromBody]object value)
        {
            var order = this.ordersRepository.FindById(id);
            if (order == null)
            {
                return this.NotFound();
            }

            switch (field.ToLower())
            {
                case "name":
                    order.Name = (string)value;
                    break;
                case "category":
                    order.Category = (string)value;
                    break;
                default:
                    return this.BadRequest("'" + field + "' cannot be found.");
            }

            this.ordersRepository.Update(order);
            return this.Ok();
        }

        [HttpPost]
        [Route("createLazy")]
        public HttpResponseMessage CreateLazy([FromBody] Order order)
        {
            try
            {
                order.DateCreated = DateTime.Now;
                this.ordersRepository.Add(order);

                new System.Threading.Tasks.Task(() =>
                {
                    // long creating operation and object is not created here yet
                    System.Threading.Thread.Sleep(5000);

                }).Start();

                // return immediately with order location where it will be when created
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Accepted);
                string url = this.Url.Link("DefaultApi", new { controller = "Orders", id = order.Id.ToString() });
                responseMessage.Headers.Location = new Uri(url);
                return responseMessage;

                //string url = this.Url.Route("DefaultApi", new { controller = "Orders", id = order.DisplayGuid.ToString() });
                //responseMessage.Headers.Location = new Uri(this.Request.RequestUri + "/" + order.DisplayGuid.ToString());
            }
            catch
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
}