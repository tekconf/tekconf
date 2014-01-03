using Should;
using NUnit.Framework;

namespace TekConf.UI.Api.Tests.Unit
{
	public class SpeakerServiceTests
	{
		public SpeakerServiceTests()
		{
			var container = new Funq.Container();
			var bootstrapper = new Bootstrapper();
			bootstrapper.BootstrapAutomapper(container);
		}

		[Test]
		public void ncrunch()
		{
			true.ShouldBeTrue();
		}

		//[Theory, AutoFakeData]
		//public void will_create_new_presentation([Frozen]Fake<IEntityConfiguration> configuration,
		//																			[Frozen]Fake<IRepository<ConferenceEntity>> conferenceRepository,
		//																			IRepository<PresentationEntity> presentationRepository,
		//																			[Frozen]Fake<CreatePresentation> presentation)
		//{
		//	var service = new SpeakerService(configuration.FakedObject, conferenceRepository.FakedObject, presentationRepository);
		//	var dto = service.Post(presentation.FakedObject) as PresentationDto;

		//	A.CallTo(() => presentationRepository.Save(A<PresentationEntity>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
		//}

		//[Theory, AutoFakeData]
		//public void will_create_return_valid_response([Frozen]Fake<IEntityConfiguration> configuration,
		//																					[Frozen]Fake<IRepository<ConferenceEntity>> conferenceRepository,
		//																					[Frozen]Fake<IRepository<PresentationEntity>> presentationRepository,
		//																					Fake<CreatePresentation> presentation)
		//{
		//	var service = new SpeakerService(configuration.FakedObject, conferenceRepository.FakedObject, presentationRepository.FakedObject);
		//	var response = service.Post(presentation.FakedObject) as PresentationDto;

		//	response.ShouldNotBeNull();
		//	response.Title.ShouldEqual(presentation.FakedObject.Title);
		//	response.Slug.ShouldEqual(presentation.FakedObject.Slug);
		//	response.Description.ShouldEqual(presentation.FakedObject.Description);
		//}
	}
}