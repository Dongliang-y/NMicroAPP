using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main：线程Id:【{0}】，是否后台线程:【{1}】，是否使用线程池:【{2}】，当前时间:【{3}】",
    Environment.CurrentManagedThreadId.ToString(),
    Thread.CurrentThread.IsBackground,
    Thread.CurrentThread.IsThreadPoolThread,
    DateTime.Now.ToString());

            Console.WriteLine("Hello World!");
            Test();

            Console.WriteLine("MainEnd：线程Id:【{0}】，是否后台线程:【{1}】，是否使用线程池:【{2}】，当前时间:【{3}】",
    Environment.CurrentManagedThreadId.ToString(),
    Thread.CurrentThread.IsBackground,
    Thread.CurrentThread.IsThreadPoolThread,
    DateTime.Now.ToString());

            Console.ReadLine();
        }

        public static async Task Test()
        {
            await TestAsync();
        }

        public static async Task TestAsync()
        { 
            Console.WriteLine("TestAsync：线程Id:【{0}】，是否后台线程:【{1}】，是否使用线程池:【{2}】，当前时间:【{3}】",
                Environment.CurrentManagedThreadId.ToString(),
                Thread.CurrentThread.IsBackground,
                Thread.CurrentThread.IsThreadPoolThread,
                DateTime.Now.ToString());
            for (var i = 0; i < 1000; i++)
            {
                await System.IO.File.AppendAllLinesAsync($"D:\\testx.txt", new List<string>() { $"士大夫首发式发生的{DateTime.Now.ToString()}" });
            }
            Console.WriteLine("TestAsync：线程Id:【{0}】，是否后台线程:【{1}】，是否使用线程池:【{2}】，当前时间:【{3}】",
    Environment.CurrentManagedThreadId.ToString(),
    Thread.CurrentThread.IsBackground,
    Thread.CurrentThread.IsThreadPoolThread,
    DateTime.Now.ToString());
            System.Threading.Thread.Sleep(5000);
        }
    }
}
