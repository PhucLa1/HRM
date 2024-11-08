using Coravel.Invocable;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HRM.Services.Scheduler
{
    public class ChangePartimePlanStatus : IInvocable
    {
        private readonly IBaseRepository<PartimePlan> _partimePlanRepository;
        public ChangePartimePlanStatus(IBaseRepository<PartimePlan> partimePlanRepository)
        {
            _partimePlanRepository = partimePlanRepository;
        }
        public async Task Invoke()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                //Thực hiện tác vụ
                var partimePlans = await _partimePlanRepository
                    .GetAllQueryAble()
                   .Where(e => e.TimeStart <= DateOnly.FromDateTime(DateTime.Now)
                   && e.StatusCalendar == StatusCalendar.Submit)
                   .ExecuteUpdateAsync(s => s.SetProperty(w => w.StatusCalendar, StatusCalendar.Cancel));

                watch.Stop();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
