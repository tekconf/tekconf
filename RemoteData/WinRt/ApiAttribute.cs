using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStack.ServiceHost
{
	public class Api : Attribute
	{
		public Api(string description)
		{

		}
	}

	public class ApiMember : Attribute
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ParameterType { get; set; }
		public string DataType { get; set; }
		public bool IsRequired { get; set; }
	}

}