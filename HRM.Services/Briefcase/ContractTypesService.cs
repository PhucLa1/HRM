using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Briefcase
{
    public interface IContractTypesService
    {
        Task<ApiResponse<IEnumerable<ContractTypeResult>>> GetAllContractType();
        Task<ApiResponse<bool>> AddNewContractType(ContractTypeUpsert contractTypeAdd);
        Task<ApiResponse<bool>> UpdateContractType(int id, ContractTypeUpsert contractTypeUpdate);
        Task<ApiResponse<bool>> RemoveContractType(int id);
    }
    public class ContractTypesService : IContractTypesService
    {
        private readonly IBaseRepository<ContractType> _baseRepository;
        private readonly IValidator<ContractTypeUpsert> _contractTypeUpsertValidator;
        private readonly IMapper _mapper;
        public ContractTypesService(
            IBaseRepository<ContractType> baseRepository,
            IValidator<ContractTypeUpsert> contractTypeUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _contractTypeUpsertValidator = contractTypeUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<ContractTypeResult>>> GetAllContractType()
        {
            try
            {
                return new ApiResponse<IEnumerable<ContractTypeResult>>
                {
                    /*
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new ContractTypeResult
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToListAsync(),
                    */
                    Metadata = _mapper.Map<IEnumerable<ContractTypeResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewContractType(ContractTypeUpsert contractTypeAdd)
        {
            try
            {
                var resultValidation = _contractTypeUpsertValidator.Validate(contractTypeAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new ContractType { Name = contractTypeAdd.Name.Trim() });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateContractType(int id, ContractTypeUpsert contractTypeUpdate)
        {
            try
            {
                var resultValidation = _contractTypeUpsertValidator.Validate(contractTypeUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                postion.Name = contractTypeUpdate.Name.Trim();
                _baseRepository.Update(postion);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveContractType(int id)
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
