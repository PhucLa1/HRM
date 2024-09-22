using AutoMapper;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using TestResult = HRM.Repositories.Dtos.Results.TestResult;

namespace HRM.Repositories
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            #region 
            //Briefcase

            CreateMap<Position,PositionResult >()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

			#endregion

			#region
			//RecruitmentManager
			CreateMap<Web, WebResult>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
			CreateMap<Job, JobResult>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
			CreateMap<Test, TestResult>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
			CreateMap<Questions, QuestionResult>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

			#endregion

			#region
			//TimeKeeping
			CreateMap<Calendar, CalendarResult>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));


            #endregion



            #region
            //TimeKeeping
            CreateMap<Calendar, CalendarResult>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));


            #endregion




        }
    }
}
