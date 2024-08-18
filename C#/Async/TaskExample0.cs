using System;
using System.Threading.Tasks;

namespace AsyncTest
{
    public class TaskAsyncExample
    {
        public static async Task<string> WaitAndReturnStringAsync()
        {
            Console.WriteLine("Test0 starts.");

            await Task.Delay(1000);

            Console.WriteLine("Test0 ended.");

            return "Test0 completes.";
        }
    }
    /// Output:
    // Test0 starts.
    // Test0 ended.
    // Test0 completes.
}