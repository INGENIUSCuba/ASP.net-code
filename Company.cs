using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Index, MaxLength(100)]
        public string TenantId { get; set; }

        public string Name { get; set; }

        public string TenantAdminEmailAddress { get; set; }

        public string HelpMeSupportEmailAddresses { get; set; }

        public WeekDays HelpMeWeekDaysToReport { get; set; }

        public int HelpMeReportHour { get; set; }

        public int HelpMeLastDayOfReport { get; set; }

        public string Logo { get; set; }

        public string Categories { get; set; }

        public int LastFreePeriodGrace { get; set; }

        public Status Status { get; set; }


    }

    public class NewStatus
    {
        public int ID { get; set; }

        public string Status { get; set; }

    }
}

[Flags]
public enum WeekDays
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4,
    Thursday = 8,
    Friday = 16,
    Saturday = 32,
    Sunday = 64
}

public enum Status
{
    waiting=0,
    enable=1,
    disable=2
}
