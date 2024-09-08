using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Web : BaseEntities
    {
        public string? Name { get; set; }
        public string? WebApi { get; set; }
        public ICollection<RecruitmentWeb> recruitmentWebs { get; set; }

    }
}
