using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Get an order detail by ID.
        /// </summary>
        /// <param name="id">The ID of the order detail.</param>
        /// <returns>The order detail.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(string id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return Ok(orderDetail);
        }

        /// <summary>
        /// Get all order details.
        /// </summary>
        /// <returns>A list of all order details.</returns>
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
            return Ok(orderDetails);
        }

        [HttpGet("order/{orderId}")]
        public IActionResult GetOrderDetailByOrderId()
        {
            return Ok();
        }

        /// <summary>
        /// Create a new order detail.
        /// </summary>
        /// <param name="orderDetail">The order detail data to create.</param>
        /// <returns>The created order detail.</returns>
        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail()
        {
            return Ok();
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            try
            {
                var response = await _orderDetailService.CancelOrderDetailAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
