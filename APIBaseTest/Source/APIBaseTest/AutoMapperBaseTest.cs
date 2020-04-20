using Autofac.Extras.Moq;
using AutoMapper;

namespace APIBaseTest
{
    public abstract class AutoMapperBaseTest<TSystem> where TSystem : Profile
    {
        protected IMapper GivenTheSystemUnderTest(AutoMock mock)
        {
            var conf = new MapperConfiguration(expression =>
            {
                expression.AddProfile(typeof(TSystem));
            });

            conf.AssertConfigurationIsValid();

            var sut = conf.CreateMapper();

            return sut;
        }
    }
}
