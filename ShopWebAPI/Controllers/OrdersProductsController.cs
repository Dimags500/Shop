using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ShopDAL;

namespace ShopWebAPI
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
            var Order = FindOrder(orderID);
            if (Order != null)
                return Order;
            else
                return StatusCode(404);

        }



        //[HttpPost("add/")]
        //public ActionResult AddOrder([FromBody] Order order)
        //{
        //    logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}");
        //    var orderList = FindOrder(order.OrderID);

        //    if (orderList != null)
        //    {
        //        foreach (var item in orderList)
        //        {
        //            if (item == order.ProductId)
        //            {
        //                return StatusCode(400);
        //            }
        //        }
        //    }


        //    ShopContext.Orders.Add(order);
        //    ShopContext.SaveChanges();
        //    return Created($"{Request.Scheme}//:{Request.Host}/addUser{order.OrderId}", order);



        //}

        //[HttpPut("update/")]
        //public ActionResult UpdateOrders([FromBody] Order order)
        //{
        //    Order newOrder = order;
        //    List<Order> oldOrder = FindOrder(order.OrderId);

        //    if (oldOrder != null)
        //    {
        //        foreach (var item in oldOrder)
        //        {
        //            if (item.ProductId == newOrder.ProductId)
        //            {
        //                item.Count = newOrder.Count;
        //                ShopContext.SaveChanges();
        //                return StatusCode(202, newOrder);
        //            }
        //        }

        //    }

        //    return StatusCode(404);
        //}

        [HttpDelete("delete/{orderId}")]
        public ActionResult DeleteOrder(int orderId)
        {

            var orderList = FindOrder(orderId);
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



        List<OrderProduct> FindOrder(int orderID)
        {
            return ShopContext.OrdersProducts.Where(x => x.OrderID == orderID).ToList();
        }
    }
}
