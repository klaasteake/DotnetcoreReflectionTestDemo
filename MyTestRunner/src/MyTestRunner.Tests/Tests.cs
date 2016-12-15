using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;

namespace MyTestRunner.Tests
{
    public class Tests
    {
        [Fact]
        public void WatZijnAttributes()
        {
            var methods = this
                .GetType()
                .GetTypeInfo()
                .GetMethods()
                .Where(m => m.GetCustomAttributes<HalloAttribute>()
                    .Any());
            Assert.Equal(2, methods.Count());

            foreach (var method in methods)
            {
                method.Invoke(this, new object[] { });
            }
        }

        [Hallo]
        public void MethodeMetMijnEigenAttribute()
        {

        }


        [Fact]
        public void HoeLeesIkDeWaardesUitVanEenAttribute()
        {
            var method = this
                .GetType()
                .GetTypeInfo()
                .GetMethod(nameof(MethodeMetMijnEigenAttributeMetData));

            var attr = method.GetCustomAttribute<HalloAttribute>();
            Assert.Equal("goedemorgen", attr.Bericht);
        }

        [Hallo(Bericht = "goedemorgen")]
        public void MethodeMetMijnEigenAttributeMetData()
        {

        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        class HalloAttribute : Attribute
        {
            public string Bericht { get; set; }
        }

        [Fact]
        public void LaadAlleMethodesInMetHetFactsAttribute()
        {
            var runner = new MyTestRunner();
            var type = typeof(DummyTestClass);

            IEnumerable<MethodInfo> methods = runner.ResolveTestMethods(type);

            Assert.Equal(new[] { nameof(DummyTestClass.DoTest) },
                methods.Select(m => m.Name));
        }



        [Fact]
        public void NegeerPrivateMethodesOokAlHebbenZeHetFactAttribute()
        {
            var type = typeof(DummyTestClassMetPrivateMethode);
            var methods = new MyTestRunner().ResolveTestMethods(type);

            Assert.Empty(methods);
        }

        [Fact]
        public void BijEenExecuteWordtEenTestMethodeUitgevoerd()
        {
            // Arrange
            var runner = new MyTestRunner();

            var type = typeof(DummyTestClassVoorUitvoeren);
            var instance = Activator.CreateInstance(type);
            var methods = runner.ResolveTestMethods(type);

            // Act
            runner.ExecuteTestMethods(methods, instance);

            // Assert
            Assert.True(((DummyTestClassVoorUitvoeren)instance).TestIsExecuted);

        }



        [Fact]
        public void NegeerStaticMethodesOokAlHebbenZeHetFactAttribute()
        {
            var runner = new MyTestRunner();

            var type = typeof(DummyTestClassMetStaticMethode);
            var methods = runner.ResolveTestMethods(type);

            Assert.Empty(methods);
        }

        //

        [Fact]
        public void TestrunnerGeeftGeenExceptionWanneerEenMethodemetFactAttributeWelEenExceptionGooit()
        {
            // Arrange
            var runner = new MyTestRunner();

            var type = typeof(DummyTestClassMetMethodeDieExceptionGooit);
            var instance = Activator.CreateInstance(type);
            var methods = runner.ResolveTestMethods(type);

            // Act
            runner.ExecuteTestMethods(methods, instance);

            // Assert

        }


        [Fact]
        public void TestrunnerGeeftEenLijstVanNietGeslaagdeTesten()
        {
            // Arrange
            var runner = new MyTestRunner();

            var type = typeof(DummyTestClassMetMethodeDieExceptionGooit);
            var instance = Activator.CreateInstance(type);
            var methods = runner.ResolveTestMethods(type);

            // Act
            var testresults = runner.ExecuteTestMethods(methods, instance);

            // Assert
            Assert.Equal(1, testresults.Where(t => t.Result == false).Count());
        }
    }
}
