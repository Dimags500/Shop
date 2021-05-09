//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Extensions.Logging;
//using System.Reflection;

//namespace ShopAPI.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class CustomersController : ControllerBase
//    {

//        private EfDebilContext ShopContext;
//        private ILogger<CustomersController> logger;
//        public CustomersController(EfDebilContext context , ILogger<CustomersController> Logger)
//        {
//            ShopContext = context;
//            logger = Logger;
//        }

//        [HttpGet("")]
//        public ActionResult<List<Customer>> GetAllcustomers()
//        {
//            return Ok(ShopContext.Customers);

//        }

//        [HttpGet("{customerID}")]
//        public ActionResult<Customer> GetCustomer(int customerID)
//        {
//            var Customer = FindCustomer(customerID);
//            if (Customer != null)
//                return Customer;
//            else
//                return StatusCode(404);

//        }


//        [HttpPost("add/")]
//        public ActionResult AddCustomer([FromBody] Customer customer)
//        {

//            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}");
//            if (FindCustomer(customer.CustomerId) == null)
//            {
//                ShopContext.Customers.Add(customer);
//                ShopContext.SaveChanges();
//                return Created($"{Request.Scheme}//:{Request.Host}/addUser{customer.CustomerId}", customer);
//            }
//            else
//                return StatusCode(400);
//        }

//        [HttpPut("update/")]
//        public ActionResult UpdateCustomer([FromBody] Customer customer)
//        {
//            Customer newCustomer = customer;
//            Customer oldCustomer = FindCustomer(customer.CustomerId);

//            if (oldCustomer != null)
//            {
//                // ShopContext.Customers.Add(newCustomer).Select(x => x.CustomerId == oldCustomer.CustomerId);

//                oldCustomer.Name = newCustomer.Name;
//                ShopContext.SaveChanges();
//                return StatusCode(202 ,newCustomer);
//            }
//            else
//                return StatusCode(404);
//        }



//        [HttpDelete("delete/{customerId}")]
//        public ActionResult DeleteCustomer(int customerId)
//        {
//            ShopContext.Customers.Remove(FindCustomer(customerId));
//            ShopContext.SaveChanges();
//            return StatusCode(200);
//        }


//        Customer FindCustomer(int customerID)
//        {
//            return ShopContext.Customers.FirstOrDefault(x => x.CustomerId == customerID);


//        }
//    }
//}
