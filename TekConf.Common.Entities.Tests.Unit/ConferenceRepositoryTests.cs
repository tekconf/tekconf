using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace TekConf.Common.Entities.Tests.Unit
{
	[TestFixture]
	public class ConferenceRepositoryTests
	{
		private IFixture _fixture;

		[TestFixtureSetUp]
		public void Setup()
		{
			_fixture = new Fixture().Customize(new AutoMoqCustomization());
		}
	}
}