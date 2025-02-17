﻿using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class OrderHeaderDto
    {

        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        public double Discount { get; set; }

        public double OrderTotal { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Status { get; set; }
        public string? PaymentId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }


    }
}