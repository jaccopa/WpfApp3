using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(()=> { ExitTask(); });

            Console.ReadLine();
        }

        static void ExitTask()
        {
            var source = new CancellationTokenSource();

            var token = source.Token;

            var task = Task.Run(() => UpdateUI(token), token);

            Thread.Sleep(500);

            Console.WriteLine("Main::Cancel");
            source.Cancel();
        }

        static void UpdateUI(CancellationToken token)
        {
            for (int i = 0; i < 10 * 60; i++)
            {
                Thread.Sleep(1000);

                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Method1 canceled");
                    return;
                }

                Console.WriteLine($"Method1 running... {i}");
                //update ui seconds
            }

            //default shutdown flow
        }

        //static void Main(string[] args)
        //{
        //    var waterOutReady = new CancellationTokenSource();
        //    var token = waterOutReady.Token;

        //    var task = Task.Run(() => UpdateUI(token), token);

        //    Thread.Sleep(500);

        //    Console.WriteLine("Main::Cancel");
        //    waterOutReady.Cancel();

        //    Console.ReadLine();
        //}

        //static void UpdateUI(CancellationToken token)
        //{
        //    for (int i = 0; i < 10 * 60; i++)
        //    {
        //        Thread.Sleep(1000);

        //        if (token.IsCancellationRequested)
        //        {
        //            Console.WriteLine("Method1 canceled");
        //            break;
        //        }

        //        Console.WriteLine($"Method1 running... {i}");
        //    }

        //    //default shutdown flow
        //}
    }
}
