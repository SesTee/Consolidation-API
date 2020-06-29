using System.ComponentModel.DataAnnotations;

namespace CommonLib.Validations
{
    public sealed class AccountNoValidation : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var accno = (object)validationContext.ObjectInstance;
           
            if (!Utility.ValidateAccountNo(value.ToString()))
            {
                return new ValidationResult("Account number is not valid");
            }
         

            return ValidationResult.Success;
        }


    }
}

