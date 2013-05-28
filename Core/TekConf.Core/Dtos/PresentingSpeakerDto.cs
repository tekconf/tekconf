using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
	public class PresentingSpeakerDto : FullSpeakerDto
	{
		public List<PresentationDto> Presentations { get; set; }
	}
}