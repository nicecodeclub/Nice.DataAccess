using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.Net.Common.Task
{
    public class TaskManager
    {
        private Queue<Action> logs = null;
        private static object locker = new object();
        private ManualResetEventSlim mre = null;
        public TaskManager()
        {
            logs = new Queue<Action>();
            mre = new ManualResetEventSlim(false);
            Thread thread = new Thread(Work);
            thread.Start();
        }
        //循环等待
        private void Work()
        {
            while (true)
            {
                if (logs.Count > 0)
                    OnWork();
                else
                {
                    //等待写入信号
                    mre.Reset();
                    mre.Wait();
                }
            }
        }

        public void Add(Action action)
        {
            lock (locker)
            {
                logs.Enqueue(action);
                mre.Set();
            }
        }

        private void OnWork()
        {
            Action action = null;
            lock (locker)
            {
                if (logs.Count > 0)
                    action = logs.Dequeue();
            }
            if (action != null)
            {
                action.Invoke();
            }
        }
    }
}
