using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.DataAnnotations
{
    /// <summary>
    /// This annotation was made specifically for handling special cases in
    /// ClinicianDiagnosis.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfImpairedAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object AssertionValue { get; set; }

        /// <summary>
        /// Property when condition asserts true.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="assertion"></param>
        public RequiredIfImpairedAttribute(string propertyName, object assertion, string errorMessage = "")
        {
            PropertyName = propertyName;
            AssertionValue = assertion;
            ErrorMessage = errorMessage;
        }

        public RequiredIfImpairedAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();

            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

            var formStatus = type.GetProperty("FormStatus");
            var hasNormalCognition = type.GetProperty("HasNormalCognition");
            if (formStatus != null && hasNormalCognition != null)
            {
                var formStatusValue = formStatus.GetValue(instance, null);
                var hasNormalCognitionValue = hasNormalCognition.GetValue(instance, null);
                if (formStatusValue.ToString() != "Complete")
                {
                    return ValidationResult.Success; // if the annotation is on a form and it is not being completed, don't run validation
                }
                else
                {
                    // form status is complete
                    string test = hasNormalCognitionValue.ToString();
                    if (hasNormalCognitionValue.ToString() == "True")
                    {
                        // cognition is normal (not impaired), so don't require
                        return ValidationResult.Success;
                    }
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


