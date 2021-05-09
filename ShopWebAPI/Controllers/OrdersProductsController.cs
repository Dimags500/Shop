using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ShopDAL;

namespace ShopWebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrdersProductsController : ControllerBase
    {
        private ShopContext ShopContext;
        private ILogger<OrdersProductsController> logger;

        public OrdersProductsController(ShopContext context, ILogger<OrdersProductsController> Logger)
        {
            ShopContext = context;
            logger = Logger;
        }

        [HttpGet("")]

        public ActionResult<List<Order>> GetAllOrders()
        {
            return Ok(ShopContext.OrdersProducts);
        }

        [HttpGet("{orderID}")]
        public ActionResult<List<OrderProduct>> GetOrder(int orderID)
        {
            var Orders = FindOP(orderID);

            if (Orders != null)
                return Orders;
            else
                return StatusCode(404);

        }



        [HttpPost("add/")]
        public ActionResult AddOrder([FromBody] OrderProduct order)
        {
            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}");
            var orderList = FindOP(order.OrderID);

            if (orderList != null)
            {
                foreach (var item in orderList)
                {
                    if (item.ProductID == order.ProductID)
                    {
                        return StatusCode(400);
                    }
                }
            }


            ShopContext.OrdersProducts.Add(order);
            ShopContext.SaveChanges();
            return Created($"{Request.Scheme}//:{Request.Host}/addUser{order.OrderID}", order);



        }

        [HttpPut("update/")]
        public ActionResult UpdateOrders([FromBody] OrderProduct order)
        {
            OrderProduct newOrder = order;
            var oldOrder = FindOP(order.OrderID);

            if (oldOrder != null)
            {
                foreach (var item in oldOrder)
                {
                    if (item.ProductID == newOrder.ProductID)
                    {
                        item.ProductCount = newOrder.ProductCount;
                        ShopContext.SaveChanges();
                        return StatusCode(202, newOrder);
                    }
                }

            }

            return StatusCode(404);
        }

        [HttpDelete("delete/{orderId}")]
        public ActionResult DeleteOrder(int orderId)
        {

            var orderList = FindOP(orderId);
            if (orderList != null)
            {
                foreach (var item in orderList)
                {

                    ShopContext.OrdersProducts.Remove(item);
                    ShopContext.SaveChanges();

                }
                return StatusCode(200);

            }
            return StatusCode(404);

        }



        List<OrderProduct> FindOP(int orderID)
        {
            return ShopContext.OrdersProducts.Where(x => x.OrderID == orderID).ToList();
        }
    }
}
