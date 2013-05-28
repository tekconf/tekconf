using System.Collections.Generic;

namespace TekConf.Core.Repositories
{
	public class ConferenceFavoritesDto
	{
		public ConferenceFavoritesDto()
		{
			sessions = new List<ConferenceFavoriteSessionDto>();
		}

		public string name { get; set; }
		public string slug { get; set; }
		public List<ConferenceFavoriteSessionDto> sessions { get; set; }
	}
}