using NUnit.Framework;
using Ploeh.AutoFixture;
using Moq;
using Should;
using TekConf.Mobile.Core.ViewModels;
using MvvmCross.Test.Core;
using System.Linq;
using AutoMapper;
using TekConf.Mobile.Core;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Core.Platform;

[TestFixture]
public class Test_ConferencesService : MvxIoCSupportingTest
{
	private IFixture _fixture;
	private IMapper _mapper;

	protected MockDispatcher MockDispatcher { get; private set; }
	protected override void AdditionalSetup ()
	{
		//// an automatically Mocked service:
		//var firstService = new Mock<IFirstService> ();
		//Ioc.RegisterSingleton<IFirstService> (firstService.Object);

		//// a manually Mocked service:
		//var secondService = new MockSecondService ();
		//Ioc.RegisterSingleton<ISecondService> (secondService);

		MockDispatcher = new MockDispatcher ();
		Ioc.RegisterSingleton<IMvxViewDispatcher> (MockDispatcher);
		Ioc.RegisterSingleton<IMvxMainThreadDispatcher> (MockDispatcher);

		Ioc.RegisterSingleton<IMvxStringToTypeParser> (new MvxStringToTypeParser ());
	}

	[SetUp]
	public void SetupTest()
	{
		_fixture = new Fixture();

		base.Setup();
	}

	[Test]
	public void should_load_from_local_database()
	{
		true.ShouldBeFalse();
	}

	[Test]
	public void should_load_from_remote_service()
	{
		true.ShouldBeFalse();
	}
}
