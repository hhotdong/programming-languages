using System;
using System.Threading.Tasks;

namespace AsyncTest
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            await TaskExample0();
            //await TaskExample1();
        }

        private static async Task TaskExample0()
        {
            var taskResult = await TaskAsyncExample.WaitAndReturnStringAsync();
            Console.WriteLine($"{taskResult}");
        }

        private static async Task TaskExample1()
        {
            await VoidAsyncExample.MultipleEventHandlersAsync();
        }
    }
}
