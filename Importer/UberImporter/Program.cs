using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UberImporter.MonkeySpace;
using UberImporter.RailsConf;

namespace UberImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var monkeySpace = new MonkeySpaceImporter();
            var railsConf = new RailsConfImporter();

            //monkeySpace.Import();
            railsConf.Import();
        }
    }
}
