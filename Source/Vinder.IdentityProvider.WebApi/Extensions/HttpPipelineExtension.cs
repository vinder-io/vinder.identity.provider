namespace Vinder.IdentityProvider.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpPipelineExtension
{
    public static void UseHttpPipeline(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        app.UseTenantMiddleware();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}