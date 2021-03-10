using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System;
namespace TaskScheduling
{
    internal class TaskScheduler
    {
        private Dictionary<Timer, ExecutionGroup> _taskSchedule;
        private List<Timer> _taskTimers;
        private List<Task> _tasks;

        public TaskScheduler()
        {
            _taskSchedule = new Dictionary<Timer, ExecutionGroup>();
            _taskTimers = new List<Timer>();
            _tasks = new List<Task>();
        }

        public void AddToSchedule(ExecutionGroup scheduled, double interval)
        {
            Timer timer = new Timer(interval);
            timer.AutoReset = scheduled._persistent;
            timer.Elapsed += Timer_Elapsed;

            _taskSchedule.Add(timer, scheduled);
            _taskTimers.Add(timer);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = sender as Timer;
            var exec = _taskSchedule[timer];

            if(!exec.Completed && exec._persistent)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"\nTimer Elapsed For: {exec.Name}: Completed Tasks: {exec.Status}, Persistent: {exec._persistent}, current execution cycle incomplete...\n");
                Console.ForegroundColor = ConsoleColor.Gray;

                return;
            }
            if(!exec._persistent)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                System.Console.WriteLine($"\nTimer Elapsed For: {exec.Name}: Completed Tasks: {exec.Status}, Persistent: {exec._persistent}, final execution cycle...\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                _taskSchedule.Remove(timer);
                return;
            }
            exec.Start();
        }

        public void Start()
        {
            _taskSchedule.Values.ForEach(eg => eg.Start());
            _taskSchedule.Keys.ForEach(t => t.Start());
        }
    }
}