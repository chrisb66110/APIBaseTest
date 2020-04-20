using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace APIBaseTest
{
    public abstract class BaseTest<TSystem> where TSystem : class
    {
        /*
         * Create a object to test
         */
        protected TSystem GivenTheSystemUnderTest(AutoMock mock)
        {
            var sut = mock.Create<TSystem>();

            return sut;
        }

        /*
         * Mock async dependency when result is a class
         */
        protected Mock<T> AndIMockDependencyMethod<T, TResult>(
            AutoMock mock,
            Expression<Func<T, Task<TResult>>> method,
            TResult response)
            where T : class
            where TResult : new()
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .ReturnsAsync(CloneObject(response));

            return mockDependency;
        }

        /*
         * Mock async dependency when result is a class and with callback
         */
        protected Mock<T> AndIMockDependencyMethod<T, TParam, TResult>(
            AutoMock mock,
            Expression<Func<T, Task<TResult>>> method,
            TResult response,
            Action<TParam> callback)
            where T : class
            where TResult : new()
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .ReturnsAsync(CloneObject(response))
                .Callback(callback);

            return mockDependency;
        }

        /*
         * Mock dependency when result is a class
         */
        protected Mock<T> AndIMockDependencyMethod<T, TResult>(
            AutoMock mock,
            Expression<Func<T, TResult>> method,
            TResult response)
            where T : class
            where TResult : new()
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .Returns(CloneObject(response));

            return mockDependency;
        }

        /*
         * Mock dependency when result is a class and with callback
         */
        protected Mock<T> AndIMockDependencyMethod<T, TParam, TResult>(
            AutoMock mock,
            Expression<Func<T, TResult>> method,
            TResult response,
            Action<TParam> callback)
            where T : class
            where TResult : new()
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .Returns(CloneObject(response))
                .Callback(callback);

            return mockDependency;
        }

        /*
         * Mock dependency when result is a exception
         */
        protected Mock<T> AndIMockDependencyMethod<T, TResult>(
            AutoMock mock,
            Expression<Func<T, TResult>> method,
            Exception exception)
            where T : class
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .Throws(CloneObject(exception));

            return mockDependency;
        }

        /*
         * Mock async dependency when result is a exception
         */
        protected Mock<T> AndIMockDependencyMethod<T, TResult>(
            AutoMock mock,
            Expression<Func<T, Task<TResult>>> method,
            Exception exception)
            where T : class
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .Throws(CloneObject(exception));

            return mockDependency;
        }

        /*
         * Mock logger
         */
        protected Mock<ILogger<TSystem>> AndIMockILogger(
            AutoMock mock)
        {
            var mockLogger = mock.Mock<ILogger<TSystem>>();

            mockLogger.Setup(x =>
                x.Log(It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    (Func<object, Exception, string>)It.IsAny<object>()));

            return mockLogger;
        }

        /*
         * Mock Mapper
         */
        protected Mock<IMapper> AndIMockIMapperMap<TSource, TDestination>(
            AutoMock mock,
            TDestination response)
            where TDestination : new()
        {
            var mockMapper = mock.Mock<IMapper>();

            mockMapper.Setup(m => m.Map<TSource, TDestination>(It.IsAny<TSource>()))
                .Returns(CloneObject(response));

            return mockMapper;
        }

        /*
         * Mock Mapper with callback
         */
        protected Mock<IMapper> AndIMockIMapperMap<TSource, TDestination>(
            AutoMock mock,
            TDestination response,
            Action<TSource> callback)
            where TDestination : new()
        {
            var mockMapper = mock.Mock<IMapper>();

            mockMapper.Setup(m => m.Map<TSource, TDestination>(It.IsAny<TSource>()))
                .Returns(CloneObject(response))
                .Callback(callback);

            return mockMapper;
        }

        /*
         * Clone
         */
        public T CloneObject<T>(
            T originObject)
            where T : new()
        {
            var json = JsonConvert.SerializeObject(originObject);

            var newObject = JsonConvert.DeserializeObject<T>(json);

            return newObject;
        }

        /*
         * Check properties values
         */
        public void CheckAllProperties<TExpected, TActual>(
            TExpected expected,
            TActual actual,
            int? index = null)
        {
            foreach (var prop in expected.GetType().GetProperties())
            {
                var name = prop.Name;
                var valueExpected = prop.GetValue(expected, null);
                var valueActual = prop.GetValue(actual, null);

                Assert.AreEqual(valueExpected, valueActual, (index == null ? "" : $"[{index}].") + $"{name} is not correct");
            }
        }

        /*
         * Check properties values in every object in list
         */
        public void CheckAllProperties<TExpected, TActual>(
            List<TExpected> expected,
            List<TActual> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Counts are different");

            for (int index = 0; index < expected.Count; index++)
            {
                CheckAllProperties(expected[index], actual[index], index);
            }
        }
    }
}
