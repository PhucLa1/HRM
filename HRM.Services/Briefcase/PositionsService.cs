using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Briefcase
{
    public interface IPositionsService
    {
        Task<ApiResponse<IEnumerable<PositionResult>>> GetAllPosition();
        Task<ApiResponse<bool>> AddNewPosition(PositionUpsert positionAdd);
        Task<ApiResponse<bool>> UpdatePosition(int id, PositionUpsert positionUpdate);
        Task<ApiResponse<bool>> RemovePosition(int id);
    }
    public class PositionsService : IPositionsService
    {
        private readonly IBaseRepository<Position> _baseRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IValidator<PositionUpsert> _positionUpsertValidator;
        private readonly IMapper _mapper;
        public PositionsService(
            IBaseRepository<Position> baseRepository,
            IValidator<PositionUpsert> positionUpsertValidator,
            IBaseRepository<Department> departmentRepository,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _positionUpsertValidator = positionUpsertValidator;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<PositionResult>>> GetAllPosition()
        {
            try
            {
                return new ApiResponse<IEnumerable<PositionResult>>
                {
                    Metadata = await (from p in _baseRepository.GetAllQueryAble()
                                      join d in _departmentRepository.GetAllQueryAble()
                                      on p.DepartmentId equals d.Id
                                      select new PositionResult
                                      {
                                          Id = p.Id,
                                          Name = p.Name,
                                          TotalPositionsNeeded = p.TotalPositionsNeeded,
                                          CurrentPositionsFilled = p.CurrentPositionsFilled,
                                          DepartmentName = d.Name,
                                          DepartmentId = d.Id,
                                      }).ToListAsync(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewPosition(PositionUpsert positionAdd)
        {
            try
            {
                var resultValidation = _positionUpsertValidator.Validate(positionAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(
                    new Position
                    {
                        Name = positionAdd.Name.Trim(),
                        TotalPositionsNeeded = positionAdd.TotalPositionsNeeded,
                        DepartmentId = positionAdd.DepartmentId,
                    }
                );
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdatePosition(int id, PositionUpsert positionUpdate)
        {
            try
            {
                var resultValidation = _positionUpsertValidator.Validate(positionUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                //Nhập lại dữ liệu
                postion.Name = positionUpdate.Name.Trim();
                postion.TotalPositionsNeeded = positionUpdate.TotalPositionsNeeded;
                postion.DepartmentId = positionUpdate.DepartmentId;

                //Lưu thông tin
                _baseRepository.Update(postion);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemovePosition(int id)
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
