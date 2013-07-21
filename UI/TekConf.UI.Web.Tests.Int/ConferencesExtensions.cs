using ConferencesIO.RemoteData.Dtos.v1;
using KellermanSoftware.CompareNetObjects;
namespace ConferencesIO.UI.Api.Tests.Int
{
  public static class ConferencesExtensions
  {
    public static bool IsTheSameAs(this ConferencesDto source, ConferencesDto expected)
    {
      var compareObjects = new CompareObjects();
      return compareObjects.Compare(source, expected);
    }
  }
}
