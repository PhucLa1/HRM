using HRM.Data.Entities.Base;
using ServiceStack.DataAnnotations;

namespace HRM.Data.Entities
{
    public class EmployeeImage : BaseEntities
    {
        public string? Url { get; set; }
        public int EmployeeId { get; set; }
        public StatusFaceTurn StatusFaceTurn { get; set; }
        public string? Descriptor { get; set; }
    }
    public enum StatusFaceTurn
    {
        Straight = 1,
        TurnLeft = 2,
        TurnRight = 3,
        TurnDown = 4,
        TurnUp = 5,
    }
}
