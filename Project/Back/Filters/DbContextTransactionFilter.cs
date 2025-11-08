using ILogger = Serilog.ILogger;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exato.Back.Filters;

public class DbContextTransactionFilter : TypeFilterAttribute
{
    public DbContextTransactionFilter() : base(typeof(DbContextTransactionFilterImpl)) { }

    private class DbContextTransactionFilterImpl(BackDbContext BackDbContext, ILogger logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            BackDbContext.Database.AutoSavepointsEnabled = false;
            await using var transaction = await BackDbContext.Database.BeginTransactionAsync();

            try
            {
                var actionExecuted = await next();
                if (actionExecuted.Result is BadRequestObjectResult || (actionExecuted.Exception != null && !actionExecuted.ExceptionHandled))
                {
                    await transaction.RollbackAsync();
                }
                else
                {
                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    logger.Error("Rollback Error -> {Message}", ex.Message);
                }

                throw;
            }
        }
    }
}
