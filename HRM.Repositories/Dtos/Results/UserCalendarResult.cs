﻿using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class UserCalendarResult
    {
        public ShiftTime ShiftTime { get; set; }
        public bool IsCheck { get; set; }
        public DateOnly PresentShift { get; set; }
        public UserCalendarStatus UserCalendarStatus { get; set; }
    }

    public class UserCalendarResultTotalWork
    {
        public int EmployeeId { get; set; }
        public ShiftTime ShiftTime { get; set; }
        public bool IsCheck { get; set; }
        public DateOnly PresentShift { get; set; }
        public UserCalendarStatus UserCalendarStatus { get; set; }
    }
}
