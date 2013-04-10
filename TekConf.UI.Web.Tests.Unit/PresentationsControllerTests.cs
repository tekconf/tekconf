using System.Threading.Tasks;
using System.Web.Mvc;
using FakeItEasy;
using NUnit.Framework;
using Should;
using TekConf.UI.Web.Controllers;
using TekConf.UI.Web.ViewModels;

namespace TekConf.UI.Web.Tests.Unit
{
	[TestFixture]
	public class PresentationsControllerTests
	{
		[Test]
		public void ncrunch() { true.ShouldBeTrue(); }

		[Test]
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
