using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.TimeKeeping
{
    public static class CalendarError
    {
        public const string CALENDAR_EXIST = "Ca làm việc này đã tồn tại rồi.";
    }
    public interface ICalendarService
    {
        Task<ApiResponse<List<ShiftGroup>>> GetAllCalendar();
        Task<ApiResponse<bool>> AddNew(CalendarUpsert calendarAdd);
        Task<ApiResponse<bool>> UpdateCalendar(int id, CalendarUpsert calendarUpdate);
        Task<ApiResponse<bool>> RemoveCalendar(int id);
    }
    public class CalendarService : ICalendarService
    {
        private readonly IBaseRepository<Calendar> _calendarRepository;
        private readonly IValidator<CalendarUpsert> _calendarUpsertValidator;
        private readonly IMapper _mapper;
        public CalendarService(
            IBaseRepository<Calendar> calendarRepository,
            IValidator<CalendarUpsert> calendarUpsertValidator,
            IMapper mapper
            )
        {
            _calendarRepository = calendarRepository;
            _calendarUpsertValidator = calendarUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<ShiftGroup>>> GetAllCalendar()
        {
            try
            {
                var calendar = await _calendarRepository
                    .GetAllQueryAble()
                    .OrderBy(x => x.ShiftTime)
                    .OrderBy(x => x.Day)
                    .ToListAsync();

                var calendarResult = _mapper.Map<List<CalendarResult>>(calendar);
                var groupedCalendars = calendarResult.GroupBy(x => x.ShiftTime)
                    .Select(e => new ShiftGroup
                    {
                        ShiftTime = e.Key,
                        CalendarResult = e.ToList()
                    })
                    .ToList();
                return new ApiResponse<List<ShiftGroup>> { Metadata = groupedCalendars, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddNew(CalendarUpsert calendarAdd)
        {
            try
            {
                var resultValidation = _calendarUpsertValidator.Validate(calendarAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var calendar = _calendarRepository
                    .GetAllQueryAble()
                    .Where(e => e.Day == calendarAdd.Day && e.ShiftTime == calendarAdd.ShiftTime)
                    .FirstOrDefault();
                if (calendar != null)
                {
                    //Tồn tại ca làm này rồi
                    return new ApiResponse<bool> { Message = [CalendarError.CALENDAR_EXIST] };
                }
                await _calendarRepository.AddAsync(new Calendar
                {
                    Day = calendarAdd.Day,
                    ShiftTime = calendarAdd.ShiftTime,
                    TimeEnd = calendarAdd.TimeEnd,
                    TimeStart = calendarAdd.TimeStart,
                });
                await _calendarRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateCalendar(int id, CalendarUpsert calendarUpdate)
        {
            try
            {
                var resultValidation = _calendarUpsertValidator.Validate(calendarUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                //Kiểm tra ca làm việc đó đã tổn tại chưa
                var calendar = await _calendarRepository
                    .GetAllQueryAble()
                    .Where(e => e.Day == calendarUpdate.Day && e.ShiftTime == calendarUpdate.ShiftTime && e.Id != id)
                    .FirstOrDefaultAsync();
                if (calendar != null)
                {
                    //Tồn tại ca làm này rồi
                    return new ApiResponse<bool> { Message = [CalendarError.CALENDAR_EXIST] };
                }
                var calendarNeedUpdate = await _calendarRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == id)
                    .FirstAsync();
                //Gán thuộc tính
                calendarNeedUpdate.Day = calendarUpdate.Day;
                calendarNeedUpdate.ShiftTime = calendarUpdate.ShiftTime;
                calendarNeedUpdate.TimeEnd = calendarUpdate.TimeEnd;
                calendarNeedUpdate.TimeStart = calendarUpdate.TimeStart;

                //Excute 
                _calendarRepository.Update(calendarNeedUpdate);
                await _calendarRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RemoveCalendar(int id)
        {
            try
            {
                await _calendarRepository.RemoveAsync(id);
                await _calendarRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
