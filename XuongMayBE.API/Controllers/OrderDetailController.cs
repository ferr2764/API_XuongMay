using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.OrderDetailModelView;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
            if (orderDetails == null)
            {
                return NotFound("Order details not found.");
            }
            return Ok(orderDetails);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] CreateOrderDetailModelView model)
        {
            if (model == null)
            {
                return BadRequest("Invalid Order Detail data.");
            }

            try
            {
                var response = await _orderDetailService.CreateOrderDetailAsync(model);
                return CreatedAtAction(nameof(GetOrderDetailsByOrderId), new { id = response.OrderId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(Guid id, [FromBody] UpdateOrderDetailModelView orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("Invalid order detail data.");
            }

            try
            {
                var updatedOrderDetail = await _orderDetailService.UpdateOrderDetailAsync(id, orderDetail);
                return updatedOrderDetail != null ? Ok(updatedOrderDetail) : NotFound("Order detail not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(Guid id)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetailAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
