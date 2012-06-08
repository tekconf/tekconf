// Type: System.Web.Routing.RouteTable
// Assembly: System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\System.Web\v4.0_4.0.0.0__b03f5f7f11d50a3a\System.Web.dll

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Web.Routing
{
  /// <summary>
  /// Stores the URL routes for an application.
  /// </summary>
  [TypeForwardedFrom("System.Web.Routing, Version=3.5.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
  public class RouteTable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Web.Routing.RouteTable"/> class.
    /// </summary>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public RouteTable();
    /// <summary>
    /// Gets a collection of objects that derive from the <see cref="T:System.Web.Routing.RouteBase"/> class.
    /// </summary>
    /// 
    /// <returns>
    /// An object that contains all the routes in the collection.
    /// </returns>
    public static RouteCollection Routes { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
  }
}
