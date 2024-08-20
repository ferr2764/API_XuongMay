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
        private readonly IOrderDetailService _orderDetailService;

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
        public async Task<IActionResult> GetAllOrderDetails(int pageNumber = 1, int pageSize = 5)
        {
            var pagedOrderDetails = await _orderDetailService.GetPaginatedOrderDetailsAsync(pageNumber, pageSize);
            return Ok(pagedOrderDetails);
        }

        /// <summary>
        /// Get order details by order ID.
        /// </summary>
        /// <returns>A list of order details for a specific order.</returns>
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

            try
            {
                var createdOrderDetail = await _orderDetailService.CreateOrderDetailAsync(orderDetail);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = createdOrderDetail.Id.ToString() }, createdOrderDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing order detail.
        /// </summary>
        /// <param name="id">The ID of the order detail to update.</param>
        /// <param name="orderDetail">The updated order detail data.</param>
        /// <returns>The updated order detail.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(string id, [FromBody] OrderDetail orderDetail)
        {
            if (orderDetail == null || id != orderDetail.Id.ToString())
            {
                return BadRequest("Invalid Order Detail data or ID mismatch.");
            }

            try
            {
                var updatedOrderDetail = await _orderDetailService.UpdateOrderDetailAsync(id, orderDetail);
                if (updatedOrderDetail == null)
                {
                    return NotFound("Order detail not found.");
                }

                return Ok(updatedOrderDetail);
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
                var response = await _orderDetailService.CancelOrderDetailAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status/next")]
        public async Task<IActionResult> MoveToNextStatus(string id)
        {
            try
            {
                var orderDetail = await _orderDetailService.MoveToNextStatusAsync(id);
                if (orderDetail == null)
                {
                    return BadRequest("Unable to move to the next status. Order detail may be completed or cancelled.");
                }

                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
