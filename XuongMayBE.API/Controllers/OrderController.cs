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
        public async Task<IActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 5)
        {
            var pagedOrders = await _orderService.GetPaginatedOrdersAsync(pageNumber, pageSize);
            return Ok(pagedOrders);
        }


        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="model">The order details for creation.</param>
        /// <returns>The created order details.</returns>
        //[Authorize(Roles = "Manager")]
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
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] Order order)
        {
            if (order == null || id != order.Id.ToString())
            {
                return BadRequest("Invalid order data or ID mismatch.");
            }

            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, order);
                if (updatedOrder == null)
                {
                    return NotFound("Order not found.");
                }

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            try
            {
                var response = await _orderService.CancelOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Move an order to the next status in the workflow.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <returns>The updated order with the new status.</returns>
        [HttpPut("{id}/status/next")]
        public async Task<IActionResult> MoveToNextStatus(string id)
        {
            try
            {
                var order = await _orderService.MoveToNextStatusAsync(id);
                if (order == null)
                {
                    return BadRequest("Unable to move to the next status. Order may be completed or invalid.");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
