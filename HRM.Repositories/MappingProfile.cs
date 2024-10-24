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


           

            //Briefcase
            CreateMap<Department, DepartmentResult>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Position,PositionResult >()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<ContractType, ContractTypeResult>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<ContractSalary, ContractSalaryResult>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Allowance, AllowanceResult>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Insurance, InsuranceResult>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Contract, ContractResult>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

            #endregion



            #region
            //RecruitmentManager
            CreateMap<Web, WebResult>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
			CreateMap<Test, TestResults>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
			CreateMap<Questions, QuestionResult>().ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

			#endregion

			#region
			//TimeKeeping
			CreateMap<Calendar, CalendarResult>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<LeaveApplication, LeaveApplicationResult>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

            #endregion




        }
    }
}
