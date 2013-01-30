using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;

namespace TekConf.UI.Web
{
    public class Bootstrapper
    {
        public void BootstrapAutomapper()
        {
            Mapper.CreateMap<FullConferenceDto, CreateConference>();
            Mapper.CreateMap<AddressDto, Address>();

            Mapper.CreateMap<FullSessionDto, AddSession>();

            Mapper.CreateMap<FullSpeakerDto, CreateSpeaker>();
        }
    }
}