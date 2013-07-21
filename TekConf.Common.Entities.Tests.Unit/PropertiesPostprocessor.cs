using Ploeh.AutoFixture.Kernel;

public class PropertiesPostprocessor : ISpecimenBuilder
{
	private readonly ISpecimenBuilder builder;

	public PropertiesPostprocessor(ISpecimenBuilder builder)
	{
		this.builder = builder;
	}

	public object Create(object request, ISpecimenContext context)
	{
		dynamic s = this.builder.Create(request, context);
		if (s is NoSpecimen)
			return s;

		s.SetupAllProperties();
		return s;
	}
}