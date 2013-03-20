using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FakeItEasy;
using Ploeh.AutoFixture.Xunit;
using Should;
using TekConf.UI.Web.Controllers;
using TekConf.UI.Web.ViewModels;
using Xunit;
using Xunit.Extensions;

namespace TekConf.UI.Web.Tests.Unit
{
    public class PresentationsControllerTests
    {
        [Fact]
        public void ncrunch() { true.ShouldBeTrue(); }

        [Theory, AutoFakeData]
        public async Task will_add_new_presentation()
        {
            var repository = A.Fake<IRemoteDataRepositoryAsync>();
            var controller = new PresentationsController(repository);
            var context = A.Fake<ControllerContext>();
            A.CallTo(() => context.HttpContext.User.Identity.Name).Returns("test");
            
            controller.ControllerContext = context;
            var addPresentationViewModel = new AddPresentationViewModel();
            var result = await controller.Add(addPresentationViewModel);

            //var service = new SpeakerService(configuration.FakedObject, conferenceRepository.FakedObject, presentationRepository);
            //var dto = service.Post(presentation.FakedObject) as PresentationDto;

            //A.CallTo(() => presentationRepository.Save(A<PresentationEntity>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }


    }
}
