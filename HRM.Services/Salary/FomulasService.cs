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
                new SalaryComponent{ Name="Phụ cấp trách nhiệm", ParameterName ="PARAM_BASE_TOTAL_ALLOWANCE", Desc="Được tính bằng tổng phụ cấp quy định trong HĐ"},
            };

            //Time keeping Salary
            var timeKeepingSalarySC = new SalaryComponentCategory();
            timeKeepingSalarySC.Name = "Thành phần lấy từ máy chấm công";
            timeKeepingSalarySC.ListSalaryComponents = new List<SalaryComponent>()
            {
                new SalaryComponent{ Name="Số công thực tế (giờ)", ParameterName ="PARAM_REAL_HOURS"},
                new SalaryComponent{ Name="Số công thực tế (ngày)", ParameterName ="PARAM_REAL_DAYS"}
            };

            //Bonus, Deduction
            var autoCollectSC = new SalaryComponentCategory();
            autoCollectSC.Name = "Thành phần tự thu thập";
            autoCollectSC.ListSalaryComponents = new List<SalaryComponent>()
            {
                new SalaryComponent{ Name="Tổng khoản thưởng", ParameterName ="PARAM_TOTAL_BONUS"},
                new SalaryComponent{ Name="Tổng khoản trừ", ParameterName ="PARAM_TOTAL_DEDUCTION"},
                new SalaryComponent{ Name="Tổng tiền ứng lương", ParameterName ="PARAM_TOTAL_ADVANCE"},
            };

            //BHXH, TAX
            var ruleSC = new SalaryComponentCategory();
            ruleSC.Name = "Thành phần được tính theo luật của nước";
            ruleSC.ListSalaryComponents = new List<SalaryComponent>()
            {
                new SalaryComponent{ Name="Tiền BHXH", ParameterName ="PARAM_RULE_BHXH", Desc="Được tính bằng tổng % NLĐ phải đóng cho các bảo hiểm đã quy định trong HĐ"},
                new SalaryComponent{ Name="Thuế TNCN", ParameterName ="PARAM_RULE_TAX"}
            };

            //FORMULA
            var formulaSC = new SalaryComponentCategory();
            formulaSC.Name = "Thành phần đã khai báo công thức tính";
            formulaSC.ListSalaryComponents = await _baseRepository.GetAllQueryAble().Select(e => new SalaryComponent()
            {
                Name = e.Name,
                ParameterName = e.ParameterName
            }).ToListAsync();

            //Fomula
            res.Add(contractSalarySC);
            res.Add(timeKeepingSalarySC);
            res.Add(autoCollectSC);
            res.Add(ruleSC);
            res.Add(formulaSC);
            return new ApiResponse<IEnumerable<SalaryComponentCategory>>()
            {
                IsSuccess=true,
                Metadata= res
            };
        }
    }
}
