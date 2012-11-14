using NUnit.Framework;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.v1;
using Ploeh.AutoFixture;

namespace TekConf.UI.Api.Tests.Int
{
    [TestFixture]
    public class ScheduleTests
    {
        private IFixture _fixture;
        [TestFixtureSetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void should_create_new_schedule()
        {
            var service = new ScheduleService();

            var request = new AddSessionToSchedule()
                                               {
                                                   authenticationMethod = "Facebook",
                                                   authenticationToken = _fixture.CreateAnonymous<string>(),
                                                   conferenceSlug = "codemash-2013",
                                                   sessionSlug = "building-real-time-web-applications"
                                               };
            var bootstrapper = new Bootstrapper();
            bootstrapper.BootstrapAutomapper();

            var response = service.Post(request);
        }
    }
}
