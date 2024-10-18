using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface IFomulasService
    {
        Task<ApiResponse<IEnumerable<FomulaResult>>> GetAllFomula();
        Task<ApiResponse<bool>> AddNewFomula(FomulaUpsert fomulaAdd);
        Task<ApiResponse<bool>> UpdateFomula(int id, FomulaUpsert fomulaUpdate);
        Task<ApiResponse<bool>> RemoveFomula(int id);

        Task<ApiResponse<IEnumerable<SalaryComponentCategory>>> GetAllSalaryComponents();
    }
    public class FomulasService : IFomulasService
    {
        private readonly IBaseRepository<Fomula> _baseRepository;
        private readonly IValidator<FomulaUpsert> _fomulaUpsertValidator;
        private readonly IBaseRepository<Allowance> _allowanceRepository;
        private readonly IBaseRepository<Insurance> _insuranceRepository;
        private readonly IBaseRepository<Deduction> _deductionRepository;
        private readonly IBaseRepository<Bonus> _bonusRepository;
        private readonly IBaseRepository<TaxDeduction> _taxDeductionRepository;
        private readonly IBaseRepository<TaxRate> _taxRateRepository;
        public FomulasService(IBaseRepository<Fomula> baseRepository,
                               IValidator<FomulaUpsert> fomulaUpsertValidator,
                               IBaseRepository<Allowance> allowanceRepository,
                               IBaseRepository<Insurance> insuranceRepository,
                               IBaseRepository<Deduction> deductionRepository,
                               IBaseRepository<Bonus> bonusRepository,
                               IBaseRepository<TaxDeduction> taxDeductionRepository,
                               IBaseRepository<TaxRate> taxRateRepository)
        {
            _baseRepository = baseRepository;
            _fomulaUpsertValidator = fomulaUpsertValidator;
            _allowanceRepository = allowanceRepository;
            _insuranceRepository = insuranceRepository;
            _deductionRepository = deductionRepository;
            _bonusRepository = bonusRepository;
            _taxDeductionRepository = taxDeductionRepository;
            _taxRateRepository = taxRateRepository;
        }
        public async Task<ApiResponse<IEnumerable<FomulaResult>>> GetAllFomula()
        {
            try
            {
                return new ApiResponse<IEnumerable<FomulaResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new FomulaResult
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ParameterName = e.ParameterName,
                        FomulaDetail = e.FomulaDetail,
                        Note = e.Note
                    }).ToListAsync(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewFomula(FomulaUpsert fomulaAdd)
        {
            try
            {
                var resultValidation = _fomulaUpsertValidator.Validate(fomulaAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Fomula { 
                    Name = fomulaAdd.Name.Trim(),
                    ParameterName = fomulaAdd.ParameterName,
                    FomulaDetail = fomulaAdd.FomulaDetail,
                    Note = fomulaAdd.Note
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateFomula(int id, FomulaUpsert fomulaUpdate)
        {
            try
            {
                var resultValidation = _fomulaUpsertValidator.Validate(fomulaUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var fomula = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                fomula.Name = fomulaUpdate.Name.Trim();
                fomula.ParameterName = fomulaUpdate.ParameterName.Trim();
                fomula.FomulaDetail = fomulaUpdate.FomulaDetail.Trim();
                fomula.Note = fomulaUpdate.Note;
                _baseRepository.Update(fomula);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveFomula(int id)
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

        public async Task<ApiResponse<IEnumerable<SalaryComponentCategory>>> GetAllSalaryComponents()
        {
            var res = new List<SalaryComponentCategory>();
            //Contract Salary
            var contractSalarySC = new SalaryComponentCategory();
            contractSalarySC.Name = "Thành phần tính lương quy định";
            contractSalarySC.ListSalaryComponents = new List<SalaryComponent>()
            {
                new SalaryComponent{ Name="Lương cơ bản", ParameterName ="PARAM_BASE_SALARY"},
                new SalaryComponent{ Name="Lương đóng bảo hiểm", ParameterName ="PARAM_BASE_INSURANCE"},
                new SalaryComponent{ Name="Số ngày quy định", ParameterName ="PARAM_BASE_DAYS"},
                new SalaryComponent{ Name="Số giờ quy định", ParameterName ="PARAM_BASE_HOURS"},
                new SalaryComponent{ Name="Ngày công", ParameterName ="PARAM_BASE_WAGE_DAYS"},
                new SalaryComponent{ Name="Giờ công", ParameterName ="PARAM_BASE_WAGE_HOURS"},
                new SalaryComponent{ Name="Hệ số lương", ParameterName ="PARAM_BASE_FACTOR"},
            };

            //Allowance
            var allowanceSC = new SalaryComponentCategory();
            allowanceSC.Name = "Thành phần phụ cấp";
            allowanceSC.ListSalaryComponents = await _allowanceRepository.GetAllQueryAble().Select(e=>new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //Insurance
            var insuranceSC = new SalaryComponentCategory();
            insuranceSC.Name = "Thành phần bảo hiểm";
            insuranceSC.ListSalaryComponents = await _insuranceRepository.GetAllQueryAble().Select(e => new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //Deduction
            var deductionSC = new SalaryComponentCategory();
            deductionSC.Name = "Các khoản khấu trừ";
            deductionSC.ListSalaryComponents = await _deductionRepository.GetAllQueryAble().Select(e => new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //Bonus
            var bonusSC = new SalaryComponentCategory();
            bonusSC.Name = "Các khoản thưởng";
            bonusSC.ListSalaryComponents = await _bonusRepository.GetAllQueryAble().Select(e => new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //Tax Deduction
            var taxDeductionSC = new SalaryComponentCategory();
            taxDeductionSC.Name = "Các khoản giảm trừ thuế";
            taxDeductionSC.ListSalaryComponents = await _taxDeductionRepository.GetAllQueryAble().Select(e => new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //TaxRate
            var taxRateSC = new SalaryComponentCategory();
            taxRateSC.Name = "% thuế suất (theo biểu lũy tiến)";
            taxRateSC.ListSalaryComponents = await _taxRateRepository.GetAllQueryAble().Select(e => new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //Fomula

            res.Add(contractSalarySC);
            res.Add(allowanceSC);
            res.Add(insuranceSC);
            res.Add(deductionSC);
            res.Add(bonusSC);
            res.Add(taxDeductionSC);
            res.Add(taxRateSC);
            return new ApiResponse<IEnumerable<SalaryComponentCategory>>()
            {
                IsSuccess=true,
                Metadata= res
            };
        }
    }
}
