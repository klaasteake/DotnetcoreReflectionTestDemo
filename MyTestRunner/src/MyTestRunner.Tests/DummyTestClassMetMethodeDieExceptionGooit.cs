using System;
using Xunit;

namespace MyTestRunner.Tests
{
    internal class DummyTestClassMetMethodeDieExceptionGooit
    {

        [Fact]
        public void DoTest()
        {
            throw new Exception();
        }
    }
}