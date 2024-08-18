using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.OrderModelViews;
using XuongMay.Services;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>The order details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(order);
        }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="model">The order details for creation.</param>
        /// <returns>The created order details.</returns>
        [Authorize(Roles = "Manager")]
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
                return CreatedAtAction(nameof(GetOrderById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public IActionResult UpdateOrder()
        {
            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("assign/{id}")]
        public async Task<IActionResult> AssignOrder([FromBody] AssignOrderModelView assignOrderModelView, string id)
        {
            if (assignOrderModelView == null)
            {
                return BadRequest("Invalid Account data.");
            }

            try
            {
                var response = await _orderService.AssignOrderAsync(assignOrderModelView, id);
                return CreatedAtAction(nameof(GetOrderById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
