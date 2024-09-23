using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Briefcase
{
    public interface IDepartmentsService
    {
        Task<ApiResponse<IEnumerable<DepartmentResult>>> GetAllDepartment();
        Task<ApiResponse<bool>> AddNewDepartment(DepartmentUpsert departmentAdd);
        Task<ApiResponse<bool>> UpdateDepartment(int id, DepartmentUpsert departmentUpdate);
        Task<ApiResponse<bool>> RemoveDepartment(int id);
    }
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IBaseRepository<Department> _baseRepository;
        private readonly IValidator<DepartmentUpsert> _departmentUpsertValidator;
        private readonly IMapper _mapper;
        public DepartmentsService(
            IBaseRepository<Department> baseRepository,
            IValidator<DepartmentUpsert> departmentUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _departmentUpsertValidator = departmentUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<bool>> AddNewDepartment(DepartmentUpsert departmentAdd)
        {
            try
            {
                var resultValidation = _departmentUpsertValidator.Validate(departmentAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var newDepartment = new Department
                {
                    Name = departmentAdd.Name.Trim(),
                    ManagerId = departmentAdd.ManagerId
                };

                await _baseRepository.AddAsync(newDepartment);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<DepartmentResult>>> GetAllDepartment()
        {
            try
            {
                return new ApiResponse<IEnumerable<DepartmentResult>>
                {
                    Metadata = _mapper.Map<IEnumerable<DepartmentResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RemoveDepartment(int id)
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

        public async Task<ApiResponse<bool>> UpdateDepartment(int id, DepartmentUpsert departmentUpdate)
        {
            try
            {
                var resultValidation = _departmentUpsertValidator.Validate(departmentUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                var department = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                department.Name = departmentUpdate.Name.Trim();
                department.ManagerId = departmentUpdate.ManagerId;

                _baseRepository.Update(department);
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
