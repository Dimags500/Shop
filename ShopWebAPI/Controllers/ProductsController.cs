
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ShopDAL;

namespace ShopWebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        private ShopContext ShopContext;
        private ILogger<ProductsController> logger;

        public ProductsController(ShopContext context, ILogger<ProductsController> Logger)
        {
            ShopContext = context;
            logger = Logger;
        }



        [HttpGet("")]
        public ActionResult<List<Product>> GetAllProducts()
        {
            return Ok(ShopContext.Products);
        }


        [HttpGet("{productID}")]
        public ActionResult<Product> GetProduct(int productID)
        {
            Product product = FindProduct(productID);
            if (product != null)
                return product;
            else
                return StatusCode(404);
        }

      

        [HttpPost("add/")]
        public ActionResult AddProduct([FromBody] Product product)
        {

            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}");
            if (FindProduct(product.ProductID) == null)
            {
                ShopContext.Products.Add(product);
                ShopContext.SaveChanges();
                return Created($"{Request.Scheme}//:{Request.Host}/addUser{product.ProductID}", product);
            }
            else
                return StatusCode(400);
        }


        [HttpPut("update/")]
        public ActionResult UpdateProduct([FromBody] Product product)
        {
            Product newProduct = product;
            Product oldProduct = FindProduct(product.ProductID);

            if (oldProduct != null)
            {

                oldProduct.Name = newProduct.Name;
                oldProduct.Value = newProduct.Value;
                ShopContext.SaveChanges();
                return StatusCode(202, newProduct);
            }
            else
                return StatusCode(404);

        }

        [HttpDelete("delete/{productID}")]
        public ActionResult DeleteProduct(int productID)
        {
            Product productToRemove = FindProduct(productID);
            if (productToRemove != null)
            {
                ShopContext.Remove(productToRemove);
                ShopContext.SaveChanges();
                  return StatusCode(200);
            }

            return StatusCode(404);
            
        }


        Product FindProduct(int productID)
        {
            return ShopContext.Products.FirstOrDefault(x => x.ProductID == productID);
        }

    }
}
