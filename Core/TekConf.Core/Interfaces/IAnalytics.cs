using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TekConf.Core.Interfaces
{
	public interface IAnalytics
	{
		void SendView(string view);
	}
}
