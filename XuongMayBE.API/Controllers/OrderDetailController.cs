using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Services.Interface;

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

        [HttpGet("{orderId}")]
        public IActionResult GetOrderDetailByOrderId()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateOrderDetail()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail()
        {
            return Ok();
        }

    }
}
