using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class InvalidRangeAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private int InvalidRangeMin;
        private int InvalidRangeMax;

        /// <summary>
        /// Property when condition asserts true.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="assertion"></param>
        public InvalidRangeAttribute(string propertyName, int invalidRangeMin, int invalidRangeMax, string errorMessage = "")
        {
            PropertyName = propertyName;
            InvalidRangeMax = invalidRangeMax;
            InvalidRangeMin = invalidRangeMin;
        }

        public InvalidRangeAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();

            var propertyValue = type.GetProperty(PropertyName).GetValue(instance);

            var formStatus = type.GetProperty("FormStatus");
            if (formStatus != null)
            {
                var formStatusValue = formStatus.GetValue(instance, null);
                if (formStatusValue.ToString() != "Complete")
                {
                    return ValidationResult.Success; // if the annotation is on a form and it is not being completed, don't run validation
                }
            }

            var invalidMinValue = InvalidRangeMin;
            var invalidMaxValue = InvalidRangeMax;

            if (propertyValue != null) // we're allowing nulls in some cases, so the watched property won't always have a value
            {
                while (invalidMinValue <= invalidMaxValue)
                {
                    if (propertyValue.ToString() == invalidMinValue.ToString() || value == null)
                    {
                        return new ValidationResult(ErrorMessage);
                    }

                    invalidMinValue++;
                }
            }

            return ValidationResult.Success;
        }
    }
}

