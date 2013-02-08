using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using TekConf.Common.Entities.Repositories;
using TekConf.UI.Api;

namespace TekConf.Common.Entities.Tests.Int
{
	[TestFixture]
	public class ConferenceRepositoryTests
	{
		[Test]
		public void Should_return_geo_results()
		{
			IConfiguration configuration = new Configuration();
			var conferenceRepository = new ConferenceRepository(configuration);

			var conferences = conferenceRepository.GeoSearch(latitude: 42.467051, longitude: -83.409285, rangeInMiles: 25);

			conferences.ShouldNotBeNull();
			conferences.Count().ShouldEqual(6);
		}

		[Test]
		public void Should_find_conferences_near_city_name()
		{
			IConfiguration configuration = new Configuration();
			var geolocationRepository = new GeoLocationRepository(configuration);
			var cityName = "Seattle";
			var state = "WA";
			double distance = 100;

			var city = geolocationRepository.AsQueryable()
				.Where(g => Regex.IsMatch(g.name, cityName, RegexOptions.IgnoreCase))
				.Where(g => Regex.IsMatch(g.fipscode, state, RegexOptions.IgnoreCase))
				.ToList()
				.Where(x => x.name.ToLower() == cityName.ToLower())
				.FirstOrDefault();

			var conferenceRepository = new ConferenceRepository(configuration);
			var conferences = conferenceRepository.GeoSearch(latitude: city.latitude, longitude: city.longitude, rangeInMiles: distance);
			conferences.Count.ShouldEqual(1);
		}


		[Test]
		[Ignore]
		public void Should_update_position_for_cities()
		{
			IConfiguration configuration = new Configuration();
			var conferenceRepository = new ConferenceRepository(configuration);
			var conferencesInUSA = conferenceRepository.AsQueryable()
													.Where(x => x.address.City != null)
													.Where(x => x.address.Country != null)
													.Where(x => x.address.State != null)
													.ToList();

			var geolocationRepository = new GeoLocationRepository(configuration);

			//Parallel.ForEach(conferencesInUSA, conference =>
			//{
			foreach (var conference in conferencesInUSA)
			{
				var city = geolocationRepository.AsQueryable()
																				.Where(g => Regex.IsMatch(g.name, conference.address.City, RegexOptions.IgnoreCase))
																				.Where(g => Regex.IsMatch(g.fipscode, conference.address.State, RegexOptions.IgnoreCase))
																				.ToList()
																				.FirstOrDefault(x => x.name.ToLower() == conference.address.City.ToLower());

				if (city != null)
				{
					conference.position = new double[] {city.longitude, city.latitude};
					conferenceRepository.Save(conference);
				}
			}
			//});

		}
	}
}
