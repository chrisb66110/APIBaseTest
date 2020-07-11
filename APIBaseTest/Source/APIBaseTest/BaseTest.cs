using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace APIBaseTest
{
    public abstract class BaseTest<TSystem>
        where TSystem : class
    {
        /*
         * Create a object to test
         */
        protected virtual TSystem GivenTheSystemUnderTest(AutoMock mock)
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
        protected Mock<T> AndIMockDependencyMethod<T, TResult, TException>(
            AutoMock mock,
            Expression<Func<T, TResult>> method,
            TException exception)
            where T : class
            where TException : Exception
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .Throws(exception);

            return mockDependency;
        }

        /*
         * Mock async dependency when result is a exception
         */
        protected Mock<T> AndIMockDependencyMethod<T, TResult, TException>(
            AutoMock mock,
            Expression<Func<T, Task<TResult>>> method,
            TException exception)
            where T : class
            where TException : Exception
        {
            var mockDependency = mock.Mock<T>();

            mockDependency.Setup(method)
                .Throws(exception);

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
        protected T CloneObject<T>(
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
        protected void CheckAllProperties<TExpected, TActual>(
            TExpected expected,
            TActual actual,
            int? index = null)
        {
            foreach (var prop in expected.GetType().GetProperties())
            {
                var name = prop.Name;
                var valueExpected = prop.GetValue(expected, null);

                var valueActual = actual.GetType().GetProperty(name)?.GetValue(actual, null);

                Assert.AreEqual(valueExpected, valueActual, (index == null ? "" : $"[{index}].") + $"{name} is not correct");
            }
        }

        /*
         * Check properties values in every object in list
         */
        protected void CheckAllProperties<TExpected, TActual>(
            List<TExpected> expected,
            List<TActual> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Counts are different");

            for (int index = 0; index < expected.Count; index++)
            {
                CheckAllProperties(expected[index], actual[index], index);
            }
        }

        ///*
        // * Check properties values
        // */
        //protected void CheckAllProperties<TType>(
        //    TType expected,
        //    TType actual,
        //    int? index = null)
        //{
        //    foreach (var prop in expected.GetType().GetProperties())
        //    {
        //        var name = prop.Name;
        //        var valueExpected = prop.GetValue(expected, null);
        //        var valueActual = prop.GetValue(actual, null);

        //        Assert.AreEqual(valueExpected, valueActual, (index == null ? "" : $"[{index}].") + $"{name} is not correct");
        //    }
        //}

        ///*
        // * Check properties values in every object in list
        // */
        //protected void CheckAllProperties<TType>(
        //    List<TType> expected,
        //    List<TType> actual)
        //{
        //    Assert.AreEqual(expected.Count, actual.Count, "Counts are different");

        //    for (int index = 0; index < expected.Count; index++)
        //    {
        //        CheckAllProperties<TType>(expected[index], actual[index], index);
        //    }
        //}

        /*
         * Create instance IMapper
         */
        protected IMapper GivenTheAllRealMapper()
        {
            var assemblies = new List<Assembly>
            {
                Assembly.GetAssembly(typeof(TSystem))
            };

            var conf = new MapperConfiguration(expression =>
            {
                expression.AddMaps(assemblies);
            });

            conf.AssertConfigurationIsValid();

            var sut = conf.CreateMapper();

            return sut;
        }

        /*
         * Register dependency basic, mapper
         */
        protected virtual void RegisterBasicDependency(ContainerBuilder builder)
        {
            var mapper = GivenTheAllRealMapper();
            builder.RegisterInstance(mapper);
        }
    }
}
