using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        public IActionResult OrderIndex()
        {
            return View();
        }
        [HttpGet]
      
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            if (!User.IsInRole(Details.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDto response = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
                switch(status)
                {
                    case "approved": list = list.Where(o => o.Status == Details.Status_Approved);
                        break;
                    case "readyforpickup":
                        list = list.Where(o => o.Status == Details.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list.Where(o => o.Status == Details.Status_Cancelled);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new { data = list });
        }
        [HttpGet]
        public async Task<ActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            var response = await _orderService.GetOrder(orderId);
        


            if (response != null && response.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            }
           if(!User.IsInRole(Details.RoleAdmin) && userId!= orderHeaderDto.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDto);
        }


        [HttpPost("OrderReadyForPickup")]
        public async Task<ActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId,Details.Status_ReadyForPickup);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status Updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }
        [HttpPost("CancelOrder")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, Details.Status_Cancelled);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status Updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }
        [HttpPost("CompleteOrder")]
        public async Task<ActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, Details.Status_Completed);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status Updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }
    }
}
