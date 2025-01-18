namespace Shop.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CopounCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
