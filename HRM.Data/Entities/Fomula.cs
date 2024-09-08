using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Fomula : BaseEntities
    {
        public required string Name { get; set; }
        public string? FomulaDetail { get; set; }
        public string? Note { get; set; }
    }
}
