using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduling
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Program p = new Program();
            ExecutionGroupManager ts = new ExecutionGroupManager();
         
            var executionGroup1 = new ExecutionGroup(persistentQueue:true,longRunning:false);
            executionGroup1.Name = "Group 1";

            var executionGroup2 = new ExecutionGroup(persistentQueue:true,longRunning:false);
            executionGroup2.Name = "Group 2";

            executionGroup1.Add(async () =>
            {
                p.PrintMe("Group 1: Task 1: working for 20 second");
                await Task.Delay(20000);
                Console.WriteLine("Group 1: Task 1: Done Executing!");


            });
            executionGroup1.Add(async () =>
            {
                p.PrintMe("Group 1: Task 2: working for 1 second");
                await Task.Delay(1000);
                Console.WriteLine("Group 1: Task 2: Done Executing!");

            });

            executionGroup2.Add(async () =>
            {
                p.PrintMe("Group 2: Task 1: working for 5 seconds");
                await Task.Delay(5000);
                Console.WriteLine("Group 2: Task 1: Done Executing!");


            });
            executionGroup2.Add(async () =>
            {
                p.PrintMe("Group 2: Task 2: working for 10 second");
                await Task.Delay(10000);
                Console.WriteLine("Group 2: Task 2: Done Executing!");


            });


            //Add to the schedule with the preferred execution interval (not guaranteed to execute when timer elapses) 
            ts.AddToSchedule(executionGroup1, 5000);
           ts.AddToSchedule(executionGroup2, 3000);

           ts.Start();


            
            Console.ReadLine();
        }

        private void PrintMe(string text)
        {
            Console.WriteLine(text);
        }

     
    }
}