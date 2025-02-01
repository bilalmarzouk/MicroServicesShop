namespace Shop.Web.Utility
{
    public class Details
    {
        public static string CouponAPIBase {  get; set; }
        public static string ProductAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ShoppingCartApi { get; set; }
        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
