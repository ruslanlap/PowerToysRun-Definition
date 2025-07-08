using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class ClipboardHelper
    {
        private static readonly object _lock = new object();
        private static Thread _staThread;
        private static bool _isInitialized = false;
        private static readonly TaskScheduler _staTaskScheduler;

        static ClipboardHelper()
        {
            _staTaskScheduler = new STATaskScheduler();
        }

        public static bool CopyToClipboard(string text)
        {
            if (!ConfigurationManager.Configuration.EnableClipboardOperations || string.IsNullOrEmpty(text)) return false;

            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Clipboard.SetDataObject(text, true);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError($"Clipboard Error: {ex}");
                        return false;
                    }
                }, CancellationToken.None, TaskCreationOptions.None, _staTaskScheduler);

                return task.Wait(TimeSpan.FromSeconds(5)) && task.Result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"Clipboard Task Error: {ex}");
                return false;
            }
        }

        private class STATaskScheduler : TaskScheduler
        {
            private readonly BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
            private readonly Thread _thread;

            public STATaskScheduler()
            {
                _thread = new Thread(RunTasks) { IsBackground = true };
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();
            }

            protected override void QueueTask(Task task)
            {
                _tasks.Add(task);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return Thread.CurrentThread == _thread && TryExecuteTask(task);
            }

            protected override IEnumerable<Task> GetScheduledTasks()
            {
                return _tasks.ToArray();
            }

            public override int MaximumConcurrencyLevel => 1;

            private void RunTasks()
            {
                foreach (var task in _tasks.GetConsumingEnumerable())
                {
                    TryExecuteTask(task);
                }
            }
        }
    }
} 