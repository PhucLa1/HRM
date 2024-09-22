using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Briefcase
{
    public interface IContractSalarysService
    {
        Task<ApiResponse<IEnumerable<ContractSalaryResult>>> GetAllContractSalary();
        Task<ApiResponse<bool>> AddNewContractSalary(ContractSalaryUpsert contractSalaryAdd);
        Task<ApiResponse<bool>> UpdateContractSalary(int id, ContractSalaryUpsert contractSalaryUpdate);
        Task<ApiResponse<bool>> RemoveContractSalary(int id);
    }
    public class ContractSalarysService : IContractSalarysService
    {
        private readonly IBaseRepository<ContractSalary> _baseRepository;
        private readonly IValidator<ContractSalaryUpsert> _contractSalaryUpsertValidator;
        private readonly IMapper _mapper;
        public ContractSalarysService(
            IBaseRepository<ContractSalary> baseRepository,
            IValidator<ContractSalaryUpsert> contractSalaryUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _contractSalaryUpsertValidator = contractSalaryUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<ContractSalaryResult>>> GetAllContractSalary()
        {
            try
            {
                return new ApiResponse<IEnumerable<ContractSalaryResult>>
                {
                    /*
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new ContractSalaryResult
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToListAsync(),
                    */
                    Metadata = _mapper.Map<IEnumerable<ContractSalaryResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewContractSalary(ContractSalaryUpsert contractSalaryAdd)
        {
            try
            {
                var resultValidation = _contractSalaryUpsertValidator.Validate(contractSalaryAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new ContractSalary { 
                    BaseSalary = contractSalaryAdd.BaseSalary,
                    BaseInsurance = contractSalaryAdd.BaseInsurance,
                    RequiredDays = contractSalaryAdd.RequiredDays,
                    RequiredHours = contractSalaryAdd.RequiredHours,
                    WageDaily = contractSalaryAdd.WageDaily,
                    WageHourly = contractSalaryAdd.WageHourly,
                    Factor = contractSalaryAdd.Factor,
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateContractSalary(int id, ContractSalaryUpsert contractSalaryUpdate)
        {
            try
            {
                var resultValidation = _contractSalaryUpsertValidator.Validate(contractSalaryUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var contractSalary = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                contractSalary.BaseSalary = contractSalaryUpdate.BaseSalary;
                contractSalary.BaseInsurance = contractSalaryUpdate.BaseInsurance;
                contractSalary.RequiredDays = contractSalaryUpdate.RequiredDays;
                contractSalary.RequiredHours = contractSalaryUpdate.RequiredHours;
                contractSalary.WageDaily = contractSalaryUpdate.WageDaily;
                contractSalary.WageHourly = contractSalaryUpdate.WageHourly;
                contractSalary.Factor = contractSalaryUpdate.Factor;
                _baseRepository.Update(contractSalary);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveContractSalary(int id)
        {
            try
            {
                await _baseRepository.RemoveAsync(id);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
