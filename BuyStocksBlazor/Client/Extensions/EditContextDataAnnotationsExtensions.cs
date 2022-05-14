using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Extensions
{
    public static class EditContextDataAnnotationsExtensions
    {
        private static ConcurrentDictionary<(Type ModelType, string FieldName), PropertyInfo> _propertyInfoCache
        = new ConcurrentDictionary<(Type, string), PropertyInfo>();

        public static EditContext AddDataAnnotationsValidationWithDI(this EditContext editContext, IServiceProvider serviceProvider)
        {
            if (editContext == null)
            {
                throw new ArgumentNullException(nameof(editContext));
            }

            var messages = new ValidationMessageStore(editContext);

            // Perform object-level validation on request
            editContext.OnValidationRequested +=
                (sender, eventArgs) => ValidateModel((EditContext)sender!, serviceProvider, messages);

            // Perform per-field validation on each field edit
            editContext.OnFieldChanged +=
                (sender, eventArgs) => ValidateField(editContext, serviceProvider, messages, eventArgs.FieldIdentifier);

            return editContext;
        }
        private static void ValidateModel(EditContext editContext, IServiceProvider serviceProvider, ValidationMessageStore messages)
        {
            var validationContext = new ValidationContext(editContext.Model, serviceProvider, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(editContext.Model, validationContext, validationResults, true);

            // Transfer results to the ValidationMessageStore
            messages.Clear();
            foreach (var validationResult in validationResults)
            {
                if (validationResult == null)
                {
                    continue;
                }

                if (!validationResult.MemberNames.Any())
                {
                    messages.Add(new FieldIdentifier(editContext.Model, fieldName: string.Empty), validationResult.ErrorMessage!);
                    continue;
                }

                foreach (var memberName in validationResult.MemberNames)
                {
                    messages.Add(editContext.Field(memberName), validationResult.ErrorMessage!);
                }
            }

            editContext.NotifyValidationStateChanged();
        }

        private static void ValidateField(EditContext editContext, IServiceProvider serviceProvider, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier)
        {
            if (TryGetValidatableProperty(fieldIdentifier, out var propertyInfo))
            {
                var propertyValue = propertyInfo.GetValue(fieldIdentifier.Model);
                var validationContext = new ValidationContext(fieldIdentifier.Model, serviceProvider, null)
                {
                    MemberName = propertyInfo.Name
                };
                var results = new List<ValidationResult>();

                Validator.TryValidateProperty(propertyValue, validationContext, results);
                messages.Clear(fieldIdentifier);
                messages.Add(fieldIdentifier, results.Select(result => result.ErrorMessage!));

                // We have to notify even if there were no messages before and are still no messages now,
                // because the "state" that changed might be the completion of some async validation task
                editContext.NotifyValidationStateChanged();
            }
        }

        private static bool TryGetValidatableProperty(in FieldIdentifier fieldIdentifier, out PropertyInfo propertyInfo)
        {
            var cacheKey = (ModelType: fieldIdentifier.Model.GetType(), fieldIdentifier.FieldName);
            if (!_propertyInfoCache.TryGetValue(cacheKey, out propertyInfo))
            {
                // DataAnnotations only validates public properties, so that's all we'll look for
                // If we can't find it, cache 'null' so we don't have to try again next time
                propertyInfo = cacheKey.ModelType.GetProperty(cacheKey.FieldName);

                // No need to lock, because it doesn't matter if we write the same value twice
                _propertyInfoCache[cacheKey] = propertyInfo;
            }

            return propertyInfo != null;
        }

    }
}