using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CurrentYearRangeAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private int YearMin;
        private int YearMax;
        private int YearUnknown;

        /// <summary>
        /// Property when condition asserts true.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="assertion"></param>
        public CurrentYearRangeAttribute(string propertyName, int yearMin, int yearUnknown)
        {
            PropertyName = propertyName;
            YearMin = yearMin;
            YearMax = DateTime.Now.Year;
            YearUnknown = yearUnknown;
        }

        public CurrentYearRangeAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        protected override ValidationResult IsValid(object propertyValue, ValidationContext propertyToValidate)
        {
            var instance = propertyToValidate.ObjectInstance;
            var instanceType = instance.GetType();
            if(propertyValue is null)
            {
                return ValidationResult.Success;
            }

            int? value = (int)propertyValue;


            var formStatus = instanceType.GetProperty("FormStatus");
            if (formStatus != null)
            {
                var formStatusValue = formStatus.GetValue(instance, null);
                if (formStatusValue.ToString() != "Complete")
                {
                    return ValidationResult.Success;
                }
            }

            if (value >= 0)
            {
                if(value >= YearMin && value <= YearMax) return ValidationResult.Success;

                if(value == YearUnknown) return ValidationResult.Success;

            } else {
                //the value was null, but the a3 form must allow null valid date inputs
                return ValidationResult.Success;
            }

            //Year was not matched with conditions and is not valid
            return new ValidationResult(ErrorMessage);
        }
    }
}

