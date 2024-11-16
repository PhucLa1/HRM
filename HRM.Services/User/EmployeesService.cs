using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using HRM.Repositories.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HRM.Services.User
{
    public static class EmployeesError
    {
        public const string INSUFFICIENT = "Không đủ trạng thái khuôn mặt được truyền vào .";
    }
    public interface IEmployeesService
    {
        Task<ApiResponse<bool>> RegistrationFace(int employeeId, List<FaceRegis> faceRegises);
        Task<ApiResponse<List<FaceRegisResult>>> GetAllFaceRegisByEmployeeId(int id);
        Task<ApiResponse<bool>> UpdateFaceRegis(int employeeId, List<FaceRegisUpdate> faceRegisUpdates);
        
        //employee crud
        Task<ApiResponse<IEnumerable<EmployeeResult>>> GetAllEmployee();
        Task<ApiResponse<EmployeeResult>> GetEmployeeInfoByContract(int contractId);
        Task<ApiResponse<IEnumerable<int>>> GetContractIdsNotInUsed();

        Task<ApiResponse<bool>> AddEmployee(EmployeeUpsert employeeAdd);
        Task<ApiResponse<bool>> UpdateEmployee(int id, EmployeeUpsert employeeUpdate);
        Task<ApiResponse<bool>> RemoveEmployee(int id);
        Task<ApiResponse<ProfileDetail>> GetCurrentProfileUser();
        Task<ApiResponse<List<LabelDescriptions>>> GetAllLabelDescription();
    }
    public class EmployeesService : IEmployeesService
    {
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<ContractSalary> _contractSalaryRepository;
        private readonly IBaseRepository<EmployeeImage> _employeeImageRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<ContractType> _contractTypeRepository;
        private readonly IValidator<FaceRegis> _faceRegisValidator;
        private readonly CompanySetting _serverCompanySetting;
        private const string FOLDER_EMPLOYEE_IMAGE = "Employee";
        private const string FOLDER_CV_NAME = "CV";
        private readonly IBaseRepository<TaxDeductionDetails> _taxDeductionDetailsRepository;
        private readonly IValidator<EmployeeUpsert> _employeeUpsertValidator;
        public EmployeesService(
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<EmployeeImage> employeeImageRepository,
            IValidator<FaceRegis> faceRegisValidator,
            IBaseRepository<Contract> contractRepository,
            IOptions<CompanySetting> serverCompanySetting,
            IBaseRepository<TaxDeductionDetails> taxDeductionDetailsRepository,
            IBaseRepository<ContractSalary> contractSalaryRepository,
            IBaseRepository<Department> departmentRepository,
            IBaseRepository<Position> positionRepository,
            IValidator<EmployeeUpsert> employeeUpsertValidator,
            IBaseRepository<ContractType> contractTypeRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeImageRepository = employeeImageRepository;
            _faceRegisValidator = faceRegisValidator;
            _contractRepository = contractRepository;
            _serverCompanySetting = serverCompanySetting.Value;
            _taxDeductionDetailsRepository = taxDeductionDetailsRepository;
            _contractSalaryRepository = contractSalaryRepository;
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _contractTypeRepository = contractTypeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _employeeUpsertValidator = employeeUpsertValidator;
        }
        public async Task<ApiResponse<List<LabelDescriptions>>> GetAllLabelDescription()
        {
            try
            {
                // Lấy tên , id employee và gộp descriptor vào thành 1 mảng
                var labelEmployees = await (from em in _employeeRepository.GetAllQueryAble().Include(e => e.employeeImages)
                                            join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                            where em.employeeImages!.Select(e => e.Descriptor).ToList()!.Count > 0
                                            select new LabelDescriptions
                                            {
                                                Id = em.Id,
                                                Name = c.Name,
                                                Descriptions = em.employeeImages!.Select(e => e.Descriptor).ToList()!
                                            })
                                            .ToListAsync();

                return new ApiResponse<List<LabelDescriptions>> { IsSuccess = true, Metadata = labelEmployees };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<ProfileDetail>> GetCurrentProfileUser()
        {
            try
            {
                var currentId = _employeeRepository.Context.GetCurrentUserId();

                var employeeProfile = await (from em in _employeeRepository.GetAllQueryAble()
                                             join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                             join ct in _contractTypeRepository.GetAllQueryAble() on c.ContractTypeId equals ct.Id
                                             join p in _positionRepository.GetAllQueryAble() on c.PositionId equals p.Id
                                             join d in _departmentRepository.GetAllQueryAble() on p.DepartmentId equals d.Id
                                             join cs in _contractSalaryRepository.GetAllQueryAble() on c.ContractSalaryId equals cs.Id
                                             where em.Id == currentId
                                             select new ProfileDetail
                                             {
                                                 UserName = em.UserName,
                                                 Password = em.Password,
                                                 Email = em.Email,
                                                 TypeContract = c.TypeContract == TypeContract.Fulltime ? "FullTime" : "Partime",
                                                 DepartmentName = d.Name,
                                                 PositionName = p.Name,
                                                 ContractTypeName = ct.Name,
                                                 Name = c.Name,
                                                 DOB = c.DateOfBirth,
                                                 Address = c.Address,
                                                 Gender = c.Gender == true ? "Nam" : "Nữ",
                                                 Countryside = c.CountrySide,
                                                 NationalId = c.NationalID,
                                                 Level = c.Level,
                                                 Major = c.Major,
                                                 BaseSalary = cs.BaseSalary,
                                                 BaseInsurance = cs.BaseInsurance,
                                                 RequiredDays = cs.RequiredDays,
                                                 RequiredHours = cs.RequiredHours,
                                                 WageDaily = cs.WageDaily,
                                                 WageHourly = cs.WageHourly,
                                                 Factor = cs.Factor,
                                                 
                                                 FileUrlSigned = c.FileUrlSigned,
                                                 FireUrlBase = c.FireUrlBase,
                                                 ContractId = c.Id,
                                                 EmployeeSignStatus = c.EmployeeSignStatus
                                             })
                                             .FirstAsync();

                return new ApiResponse<ProfileDetail> { Metadata = employeeProfile, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse<bool>> UpdateFaceRegis(int employeeId, List<FaceRegisUpdate> faceRegisUpdates)
        {
            try
            {
                var listIds = faceRegisUpdates.Select(e => e.Id).ToList();
                var employeeImages = await _employeeImageRepository
                    .GetAllQueryAble()
                    .Where(e => listIds.Contains(e.Id) && e.EmployeeId == employeeId)
                    .ToListAsync();

                //Thay đổi giá trị
                for (int i = 0; i < listIds.Count; i++)
                {
                    var employeeImage = employeeImages[i];
                    var faceRegisUpdate = faceRegisUpdates[i];

                    //Xóa ảnh cũ
                    HandleFile.DELETE_FILE(FOLDER_EMPLOYEE_IMAGE, employeeImage.Url!);

                    //Cập nhật thông tin mới
                    employeeImage.Url = HandleFile.UPLOAD(FOLDER_EMPLOYEE_IMAGE, faceRegisUpdate.FaceFile!);
                    employeeImage.StatusFaceTurn = faceRegisUpdate.StatusFaceTurn;
                    employeeImage.Descriptor = faceRegisUpdate.Descriptor;


                }

                _employeeImageRepository.UpdateMany(employeeImages);
                await _employeeImageRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<FaceRegisResult>>> GetAllFaceRegisByEmployeeId(int id)
        {
            try
            {
                var employeeImages = await _employeeImageRepository
                    .GetAllQueryAble()
                    .Where(e => e.EmployeeId == id)
                    .Select(e => new FaceRegisResult
                    {
                        Id = e.Id,
                        Url = _serverCompanySetting.CompanyHost + FOLDER_EMPLOYEE_IMAGE + "/" + e.Url,
                        StatusFaceTurn = e.StatusFaceTurn,
                        Descriptor = e.Descriptor,
                    })
                    .ToListAsync();
                return new ApiResponse<List<FaceRegisResult>> { IsSuccess = true, Metadata = employeeImages };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RegistrationFace(int employeeId, List<FaceRegis> faceRegises)
        {
            try
            {
                //Check có đủ 5 ảnh không
                List<StatusFaceTurn> requiredStatuses = new List<StatusFaceTurn>()
                {
                    StatusFaceTurn.TurnUp,
                    StatusFaceTurn.TurnDown,
                    StatusFaceTurn.TurnLeft,
                    StatusFaceTurn.TurnRight,
                    StatusFaceTurn.Straight
                };
                var existingStatuses = faceRegises
                    .Select(fr => fr.StatusFaceTurn)
                    .ToList(); // Assuming FaceRegis has a property called Status
                // Check if all required statuses are present
                bool allRequiredStatusesPresent = requiredStatuses.All(status => existingStatuses.Contains(status));
                //Check validate
                if (!allRequiredStatusesPresent)
                {
                    return new ApiResponse<bool> { Message = [EmployeesError.INSUFFICIENT] };
                }

                //Check validate từng cái
                foreach (var faceRegis in faceRegises)
                {
                    var resultValidation = _faceRegisValidator.Validate(faceRegis);
                    if (!resultValidation.IsValid)
                    {
                        return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                    }
                }
                //Thêm ảnh mới vào
                var employeeImages = new List<EmployeeImage>();
                foreach (var faceRegis in faceRegises)
                {
                    var employeeImage = new EmployeeImage
                    {
                        StatusFaceTurn = faceRegis.StatusFaceTurn,
                        Url = HandleFile.UPLOAD(FOLDER_EMPLOYEE_IMAGE, faceRegis.FaceFile!),
                        EmployeeId = employeeId,
                        Descriptor = faceRegis.Descriptor,
                    };
                    employeeImages.Add(employeeImage);
                }
                await _employeeImageRepository.AddRangeAsync(employeeImages);
                await _employeeImageRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse<IEnumerable<EmployeeResult>>> GetAllEmployee()
        {
            try
            {
                var allTaxDeductionDetails = _taxDeductionDetailsRepository.GetAllQueryAble();
                var employeeInfo = (from e in _employeeRepository.GetAllQueryAble()
                                    join c in _contractRepository.GetAllQueryAble() on e.ContractId equals c.Id into empGroup
                                    from c in empGroup.DefaultIfEmpty()
                                    join p in _positionRepository.GetAllQueryAble() on c.PositionId equals p.Id into contractGroup
                                    from p in contractGroup.DefaultIfEmpty()
                                    join d in _departmentRepository.GetAllQueryAble() on p.DepartmentId equals d.Id into positonGroup
                                    from d in positonGroup.DefaultIfEmpty()
                                    select new EmployeeResult
                                    {
                                        Id = e.Id,
                                        ContractId = c.Id,
                                        Name = c.Name,
                                        DepartmentName = d.Name,
                                        PositionName = p.Name,
                                        Email = e.Email,
                                        PhoneNumber = e.PhoneNumber,
                                        DateOfBirth = c.DateOfBirth,
                                        Age = DateTime.Now.Year - c.DateOfBirth.Year - (DateTime.Now.DayOfYear < c.DateOfBirth.DayOfYear ? 1 : 0), // Tính tuổi
                                        Gender = c.Gender,
                                        Tenure = DateTime.Now.Year - c.StartDate.Year - (DateTime.Now.DayOfYear < c.StartDate.DayOfYear ? 1 : 0), // Tính thâm niên
                                        Address = c.Address,
                                        CountrySide = c.CountrySide,
                                        NationalID = c.NationalID,
                                        NationalStartDate = c.NationalStartDate,
                                        NationalAddress = c.NationalAddress,
                                        Level = c.Level,
                                        Major = c.Major,
                                        PositionId = c.PositionId,
                                        DepartmentId = d.Id,

                                        TaxDeductionIds = allTaxDeductionDetails.Where(x=>x.EmployeeId == e.Id).Select(x=>x.TaxDeductionId).ToList(),
                                        UserName = e.UserName??"undefined",
                                        Password = "?????????"
                                        
                                    }).ToList();
                return new ApiResponse<IEnumerable<EmployeeResult>>
                {

                    Metadata = employeeInfo,
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<EmployeeResult>> GetEmployeeInfoByContract(int contractId)
        {       
            try
            {
                var selectedContract = await _contractRepository.GetAllQueryAble().Where(x => x.Id == contractId).FirstOrDefaultAsync();
                if (selectedContract == null) throw new Exception("Không tìm thấy thoog tin hợp đồng!");
                var selectedPosition = await _positionRepository.GetAllQueryAble().FirstOrDefaultAsync(x => x.Id == selectedContract.PositionId);
                var department = await _departmentRepository.GetAllQueryAble().FirstOrDefaultAsync(x => x.Id == selectedPosition.DepartmentId);
                var employeeInfor = new EmployeeResult
                {
                    ContractId = selectedContract.Id,
                    Name = selectedContract.Name??"",
                    DepartmentName = department?.Name??"Không có phòng ban",
                    PositionName = selectedPosition?.Name??"Không chức vị",
                    DateOfBirth = selectedContract.DateOfBirth,
                    Age = DateTime.Now.Year - selectedContract.DateOfBirth.Year - (DateTime.Now.DayOfYear < selectedContract.DateOfBirth.DayOfYear ? 1 : 0), // Tính tuổi
                    Gender = selectedContract.Gender,
                    Tenure = DateTime.Now.Year - selectedContract.StartDate.Year - (DateTime.Now.DayOfYear < selectedContract.StartDate.DayOfYear ? 1 : 0), // Tính thâm niên
                    Address = selectedContract.Address??"",
                    CountrySide = selectedContract.CountrySide??"",
                    NationalID = selectedContract.NationalID??"",
                    NationalStartDate = selectedContract.NationalStartDate,
                    NationalAddress = selectedContract.NationalAddress??"",
                    Level = selectedContract.Level??"",
                    Major = selectedContract.Major??"",
                    PositionId = selectedPosition.Id,
                    DepartmentId = department?.Id??0,
                    Password = "?????????"

                };
                return new ApiResponse<EmployeeResult> { IsSuccess = true, Metadata = employeeInfor };
            }
            catch (Exception ex)
            {

                return new ApiResponse<EmployeeResult> { IsSuccess = false, Message = new List<string>() { ex.Message }, Metadata = null };
            }
        }
        public async Task<ApiResponse<IEnumerable<int>>> GetContractIdsNotInUsed()
        {
            try
            {
                var lstContractIdInused = await _employeeRepository.GetAllQueryAble().Select(x=>x.ContractId).ToListAsync();
                var lstContractNOTInUsed = await _contractRepository.GetAllQueryAble().Where(x=> !lstContractIdInused.Contains(x.Id)).Select(x => x.Id).ToListAsync();
                return new ApiResponse<IEnumerable<int>>
                {
                    Metadata = lstContractNOTInUsed,
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<int>> { IsSuccess = false, Message = new List<string>() { ex.Message }, Metadata = new List<int>() };
            }
        }
        public async Task<ApiResponse<bool>> AddEmployee(EmployeeUpsert employeeAdd)
        {
            try
            {
                var resultValidation = _employeeUpsertValidator.Validate(employeeAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                var allEmployee = await _employeeRepository.GetAllQueryAble().ToListAsync();

                //Check contract in used by other
                if (allEmployee.Any(x => x.ContractId == employeeAdd.ContractId))
                {
                    throw new Exception("Hợp đồng không hợp lệ (do đã có người khác sử dụng)");
                }

                //check email is unique
                if (allEmployee.Any(x => x.Email == employeeAdd.Email))
                {
                    throw new Exception("Email đã được đăng kí bới tài khoản khác!");
                }


                //check username is unique
                if (allEmployee.Any(x => x.UserName == employeeAdd.UserName))
                {
                    throw new Exception("UserName đã được đăng kí bới tài khoản khác!");
                }

                var newEmployee = new Employee
                {
                    ContractId = employeeAdd.ContractId,
                    PhoneNumber = employeeAdd.PhoneNumber,
                    UserName = employeeAdd.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(employeeAdd.Password),
                    Avatar = employeeAdd.Avatar ?? "user.png",
                    StatusEmployee = StatusEmployee.Active

                };

                using(var transaction = await _employeeRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _employeeRepository.AddAsync(newEmployee);
                        await _employeeRepository.SaveChangeAsync();

                        if (employeeAdd.TaxDeductionIds != null && employeeAdd.TaxDeductionIds.Count > 0)
                        {
                            var lstTaxDeductionDetails = new List<TaxDeductionDetails>();
                            foreach (var taxDeductionId in employeeAdd.TaxDeductionIds)
                            {
                                lstTaxDeductionDetails.Add(new TaxDeductionDetails
                                {
                                    TaxDeductionId = taxDeductionId,
                                    EmployeeId = newEmployee.Id
                                });
                            }
                            await _taxDeductionDetailsRepository.AddRangeAsync(lstTaxDeductionDetails);
                            await _taxDeductionDetailsRepository.SaveChangeAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }
                  
                }
              
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = new List<string>() { ex.Message} };
            }
        }
        public async Task<ApiResponse<bool>> UpdateEmployee(int id, EmployeeUpsert employeeUpdate)
        {
            try
            {
                var resultValidation = _employeeUpsertValidator.Validate(employeeUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var allEmployee = await _employeeRepository.GetAllQueryAble().ToListAsync();

                var selectedEmployee = allEmployee.Where(e => e.Id == id).FirstOrDefault();
                if (selectedEmployee == null)
                {
                    throw new Exception("Không tìm thấy nhân viên tương ứng!");
                }

                //Check contract in used by other
                if (allEmployee.Any(x => x.Id!=id&&x.ContractId == employeeUpdate.ContractId))
                {
                    throw new Exception("Hợp đồng không hợp lệ (do đã có người khác sử dụng)");
                }

                //check email is unique
                if (allEmployee.Any(x => x.Id != id && x.Email == employeeUpdate.Email))
                {
                    throw new Exception("Email đã được đăng kí bới tài khoản khác!");
                }

                //check username is unique
                if (allEmployee.Any(x => x.Id != id && x.UserName == employeeUpdate.UserName))
                {
                    throw new Exception("UserName đã được đăng kí bới tài khoản khác!");
                }
                if (employeeUpdate.Password.Trim() != "?????????")
                {
                    selectedEmployee.Password = BCrypt.Net.BCrypt.HashPassword(employeeUpdate.Password);
                }
                selectedEmployee.ContractId = employeeUpdate.ContractId;
                selectedEmployee.Email = employeeUpdate.Email;
                selectedEmployee.PhoneNumber = employeeUpdate.PhoneNumber;
                selectedEmployee.Avatar = employeeUpdate.Avatar ?? "user.png";

                using (var transaction = await _employeeRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _employeeRepository.Update(selectedEmployee);
                        await _employeeRepository.SaveChangeAsync();

                        var selectedTaxDeductionDetails = await _taxDeductionDetailsRepository.GetAllQueryAble().Where(x => x.EmployeeId == selectedEmployee.Id).ToListAsync();
                        _taxDeductionDetailsRepository.RemoveRange(selectedTaxDeductionDetails);

                        if (employeeUpdate.TaxDeductionIds != null && employeeUpdate.TaxDeductionIds.Count > 0)
                        {
                            var lstTaxDeductionDetails = new List<TaxDeductionDetails>();
                            foreach (var taxDeductionId in employeeUpdate.TaxDeductionIds)
                            {
                                lstTaxDeductionDetails.Add(new TaxDeductionDetails
                                {
                                    TaxDeductionId = taxDeductionId,
                                    EmployeeId = selectedEmployee.Id
                                });
                            }
                            await _taxDeductionDetailsRepository.AddRangeAsync(lstTaxDeductionDetails);
                        }

                        await _taxDeductionDetailsRepository.SaveChangeAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }
                }
                  
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = new List<string>() { ex.Message } };
            }
        }
        public async Task<ApiResponse<bool>> RemoveEmployee(int id)
        {
            try
            {
                using (var transaction = await _employeeRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _employeeRepository.RemoveAsync(id);
                        await _employeeRepository.SaveChangeAsync();

                        var selectedTaxDeductionDetails = await _taxDeductionDetailsRepository.GetAllQueryAble().Where(x => x.EmployeeId == id).ToListAsync();
                        _taxDeductionDetailsRepository.RemoveRange(selectedTaxDeductionDetails);
                        await _taxDeductionDetailsRepository.SaveChangeAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }
                }
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = new List<string>() { ex.Message } };
            }
        }
    }
}
