namespace Shop.Services.AuthApi.Model.Dto
{
    public class UserDTO
    {
       // as .net entity userid
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumer { get; set; }

    }
}
