using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object AssertionValue { get; set; }

        /// <summary>
        /// Property when condition asserts true.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="assertion"></param>
        public RequiredIfAttribute(string propertyName, object assertion, string errorMessage = "")
        {
            PropertyName = propertyName;
            AssertionValue = assertion;
            ErrorMessage = errorMessage;
        }

        public RequiredIfAttribute(string ErrorMessage)
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

            if (propertyValue != null) // we're allowing nulls in some cases, so the watched property won't always have a value
            {
                if (propertyValue.ToString() == AssertionValue.ToString() && value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }

    }
}

