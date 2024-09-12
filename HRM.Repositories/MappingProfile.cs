using AutoMapper;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;

namespace HRM.Repositories
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            #region 

            CreateMap<Position,PositionResult >()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

            #endregion



        }
    }
}
