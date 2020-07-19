using System.Reflection;
using Autofac.Extras.Moq;
using AutoMapper;

namespace APIBaseTest
{
    public abstract class AutoMapperBaseTest<TSystem> : BaseTest<IMapper>
        where TSystem : Profile
    {
        protected override IMapper GivenTheSystemUnderTest(AutoMock mock)
        {
            var conf = new MapperConfiguration(expression =>
            {
                var assembly = Assembly.GetAssembly(typeof(TSystem));
                expression.AddMaps(assembly);
            });

            conf.AssertConfigurationIsValid();

            var sut = conf.CreateMapper();

            return sut;
        }
    }
}
