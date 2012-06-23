using KellermanSoftware.CompareNetObjects;
using Should.Core;
namespace ArtekSoftware.Conference.UI.Web.Tests.Int
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
