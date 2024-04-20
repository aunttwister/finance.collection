using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T> ExecuteWithMetadata<T>(this Task<T> task, string context)
        {
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Wrap and rethrow with additional context information without handling it here
                throw new ApplicationException($"Exception in context '{context}': {ex.Message}", ex);
            }
        }
    }
}
