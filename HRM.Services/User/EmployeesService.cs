using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using HRM.Repositories.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Internal;

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
        Task<ApiResponse<IEnumerable<EmployeeResult>>> GetAllEmployee();
    }
    public class EmployeesService : IEmployeesService
    {
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<EmployeeImage> _employeeImageRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        
        private readonly IValidator<FaceRegis> _faceRegisValidator;
        private readonly CompanySetting _serverCompanySetting;
        private const string FOLDER_EMPLOYEE_IMAGE = "Employee";

        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<TaxDeductionDetails> _taxDeductionDetailsRepository;
        public EmployeesService(
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<EmployeeImage> employeeImageRepository,
            IValidator<FaceRegis> faceRegisValidator,
            HttpClient httpClient,
            IBaseRepository<Contract> contractRepository,
            IOptions<CompanySetting> serverCompanySetting,
            IBaseRepository<Department> departmentRepository,
            IBaseRepository<Position> positionRepository,
            IBaseRepository<TaxDeductionDetails> taxDeductionDetailsRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeImageRepository = employeeImageRepository;
            _faceRegisValidator = faceRegisValidator;
            _contractRepository = contractRepository;
            _serverCompanySetting = serverCompanySetting.Value;

            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _taxDeductionDetailsRepository = taxDeductionDetailsRepository;
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
                for(int i = 0; i < listIds.Count; i++) 
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
            }catch (Exception ex)
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
                
                return new ApiResponse<IEnumerable<EmployeeResult>>
                {

                    Metadata = await _employeeRepository.GetAllQueryAble().Select(e => new EmployeeResult
                    {
                        Id = e.Id,
                        Name = e.Contract.Name,
                        DateOfBirth = e.Contract.DateOfBirth,
                        Age = DateTime.Now.Year - e.Contract.DateOfBirth.Year
                                 - (DateTime.Now.DayOfYear < e.Contract.DateOfBirth.DayOfYear ? 1 : 0), // Tính tuổi
                        Gender = e.Contract.Gender,
                        Tenure = DateTime.Now.Year - e.Contract.StartDate.Year
                         - (DateTime.Now.DayOfYear < e.Contract.StartDate.DayOfYear ? 1 : 0), // Tính thâm niên
                        Address = e.Contract.Address,
                        CountrySide = e.Contract.CountrySide,
                        NationalID = e.Contract.NationalID,
                        NationalStartDate = e.Contract.NationalStartDate,
                        NationalAddress = e.Contract.NationalAddress,
                        Level = e.Contract.Level,
                        Major = e.Contract.Major,
                        PositionName = e.Contract.Position.Name,
                        PositionId = e.Contract.PositionId,
                        PhoneNumber = e.PhoneNumber,
                        Email = e.Email
                    }).ToListAsync(),
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
