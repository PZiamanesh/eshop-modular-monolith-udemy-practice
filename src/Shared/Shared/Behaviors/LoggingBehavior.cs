using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[START] Handling request={Request} - response={Response}",
            typeof(TRequest).Name, typeof(TResponse).Name
            );

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        if (timeTaken.Seconds > 5)
        {
            logger.LogWarning(
                "[PERFORMANCE] the request {Request} took {TimeTaken} seconds",
                typeof(TRequest).Name, timeTaken.Seconds
                );
        }

        logger.LogInformation(
            "[END] Handled request={Request} with response={Response}",
            typeof(TRequest).Name, typeof(TResponse).Name
            );

        return response;
    }
}
