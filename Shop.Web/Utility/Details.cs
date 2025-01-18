namespace Shop.Web.Utility
{
    public class Details
    {
        public static string CouponAPIBase {  get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
