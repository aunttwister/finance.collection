using AspectInjector.Broker;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

[Aspect(Scope.Global)]
[Injection(typeof(HandleMethodExecutionAspect))]
public class HandleMethodExecutionAspect : Attribute
{
    private static ILogger _logger;

    public static void SetLogger(ILogger logger)
    {
        _logger = logger;
    }
    [Advice(Kind.Around, Targets = Target.Method)]
    public object HandleMethodExecution([Argument(Source.Arguments)] object[] args,
                                       [Argument(Source.Target)] Func<object[], object> target,
                                       [Argument(Source.Metadata)] MethodBase method,
                                       [Argument(Source.Name)] string methodName)
    {
        // Log before execution
        _logger?.LogInformation($"Entering method {method.DeclaringType.Name}.{methodName}.");
        try
        {
            var result = target(args);

            // Log after successful execution
            _logger?.LogInformation($"Successfully executed method {method.DeclaringType.Name}.{methodName}.");

            return result;
        }
        catch (Exception ex)
        {
            var message = $"Error in method {method.DeclaringType.Name}.{methodName}: {ex.Message}";

            // Determine the severity of the log based on exception type or other criteria
            _logger?.LogError(ex, message);

            throw new Exception(message, ex);
        }
        finally
        {
            // Optionally log on method exit if needed
            _logger?.LogInformation($"Exiting method {method.DeclaringType.Name}.{methodName}.");
        }
    }
}
