using System.Threading.Tasks;
using System.Collections.Generic;
using Tekconf.DTO;
using Fusillade;

namespace TekConf.Mobile.Core.Services
{
	public interface ISchedulesService
	{
		Task<List<Schedule>> GetSchedules(Priority priority);
		Task<Schedule> GetSchedule(Priority priority, string slug);
		Task<Schedule> AddToSchedule(Priority priority, string slug);
        Task RemoveFromSchedule(Priority priority, string slug);
    }
}