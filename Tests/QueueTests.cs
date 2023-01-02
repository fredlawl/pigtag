using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void QueueDoesNotDequeueInLoop()
        {
            Queue<int> queue = new Queue<int>(new List<int>() { 1, 2, 3, 4 });
            foreach (int i in queue)
            {
                Console.WriteLine(i);
            }

            Assert.IsTrue(queue.Count > 0);
        }
    }
}
