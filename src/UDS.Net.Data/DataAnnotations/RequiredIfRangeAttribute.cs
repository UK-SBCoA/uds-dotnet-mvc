using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfRangeAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private int StartAssertionValue;
        private int EndAssertionValue;

        /// <summary>
        /// Property when condition asserts true.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="assertion"></param>
        public RequiredIfRangeAttribute(string propertyName, int startAssertionValue, int endAssertionValue, string errorMessage = "")
        {
            PropertyName = propertyName;
            StartAssertionValue = startAssertionValue;
            EndAssertionValue = endAssertionValue;
            ErrorMessage = errorMessage;
        }

        public RequiredIfRangeAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();

            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

            var formStatus = type.GetProperty("FormStatus");
            if (formStatus != null)
            {
                var formStatusValue = formStatus.GetValue(instance, null);
                if (formStatusValue.ToString() != "Complete")
                {
                    return ValidationResult.Success; // if the annotation is on a form and it is not being completed, don't run validation
                }
            }

            var startValue = StartAssertionValue;
            var endValue = EndAssertionValue;

            if (propertyValue != null) // we're allowing nulls in some cases, so the watched property won't always have a value
            {
                while (startValue <= endValue)
                {
                    if (propertyValue.ToString() == startValue.ToString() && value == null)
                    {
                        return new ValidationResult(ErrorMessage);
                    }

                    startValue++;
                }
            }

            return ValidationResult.Success;
        }

    }
}

