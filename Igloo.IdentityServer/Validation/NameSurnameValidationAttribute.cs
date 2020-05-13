using System;
using System.ComponentModel.DataAnnotations;

namespace Igloo.IdentityServer.Validation
{
    public class NameSurnameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var nameSurname = (string)value ?? string.Empty;
            var partitions = nameSurname.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries).Length;
            return partitions == 2
                ? ValidationResult.Success
                : new ValidationResult("User name must be in format 'Name Surname'.");
        }
    }
}
