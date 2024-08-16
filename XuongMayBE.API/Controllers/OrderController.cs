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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello, world!");
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
    }
}
