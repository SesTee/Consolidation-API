using System.ComponentModel.DataAnnotations;

namespace CommonLib.Validations
{
    public sealed class CifValidaiton : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var accno = (object)validationContext.ObjectInstance;
           
            if (!Utility.ValidateCIF(value.ToString()))
            {
                return new ValidationResult("CIF is not valid");
            }
         

            return ValidationResult.Success;
        }


    }
}

