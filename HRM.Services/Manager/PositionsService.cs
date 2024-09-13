﻿using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Manager
{
    public interface IPositionsService
    {
        Task<ApiResponse<IEnumerable<PositionResult>>> GetAllPosition();
        Task<ApiResponse<bool>> AddNewPosition(PositionAdd positionAdd);
        Task<ApiResponse<bool>> UpdatePosition(int id, PositionUpdate positionUpdate);
        Task<ApiResponse<bool>> RemovePosition(int id);
    }
    public class PositionsService : IPositionsService
    {
        private readonly IBaseRepository<Position> _baseRepository;
        private readonly IValidator<PositionAdd> _positionAddValidator;
        private readonly IValidator<PositionUpdate> _positionUpdateValidator;
        private readonly IMapper _mapper;
        public PositionsService(IBaseRepository<Position> baseRepository, 
            IValidator<PositionAdd> positionAddValidator,
            IValidator<PositionUpdate> positionUpdateValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _positionAddValidator = positionAddValidator;
            _positionUpdateValidator = positionUpdateValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<PositionResult>>> GetAllPosition()
        {
            try
            {
                return new ApiResponse<IEnumerable<PositionResult>>
                {
                    /*
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new PositionResult
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToListAsync(),
                    */
                    Metadata = _mapper.Map<IEnumerable<PositionResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewPosition(PositionAdd positionAdd)
        {
            try
            {
                var resultValidation = _positionAddValidator.Validate(positionAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Position { Name = positionAdd.Name.Trim() });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdatePosition(int id, PositionUpdate positionUpdate)
        {
            try
            {
                var resultValidation = _positionUpdateValidator.Validate(positionUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                postion.Name = positionUpdate.Name.Trim();
                _baseRepository.Update(postion);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch(Exception ex)
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
