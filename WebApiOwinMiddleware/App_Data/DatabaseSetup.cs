namespace WebApiOwinMiddleware.App_Data
{
    using System;
    using System.Collections.Generic;

    using LiteDB;

    using WebApiOwinMiddleware.Configuration;
    using WebApiOwinMiddleware.Models;

    public static class DatabaseSetup
    {
        private static readonly string DatabaseFilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + ConfigurationProvider.DBFileName);

        public static void Initialize()
        {
            using (var db = new LiteDatabase(DatabaseFilePath))
            {
                var customersExist = db.CollectionExists("Customer");
                if (!customersExist)
                {
                    db.GetCollection<Customer>().Insert(InitCustomers);
                }
                else
                {
                    var customers = db.GetCollection<Customer>();
                    foreach (var cust in InitCustomers)
                    {
                        var dbCustomer = customers.FindById(cust.Id);
                        if (dbCustomer != null)
                        {
                            customers.Update(dbCustomer);
                        }
                        else
                        {
                            customers.Insert(cust);
                        }
                    }
                }

                var ordersExists = db.CollectionExists("Order");
                if (!ordersExists)
                {
                    db.GetCollection<Order>().Insert(InitOrders);
                }
                else
                {
                    var orders = db.GetCollection<Order>();
                    foreach (var order in InitOrders)
                    {
                        var dbOrder = orders.FindById(order.Id);
                        if (dbOrder != null)
                        {
                            orders.Update(dbOrder);
                        }
                        else
                        {
                            orders.Insert(order);
                        }
                    }
                }
            }
        }

        private static  readonly List<Customer> InitCustomers = new List<Customer>
        {
            new Customer { Name = "John Doe", Phones = new string[] { "8000-0000", "9000-0000" }, IsActive = true, Id = new Guid("6c7db6d8-fa43-4f6f-9e02-1ca395e9cae5")},
            new Customer { Name = "Roger Moss", Phones = new string[] { "6000-1000", "7000-2000" }, IsActive = true, Id = new Guid("5f60087d-43d9-4293-8516-a7aff11ef89b")},
            new Customer { Name = "Jenny Ingram", Phones = new string[] { "4000-2000", "5000-3000" }, IsActive = false, Id = new Guid("b16934bf-41e0-44d0-a674-46d7a3505e79")},
            new Customer { Name = "Tammy Shelton", Phones = new string[] { "2000-4000", "3000-5000" }, IsActive = true, Id = new Guid("f8dab8a1-c6e7-4178-b986-9d7154a09254")}
        };

        private static readonly List<Order> InitOrders = new List<Order>
        {
            new Order { Name = "Order 1", Category = "C1", Id = new Guid("30049d31-f29e-447e-a6c3-cff767b2dc6d"), DateCreated = DateTime.Now },
            new Order { Name = "Order 2", Category = "C2", Id = new Guid("145e9a65-7594-403d-a829-d20c089cafa0"), DateCreated = DateTime.Now },
            new Order { Name = "Order 3", Category = "C3", Id = new Guid("ff74d120-36ee-4c96-9d48-3a6cd8804d94"), DateCreated = DateTime.Now },
            new Order { Name = "Order 4", Category = "C4", Id = new Guid("4954c83c-0f22-4e41-a303-1e359d00d9b1"), DateCreated = DateTime.Now }
        };
    }
}
