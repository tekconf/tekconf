using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TekConf.Core.ViewModels
{
	public class TempViewModel : MvxViewModel
	{
		public List<string> Names
		{
			get
			{
				return new List<string>() { "Rob", "Lynne" };
			}
		}
		public ICommand TestCommand
		{
			get
			{
				return null;
			}
		}

	}
}
