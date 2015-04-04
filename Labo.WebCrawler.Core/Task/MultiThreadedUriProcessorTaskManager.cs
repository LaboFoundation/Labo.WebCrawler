namespace Labo.WebCrawler.Core.Task
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal sealed class MultiThreadedUriProcessorTaskManager : IUriProcessorTaskManager
    {
        private readonly Queue<IUriProcessorTask> m_TaskQueue;
        private readonly object m_Locker = new object();
        private readonly Thread[] m_Workers;

        private bool m_Running;

        public MultiThreadedUriProcessorTaskManager(int threadWorkerCount)
        {
            if (threadWorkerCount < 1)
            {
                throw new ArgumentOutOfRangeException("threadWorkerCount", "workercount cannot be less than 1");
            }

            m_TaskQueue = new Queue<IUriProcessorTask>();
            m_Workers = new Thread[threadWorkerCount];

            for (int i = 0; i < threadWorkerCount; i++)
            {
                m_Workers[i] = new Thread(DoWork);
            }
        }

        public bool IsWorking()
        {
            lock (m_Locker)
            {
                return m_Running;
            }
        }

        public int GetQueueLength()
        {
            lock (m_Locker)
            {
                return m_TaskQueue.Count;
            }
        }

        public void EnqueueDownloader(IUriProcessorTask task)
        {
            lock (m_Locker)
            {
                m_TaskQueue.Enqueue(task);
                Monitor.Pulse(m_Locker);
            }
        }

        public void Start()
        {
            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                worker.Start();
            }
        }

        public void Finish(bool waitForAllWorkers)
        {
            for (int i = 0; i < m_Workers.Length; i++)
            {
                EnqueueDownloader(null);
            }

            if (waitForAllWorkers)
            {
                for (int i = 0; i < m_Workers.Length; i++)
                {
                    Thread worker = m_Workers[i];
                    worker.Join();
                }
            }

            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                worker.Abort();
            }
        }

        private void DoWork()
        {
            while (true)
            {
                IUriProcessorTask task;
                lock (m_Locker)
                {
                    while (m_TaskQueue.Count == 0)
                    {
                        m_Running = false;
                        Monitor.Wait(m_Locker);
                    }

                    task = m_TaskQueue.Dequeue();
                }

                if (task == null)
                {
                    return;
                }

                m_Running = true;

                task.Process();
            }
        }
    }
}