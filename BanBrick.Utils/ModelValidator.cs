using BanBrick.Utils.Internals;
using System;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BanBrick.Utils
{
    public static class ModelValidator
    {
        private static ConcurrentDictionary<Type, IEnumerable<PropertyValidation>> _typePropertyValidations = new ConcurrentDictionary<Type, IEnumerable<PropertyValidation>>();

        /// <summary>
        /// Validate model using DataAnnotations
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="ArgumentException" />
        /// <exception cref="MethodAccessException" />
        /// <exception cref="TargetException" />
        /// <exception cref="TargetParameterCountException" />
        /// <exception cref="TargetInvocationException" />
        /// <exception cref="NullReferenceException" />
        /// <exception cref="MethodAccessException" />
        /// <exception cref="ArgumentException" />
        public static IEnumerable<ValidationResult> Validate(object model)
        {
            if (model == null)
                throw new NullReferenceException("Validate Error: model cannot be null");

            var validationResults = new List<ValidationResult>();

            var propertyValidations = GetPropertyValidations(model);

            validationResults.AddRange(ValidateByPropertyValidators(model, propertyValidations));

            validationResults.AddRange(ValidateByValidatableObject(model));

            return validationResults;
        }

        // Get PropertyValidations
        private static IEnumerable<PropertyValidation> GetPropertyValidations(object model) {
            var type = model.GetType();
            
            if (!_typePropertyValidations.ContainsKey(type))
            {
                var propertyValidations = new List<PropertyValidation>();
                var properties = type.GetProperties();

                foreach (var property in properties)
                {
                    var validations = property.GetCustomAttributes(true).OfType<ValidationAttribute>();
                    if (validations == null || !validations.Any())
                        continue;

                    propertyValidations.Add(new PropertyValidation
                    {
                        Property = property,
                        Validations = validations
                    });
                }

                _typePropertyValidations[type] = propertyValidations;
            }

            return _typePropertyValidations[type];
        }

        // Validate model by property validators, without using reflection to get property and attributes
        private static IEnumerable<ValidationResult> ValidateByPropertyValidators(object model, IEnumerable<PropertyValidation> propertyValidators)
        {
            var validationResults = new List<ValidationResult>();

            foreach (var propertyValidator in propertyValidators)
            {
                var property = propertyValidator.Property;
                var value = propertyValidator.Property.GetValue(model, null);

                foreach (var validation in propertyValidator.Validations)
                {
                    try
                    {
                        validation.Validate(value, property.Name);
                    }
                    catch (ValidationException ex)
                    {
                        validationResults.Add(ex.ValidationResult);
                    }
                }
            }
            return validationResults;
        }

        // Validate model by IValidatableObject Interface
        private static IEnumerable<ValidationResult> ValidateByValidatableObject(object model) {
            if (model is IValidatableObject)
                return  ((IValidatableObject)model).Validate(new ValidationContext(model));
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
