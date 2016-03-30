using System.Threading.Tasks;
using Tekconf.DTO;

namespace TekConf.Mobile.Core.Services
{
	public interface IImageService
	{
		Task<string> GetConferenceImagePath (Conference conference);
		Task<string> GetSpeakerImagePath (Conference conference, Speaker speaker);

	}
}