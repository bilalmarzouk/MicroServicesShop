using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Utility
{
    public class AllowedExtentionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtentions;
        public AllowedExtentionsAttribute(string[] allowedExtentions)
        {
            _allowedExtentions = allowedExtentions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if(file != null)
            {
                var extention = Path.GetExtension(file.FileName);
                if(!_allowedExtentions.Contains(extention.ToLower()))
                {
                    return new ValidationResult("This photo extention is not allowed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
