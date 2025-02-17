using Microsoft.EntityFrameworkCore;
using Shop.Services.EmailAPI.Data;
using Shop.Services.EmailAPI.Message;
using Shop.Services.EmailAPI.Model;
using Shop.Services.EmailAPI.Model.Dto;
using System.Text;

namespace Shop.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;
        public EmailService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/>Chart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("ul>");

            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append((item.Product.Name + " x " + item.Count));
                message.Append("</ul>");
            }
            message.Append("</ul>");
            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardMessage rewardMessage)
        {
            string message = "New Order Placed" + Environment.NewLine + " OrderId: " + rewardMessage.OrderId;
            await LogAndEmail(message, "Test@admin.at");
        }

        public async Task RegisterUserEmailLog(string email)
        {
            string message = "User Tegistration Successful. <br/> Email: " + email;
            await LogAndEmail(message, "Test@admin.at");
        }

        private async Task <bool> LogAndEmail(string message,string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new ApplicationDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog); ;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
