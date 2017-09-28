using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UtilityComponent.Entities
{
    [Serializable]
    public abstract class ValidatableObject : BaseObject
    {
        [NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        protected ValidationResults validRes;

        /// <summary>
        /// Returns the ValidationResults
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public virtual ValidationResults ValidationResults
        {
            get { return validRes ?? new ValidationResults(); }
        }

        /// <summary>
        /// Validates the entire entity that derives from this class and returns true/false
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public virtual bool IsValid
        {
            get
            {
                validRes = ValidateSelfAndProperties();
                return validRes.IsValid;
            }
        }

        /// <summary>
        /// Validates the entire entity that derives from this class and the ValidationResults
        /// </summary>
        public virtual ValidationResults Validate()
        {
            return ValidateSelfAndProperties();
        }

        public static void ValidateChild(string parentName, object child, ValidationResults parentValidationResults)
        {
            ValidationResults childValidationResults = ValidateObject(child);
            foreach (var childValidationResult in childValidationResults)
            {
                // we "re-pack" property validation result in order to have better explanation of the problem
                parentValidationResults.AddResult(new ValidationResult(childValidationResult.Message, childValidationResult.Target,
                                            parentName + ":" + childValidationResult.Key, childValidationResult.Tag, childValidationResult.Validator));
            }
        }

        #region private methods
        private ValidationResults ValidateSelfAndProperties()
        {
            ValidationResults validationResults;
            IEnumerable<PropertyInfo> validatableProperties;

            // 1. validate itself
            validationResults = ValidateObject(this);
            // 2. find and validate all properties which can be validated
            validatableProperties = GetValidatableProperties(this);
            foreach (var property in validatableProperties)
            {
                object propertyValue = property.GetValue(this, null);
                if (propertyValue != null)
                    ValidateChild(property.Name, propertyValue, validationResults);
            }

            // return all results
            return validationResults;
        }

        private static void AddChildValidationResult(ValidationResults validationResults, PropertyInfo property, ValidationResult childValidationResult)
        {
            validationResults.AddResult(new ValidationResult(childValidationResult.Message, childValidationResult.Target,
                property.Name + ":" + childValidationResult.Key, childValidationResult.Tag, childValidationResult.Validator));
        }

        private IEnumerable<PropertyInfo> GetValidatableProperties(object o)
        {
            return o.GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(ValidatableObject)));

        }

        private static ValidationResults ValidateObject(object o)
        {
            return ValidationFactory.CreateValidator(o.GetType()).Validate(o);
        }
        #endregion
    }
}
