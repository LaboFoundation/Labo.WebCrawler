namespace Labo.WebCrawler.Core.Tests
{
    using System;
    using System.Diagnostics;

    using Labo.WebCrawler.Core.Task;

    using NSubstitute;
    using NSubstitute.Core;

    using NUnit.Framework;

    [TestFixture]
    public class UriProcessorTaskManagerFixture
    {
        [Test]
        public void IsWorking_MustBeFalseWhenUriProcessTaskFinishesWorking()
        {
            MultiThreadedUriProcessorTaskManager uriProcessorTaskManager = new MultiThreadedUriProcessorTaskManager(4);
            uriProcessorTaskManager.Start();

            bool uriTaskProcessorIsWorking = true;
            IUriProcessorTask uriProcessorTask = Substitute.For<IUriProcessorTask>();
            uriProcessorTask.When(x => x.Process()).Do(
                x =>
                    {
                        while (uriTaskProcessorIsWorking)
                        {  
                        }
                    });

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());

            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask);

            While(() => uriProcessorTaskManager.GetQueueLength() > 0);

            Assert.IsTrue(uriProcessorTaskManager.IsWorking());

            uriTaskProcessorIsWorking = false;

            WaitTaskManagerToFinishWorking(uriProcessorTaskManager, Assert.Fail);

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());
        }

        [Test]
        public void IsWorking_MustBeFalseWhenAllUriProcessTasksFinishWorking()
        {
            MultiThreadedUriProcessorTaskManager uriProcessorTaskManager = new MultiThreadedUriProcessorTaskManager(4);
            uriProcessorTaskManager.Start();

            bool uriTaskProcessorIsWorking = true;
            Action<CallInfo> uriProcessorTaskAction = x =>
                {
                    while (uriTaskProcessorIsWorking)
                    {
                    }
                };

            IUriProcessorTask uriProcessorTask1 = Substitute.For<IUriProcessorTask>();
            uriProcessorTask1.When(x => x.Process()).Do(uriProcessorTaskAction);

            IUriProcessorTask uriProcessorTask2 = Substitute.For<IUriProcessorTask>();
            uriProcessorTask2.When(x => x.Process()).Do(uriProcessorTaskAction);

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());

            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask1);
            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask2);

            While(() => uriProcessorTaskManager.GetQueueLength() > 0);

            Assert.IsTrue(uriProcessorTaskManager.IsWorking());

            uriTaskProcessorIsWorking = false;

            WaitTaskManagerToFinishWorking(uriProcessorTaskManager, Assert.Fail);

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());
        }

        [Test]
        public void IsWorking_MustBeTrueWhenOnlyOneOfTwoUriProcessTasksFinishWorking()
        {
            MultiThreadedUriProcessorTaskManager uriProcessorTaskManager = new MultiThreadedUriProcessorTaskManager(2);
            uriProcessorTaskManager.Start();

            bool uriTaskProcessor1IsWorking = true;
            bool uriTaskProcessor2IsWorking = true;

            IUriProcessorTask uriProcessorTask1 = Substitute.For<IUriProcessorTask>();
            uriProcessorTask1.When(x => x.Process()).Do(x =>
            {
                while (uriTaskProcessor1IsWorking)
                {
                }
            });

            IUriProcessorTask uriProcessorTask2 = Substitute.For<IUriProcessorTask>();
            uriProcessorTask2.When(x => x.Process()).Do(x =>
            {
                while (uriTaskProcessor2IsWorking)
                {
                }
            });

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());

            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask1);
            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask2);

            While(() => uriProcessorTaskManager.GetQueueLength() > 0);

            Assert.IsTrue(uriProcessorTaskManager.IsWorking());

            uriTaskProcessor1IsWorking = false;

            Assert.IsTrue(uriProcessorTaskManager.IsWorking());

            uriTaskProcessor2IsWorking = false;

            WaitTaskManagerToFinishWorking(uriProcessorTaskManager, Assert.Fail);

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());
        }

        [Test]
        public void MustProcessTasksOneByOneWhenThreadWorkerCountIsOne()
        {
            MultiThreadedUriProcessorTaskManager uriProcessorTaskManager = new MultiThreadedUriProcessorTaskManager(1);
            uriProcessorTaskManager.Start();

            bool uriTaskProcessor1IsWorking = true;
            bool uriTaskProcessor2IsWorking = true;

            bool uriTaskProcessor1IsStarted = false;
            bool uriTaskProcessor2IsStarted = false;

            IUriProcessorTask uriProcessorTask1 = Substitute.For<IUriProcessorTask>();
            uriProcessorTask1.When(x => x.Process()).Do(
                x =>
                    {
                        uriTaskProcessor1IsStarted = true;

                        while (uriTaskProcessor1IsWorking)
                        {
                        }
                    });

            IUriProcessorTask uriProcessorTask2 = Substitute.For<IUriProcessorTask>();
            uriProcessorTask2.When(x => x.Process()).Do(x =>
            {
                uriTaskProcessor2IsStarted = true;

                while (uriTaskProcessor2IsWorking)
                {
                }
            });

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());

            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask1);
            uriProcessorTaskManager.EnqueueDownloader(uriProcessorTask2);

            While(() => uriProcessorTaskManager.GetQueueLength() > 1, Assert.Fail);

            Assert.IsTrue(uriProcessorTaskManager.IsWorking());

            uriTaskProcessor1IsWorking = false;

            Assert.IsTrue(uriProcessorTaskManager.IsWorking());

            While(() => uriProcessorTaskManager.GetQueueLength() > 0, Assert.Fail);

            While(() => uriTaskProcessor2IsStarted, Assert.Fail);

            uriTaskProcessor2IsWorking = false;

            WaitTaskManagerToFinishWorking(uriProcessorTaskManager, Assert.Fail);

            Assert.IsFalse(uriProcessorTaskManager.IsWorking());
        }

        private static void WaitTaskManagerToFinishWorking(IUriProcessorTaskManager uriProcessorTaskManager, Action onTimeout = null, int timeout = 500)
        {
            While(uriProcessorTaskManager.IsWorking, onTimeout, timeout);
        }

        private static void While(Func<bool> func, Action onTimeout = null, int timeout = 1000)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (func())
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    if (onTimeout != null)
                    {
                        onTimeout();
                    }

                    break;
                }
            }

            sw.Stop();
        }
    }
}
