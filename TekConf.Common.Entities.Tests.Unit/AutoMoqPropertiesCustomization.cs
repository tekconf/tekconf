using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;

public class AutoMoqPropertiesCustomization : ICustomization
{
	public void Customize(IFixture fixture)
	{
		fixture.Customizations.Add(
			new PropertiesPostprocessor(
				new MockPostprocessor(
					new MethodInvoker(
						new MockConstructorQuery()))));
		fixture.ResidueCollectors.Add(new MockRelay());
	}
}