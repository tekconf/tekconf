using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xunit;

namespace RemoteData.Shared.Tests.Int
{
  [TestFixture]
  public class Tests
  {
    [Test]
    public void GetConferences()
    {
      RemoteData remoteData = new RemoteData();
      remoteData.GetAllConferences(null);
    }
  }
}
