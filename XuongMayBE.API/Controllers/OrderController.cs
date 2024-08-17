using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.OrderModelViews;
using XuongMay.Services;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllOrder()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderModelView model)
        {
            if (model == null)
            {
                return BadRequest("Invalid Order data.");
            }

            try
            {
                var response = await _orderService.CreateOrderAsync(model);
                return Created("Order created successfully.", response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateOrder()
        {
            return Ok();
        }

        [HttpPut("assign/{id}")]
        public IActionResult AssignOrder()
        {
            return Ok();
        }
    }
}
