using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class RetryHelper
    {
        private static readonly TimeSpan[] RetryDelays = 
        {
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2)
        };

        public static async Task<T> RetryAsync<T>(
            Func<Task<T>> operation,
            CancellationToken cancellationToken,
            int maxRetries = 3,
            string operationName = "Operation")
        {
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (ShouldRetry(ex, attempt, maxRetries))
                {
                    var delay = attempt < RetryDelays.Length ? RetryDelays[attempt] : TimeSpan.FromSeconds(5);
                    
                    LogHelper.WriteLog($"{operationName} failed (attempt {attempt + 1}/{maxRetries}): {ex.Message}. Retrying in {delay.TotalSeconds}s...");

                    await Task.Delay(delay, cancellationToken);
                }
            }

            // Final attempt without retry
            return await operation();
        }

        private static bool ShouldRetry(Exception ex, int attempt, int maxRetries)
        {
            if (attempt >= maxRetries - 1)
                return false;

            switch (ex)
            {
                case HttpRequestException httpEx:
                    return IsRetryableHttpError(httpEx);
                case TaskCanceledException:
                    return false; // Don't retry if cancelled
                case OperationCanceledException:
                    return false; // Don't retry if cancelled
                default:
                    return IsTransientError(ex);
            }
        }

        private static bool IsRetryableHttpError(HttpRequestException ex)
        {
            // Check if it's a server error (5xx) or specific client errors that might be temporary
            if (ex.Data.Contains("StatusCode"))
            {
                var statusCode = (HttpStatusCode)ex.Data["StatusCode"];
                return statusCode >= HttpStatusCode.InternalServerError || 
                       statusCode == HttpStatusCode.RequestTimeout ||
                       statusCode == HttpStatusCode.TooManyRequests;
            }

            // Network-related errors that might be temporary
            var message = ex.Message.ToLowerInvariant();
            return message.Contains("timeout") ||
                   message.Contains("connection") ||
                   message.Contains("network") ||
                   message.Contains("dns") ||
                   message.Contains("socket");
        }

        private static bool IsTransientError(Exception ex)
        {
            // Add more transient error conditions as needed
            var message = ex.Message.ToLowerInvariant();
            return message.Contains("timeout") ||
                   message.Contains("connection") ||
                   message.Contains("network") ||
                   message.Contains("temporary");
        }
    }
}