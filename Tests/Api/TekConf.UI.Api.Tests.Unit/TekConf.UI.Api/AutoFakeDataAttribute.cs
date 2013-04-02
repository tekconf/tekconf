using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Xunit;

namespace TekConf.UI.Api.Tests.Unit
{
	public class AutoFakeDataAttribute : AutoDataAttribute
	{
		public AutoFakeDataAttribute()
			: base(new Fixture().Customize(new AutoFakeItEasyCustomization()))
		{
		}
	}
}