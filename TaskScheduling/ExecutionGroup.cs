using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduling
{
    internal class ExecutionGroup
    {
        private List<Func<Task>> _functions { get; set; }
        public List<Func<Task>> _persistentAction;
        public string Name { get; set; }
        public bool Completed { get; set; }

        public bool _persistent;

        private int _tasksCompleted;

        public bool LongRunning { get; set; }

        public string Status
        {
            get
            {
                return $"{_tasksCompleted}\\{_persistentAction.Count}";
            }
        }


        public ExecutionGroup(bool persistentQueue, bool longRunning)
        {
            LongRunning = longRunning;
            _persistent = persistentQueue;
            _functions = new List<Func<Task>>();
            _persistentAction = new List<Func<Task>>();
         
        }

        public Task Start()
        {
            Completed = false;
            _tasksCompleted = 0;
            LoadActions();
            var task = Task.Factory.StartNew(async () =>
            {
                Console.WriteLine($"{Name} is running on {Thread.CurrentThread.ManagedThreadId}, ThreadPool Thread: {Thread.CurrentThread.IsThreadPoolThread}");

                foreach (var func in _persistentAction)
                {
                    await func();
                    _tasksCompleted++;
                }
                Completed = true;
                Console.ForegroundColor = ConsoleColor.Green;

                System.Console.WriteLine($"\n\n{Name} Completed Tasks: {Status}, execution cycle complete...\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;


            }, LongRunning ? TaskCreationOptions.LongRunning :TaskCreationOptions.PreferFairness);
            return task;
        }

        private void LoadActions()
        {
            foreach (var action in _persistentAction)
            {
                _functions.Add(action);
            }
        }

        public void Add(Func<Task> action)
        {
            _persistentAction.Add(action);
        }
    }
}