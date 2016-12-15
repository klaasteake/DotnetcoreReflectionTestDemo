using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyTestRunner.Tests
{
    public class DummyTestClassVoorUitvoeren
    {
        public bool TestIsExecuted { get; internal set; }

        [Fact]
        public void DoTest()
        {
            TestIsExecuted = true;
        }
    }
}
