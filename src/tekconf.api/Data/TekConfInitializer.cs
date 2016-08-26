using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TekConf.Api.Data.Models;

namespace TekConf.Api.Data
{
    public class TekConfInitializer : System.Data.Entity.DropCreateDatabaseAlways<TekConfContext>
    {
        protected override void Seed(TekConfContext context)
        {
           
            var robGibbens = new User()
            {
                Bio = "He rocks so hard he has to be 20 chars",
                FirstName = "Rob",
                LastName = "Gibbens",
                Slug = "rob-gibbens",
                
            };
            var xamarinEvolve = new Conference()
            {
                CreatedAt = DateTime.Now,
                Description = "Xamarin Evolve - Mobile apps",
                Name = "Xamarin Evolve",
                Owner = robGibbens,
                Slug = "xamarin-evolve"
            };

            robGibbens.OwnedConferences.Add(xamarinEvolve);

            var xamarinEvolve2017 = new ConferenceInstance()
            {
                Conference = xamarinEvolve,
                Name = "Xamarin Evolve 2017",
                Slug = "2017",
                Description = "Mobile apps are the best thing ever",
                IsOnline = false,
                IsLive = true
            };

            xamarinEvolve.Instances.Add(xamarinEvolve2017);

            context.Conferences.Add(xamarinEvolve);
            context.SaveChanges();
            Console.WriteLine("CHANGES SAVED!!");
            base.Seed(context);
        }
    }

}