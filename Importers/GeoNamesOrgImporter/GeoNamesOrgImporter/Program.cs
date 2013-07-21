using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FileHelpers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace GeoNamesOrgImporter
{
	class Program
	{
		private static List<CountryRecord> countryRecords;
		static void Main(string[] args)
		{
			ImportCountries();
			ImportCities();
		}

		private static void ImportCountries()
		{
			var engine = new FileHelperEngine<CountryRecord>();
			string fileName = @"C:\dev\countries.txt";
			countryRecords = engine.ReadFile(fileName).ToList();
		}

		private static void ImportCities()
		{
			Mapper.CreateMap<GeoNameRecord, GeoLocationEntity>()
						.ForMember(dest => dest._id, opt => opt.Ignore())
						.ForMember(dest => dest.fipscode, opt => opt.MapFrom(x => x.admin1code));

			var files = new DirectoryInfo(@"C:\dev\cities5000\").GetFiles();
			IConfiguration configuration = new Configuration();
			var repository = new GeoLocationRepository(configuration);

			Parallel.ForEach(files, file =>
				{
					var engine = new FileHelperEngine<GeoNameRecord>();
					//string fileName = @"C:\dev\allCountries.txt";
					var geonameRecords = engine.ReadFile(file.FullName).ToList();
					Console.WriteLine("Importing " + file.FullName);
					var geoLocationEntities = Mapper.Map<List<GeoNameRecord>, List<GeoLocationEntity>>(geonameRecords);


					var filtered =
						geoLocationEntities.Where(c => c.countrycode == "US" || c.countrycode == "CA" || c.countrycode == "GB")
						.OrderBy(x => x.countrycode)
						.ToList();

					Parallel.ForEach(filtered, entity =>
						{
							entity.countryname = countryRecords.Where(x => x.ISO == entity.countrycode).Select(x => x.Country).FirstOrDefault();
							if (
								!repository.AsQueryable().Where(x => x.countrycode == entity.countrycode)
								.Where(x => x.latitude == entity.latitude)
								.Where(x => x.longitude == entity.longitude)
								.Any(x => x.name == entity.name))
							{
								repository.Save(entity);
							}

							Console.WriteLine(file.FullName);
						});

				});

		}
	}

}