using System;

namespace Tekconf.DTO
{
    public class Schedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }

        public DateTime Created { get; set; }
    }
}