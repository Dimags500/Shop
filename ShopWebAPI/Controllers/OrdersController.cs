using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopDAL;
using System.Reflection;

namespace ShopWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {

        private ShopContext ShopContext;
        private ILogger<OrdersController> logger;

        public OrdersController(ShopContext context, ILogger<OrdersController> Logger)
        {
            ShopContext = context;
            logger = Logger;
        }


        [HttpGet("")]
        public ActionResult<List<Order>> GetAllorders()
        {
            return Ok(ShopContext.Orders);
        }


        [HttpGet("{orderID}")]
        public ActionResult<Order> GetOrder(int orderID)
        {
            Order orderToFind = FindOrder(orderID);
            if (orderToFind != null)
                return orderToFind;
            else
                return StatusCode(404);
        }

        [HttpPost("add")]
        public ActionResult AddOrder([FromBody] Order order)
        {

            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}");
            if (FindOrder(order.OrderID) == null)
            {
                ShopContext.Orders.Add(order);
                ShopContext.SaveChanges();
                return Created($"{Request.Scheme}//:{Request.Host}/addUser{order.OrderID}", order);
            }
            else
                return StatusCode(400);
        }

        [HttpPut("update")]
        public ActionResult UpdateOrder([FromBody] Order order)
        {
            Order newOrder = order;
            Order oldOrder = FindOrder(order.OrderID);

            if (oldOrder != null)
            {
                oldOrder.UserID = newOrder.UserID;
                oldOrder.Created = newOrder.Created;
                ShopContext.SaveChanges();
                return StatusCode(202, newOrder);
            }
            return StatusCode(404);
        }

        [HttpDelete("delete/{orderID}")]
        public ActionResult DeleteOrder(int orderID)
        {
            ShopContext.Orders.Remove(FindOrder(orderID));
            ShopContext.SaveChanges();
            return StatusCode(200);
        }

        Order FindOrder(int orderID)
        {
            return ShopContext.Orders.FirstOrDefault(o =>o.OrderID==orderID);
        }

    }
}
