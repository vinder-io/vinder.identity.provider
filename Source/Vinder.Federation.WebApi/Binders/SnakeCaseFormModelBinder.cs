namespace Vinder.Federation.WebApi.Binders;

public sealed class SnakeCaseFormModelBinder : IModelBinder
{
    #pragma warning disable S2325
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.HttpContext.Request.HasFormContentType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        var form = await bindingContext.HttpContext.Request.ReadFormAsync();
        var model = Activator.CreateInstance(bindingContext.ModelType)!;

        foreach (var property in bindingContext.ModelMetadata.Properties)
        {
            var snakeCaseName = ToSnakeCase(property.PropertyName!);

            if (form.TryGetValue(snakeCaseName, out var value))
            {
                var convertedValue = Convert.ChangeType(value.ToString(), property.ModelType);
                property.PropertySetter!(model, convertedValue);

                continue;
            }

            if (form.TryGetValue(property.PropertyName!, out var originalValue))
            {
                var convertedValue = Convert.ChangeType(originalValue.ToString(), property.ModelType);
                property.PropertySetter!(model, convertedValue);
            }
        }

        bindingContext.Result = ModelBindingResult.Success(model);
    }

    private static string ToSnakeCase(string input)
    {
        return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
    }
}