using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BedeSlots.Common.CustomAttributes
{
    public class EnoughMoneyAtribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "The date should be in the future!";
        }

        //protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        //{
           

        //    //if (dateValue <= DateTime.Now)
        //    //{
        //    //    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        //    //}
        //    //return ValidationResult.Success;
        //}
    }
}
