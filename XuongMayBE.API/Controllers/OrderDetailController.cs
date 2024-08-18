using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.OrderDetailModelView;
using XuongMay.Services.Service;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : Controller
    {
        public readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderDetailById()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllOrderDetail()
        {
            return Ok();
        }

        [HttpGet("order/{orderId}")]
        public IActionResult GetOrderDetailByOrderId()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] CreateOrderDetailModelView orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("Order Detail data is null.");
            }

            var createdOrderDetail = await _orderDetailService.CreateOrderDetailAsync(orderDetail);
            return CreatedAtAction(nameof(GetOrderDetailById), new { id = createdOrderDetail.Id.ToString() }, createdOrderDetail);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail()
        {
            return Ok();
        }

    }
}
