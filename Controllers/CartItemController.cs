using CartAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private static List<CartItem> items = new List<CartItem>()
    {
        new CartItem(){ Id=1, Product="Laptop", Price=1200.50, Quantity=1 },
        new CartItem(){ Id=2, Product="Mouse", Price=25.99, Quantity=2 },
        new CartItem(){ Id=3, Product="Keyboard", Price=45.00, Quantity=1 }
    };
        private static int nextId = 4;

        [HttpGet()]
        public IActionResult GetAll(int? maxprice = null, string prefix = null, int? pageSize = null)
        {
            List<CartItem> result = items;
            if(maxprice != null)
            {
                result = result.Where(i => i.Price <= maxprice).ToList();
            }
            if (!string .IsNullOrEmpty(prefix))
            {
                result = result.Where(i =>i.Product.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (pageSize != null && pageSize>0)
            {
                result = result.Take(pageSize.Value).ToList();
            }
            return Ok(result);
        }
//       $ curl -k -X 'GET' \
//  'https://localhost:7154/api/CartItem?maxprice=100&prefix=m&pageSize=2' \
//  -H 'accept: */*'
//Request URL
//  % Total    % Received % Xferd Average Speed Time    Time Time  Current
//                                 Dload  Upload Total   Spent Left  Speed
//100    55    0    55    0     0   2468      0 --:--:-- --:--:-- --:--:--  2500[{"id":2,"product":"Mouse","price":25.99,"quantity":2}]
//bash: Request: command not found


//        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CartItem c = items.FirstOrDefault(i => i.Id == id);
            if (c == null)
            {
                return NotFound("ID Not Found");
            }
            else
            {
                return Ok(c);
            }
        }

        //        $ curl -k -X 'GET' \
        //  'https://localhost:7154/api/CartItem/3' \
        //  -H 'accept: */*'
        //  % Total    % Received % Xferd Average Speed Time    Time Time  Current
        //                                 Dload  Upload Total   Spent Left  Speed
        //100    53    0    53    0     0   2854      0 --:--:-- --:--:-- --:--:--  2944{"id":3,"product":"Keyboard","price":45,"quantity":1}

        [HttpPost()]
        public IActionResult AddItems([FromBody] CartItem newCartItem)
        {
            newCartItem.Id = nextId;
            items.Add(newCartItem);
            nextId++;
            return Created($"/api/CartItem/{newCartItem.Id}",newCartItem);
        }
        //        $ curl -k -X 'POST' \
        //  'https://localhost:7154/api/CartItem' \
        //  -H 'accept: */*' \
        //  -H 'Content-Type: application/json' \
        //  -d '{
        //  "id": 0,
        //  "product": "Monitor",
        //  "price": 200,
        //  "quantity": 1
        //}'
        //  % Total    % Received % Xferd Average Speed Time    Time Time  Current
        //                                 Dload  Upload Total   Spent Left  Speed
        //100   123    0    53  100    70   2290   3024 --:--:-- --:--:-- --:--:--  5347{"id":5,"product":"Monitor","price":200,"quantity":1}

        [HttpDelete()]
        public IActionResult DeleteById(int id)
        {
            int index = items.FindIndex(i => i.Id == id);
            if (index == -1)
            {
                return NotFound("ID not found");
            }
            else
            {
                items.RemoveAt(index);
                return NoContent();
            }
        }
        //        $ curl -k -X 'DELETE' \
        //  'https://localhost:7154/api/CartItem?id=1' \
        //  -H 'accept: */*'
        //  % Total    % Received % Xferd Average Speed Time    Time Time  Current
        //                                 Dload  Upload Total   Spent Left  Speed
        //100    12    0    12    0     0    454      0 --:--:-- --:--:-- --:--:--   461ID not found

        [HttpPut()]
        public IActionResult UpdateCart(int id, [FromBody] CartItem updatedCart)
        {
            if (id != updatedCart.Id)
            {
                return BadRequest();
            }
            if(!items.Any(i => i.Id == id)) { return NotFound(); }
            int index = items.FindIndex(i => i.Id == id);
            items[index] = updatedCart;
            return Ok(updatedCart);
        }
  //      curl -X 'PUT' \
  //'https://localhost:7154/api/CartItem?id=1' \
  //-H 'accept: */*' \
  //-H 'Content-Type: application/json' \
  //-d ' {
  //  "id": 1,
  //  "product": "Laptop",
  //  "price": 1200.5,
  //  "quantity": 2
  //}'
    }
}
