namespace Vinder.IdentityProvider.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class FromSnakeCaseFormAttribute : ModelBinderAttribute
{
    public FromSnakeCaseFormAttribute() : base(typeof(SnakeCaseFormModelBinder)) {  }
}