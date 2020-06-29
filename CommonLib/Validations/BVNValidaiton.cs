using System.ComponentModel.DataAnnotations;

namespace CommonLib.Validations
{
    public sealed class BVNValidaiton : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var accno = (object)validationContext.ObjectInstance;
           
            if (!Utility.ValidateBVN(value.ToString()))
            {
                return new ValidationResult("BVN is not valid");
            }
         

            return ValidationResult.Success;
        }


    }
}

