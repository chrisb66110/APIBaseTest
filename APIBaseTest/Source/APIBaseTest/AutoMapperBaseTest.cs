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
                expression.AddProfile(typeof(TSystem));
            });

            conf.AssertConfigurationIsValid();

            var sut = conf.CreateMapper();

            return sut;
        }
    }
}
