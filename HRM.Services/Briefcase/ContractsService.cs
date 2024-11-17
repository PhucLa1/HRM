extern alias BouncyCastleOrg;
using BouncyCastleOrg::Org.BouncyCastle.Crypto;
using BouncyCastleOrg::Org.BouncyCastle.Pkcs;
using BouncyCastleOrg::Org.BouncyCastle.X509;

using Aspose.Words;
using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using HRM.Repositories.Setting;
using HRM.Services.User;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


using AsposeDocument = Aspose.Words.Document;
using Body = DocumentFormat.OpenXml.Wordprocessing.Body;
using Position = HRM.Data.Entities.Position;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace HRM.Services.Briefcase
{
    public interface IContractsService
    {
        Task<ApiResponse<IEnumerable<ContractResult>>> GetAllContract();
        Task<ApiResponse<bool>> AddNewContract(ContractUpsert contractAdd);
        Task<ApiResponse<bool>> CreateNewContract(int applicantId, ContractAdd contractAdd);
        Task<ApiResponse<bool>> RemoveContract(int id);
        Task<ApiResponse<bool>> FillContractDetails(int id, ContractUpdate contractUpdate);
        Task<ApiResponse<bool>> UpdateContract(int id, ContractUpsert contractUpdate);
        Task<ApiResponse<bool>> UpdateContractStatus(int id, ContractStatus status);
        Task<ApiResponse<bool>> SignContract(int contractId, DigitalSignature signatureModel);
    }
    public class ContractsService : IContractsService
    {
        private readonly IBaseRepository<Applicants> _applicantsRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Allowance> _allowanceRepository;
        private readonly IBaseRepository<Insurance> _insuranceRepository;
        private readonly IBaseRepository<ContractAllowance> _contractAllowanceRepository;
        private readonly IBaseRepository<ContractInsurance> _contractInsuranceRepository;
        private readonly IBaseRepository<ContractType> _contractTypeRepository;
        private readonly IBaseRepository<ContractSalary> _contractSalaryRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IEmailService _emailService;
        private readonly IValidator<ContractAdd> _contractAddValidator;
        private readonly IValidator<ContractUpdate> _contractUpdateValidator;
        private readonly IValidator<ContractUpsert> _contractUpsertValidator;
        private readonly CompanySetting _serverCompanySetting;
        private readonly IMapper _mapper;
        private const string CONTRACT_NOTIFICATION = "Thông báo về việc tạo hợp đồng lao động .";
        private const string FOLER = "Email";
        private const string CONTRACT_NOTIFICATION_FILE = "ContractNotification.html";
        private const string CONTRACT_FOLDER = "Contract";
        private const string CONTRACT_TEMPLATE_WORD_FILE = "mau-hop-dong-lao-dong-chung.docx";
        public ContractsService(
            IBaseRepository<Applicants> applicantsRepository,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<Allowance> allowanceRepository,
            IBaseRepository<Insurance> insuranceRepository,
            IBaseRepository<ContractAllowance> contractAllowanceRepository,
            IBaseRepository<ContractInsurance> contractInsuranceRepository,
            IBaseRepository<ContractType> contractTypeRepository,
            IBaseRepository<ContractSalary> contractSalaryRepository,
            IBaseRepository<Position> positionRepository,
            IBaseRepository<Department> departmentRepository,
            IEmailService emailService,
            IValidator<ContractAdd> contractAddValidator,
            IValidator<ContractUpdate> contractUpdateValidator,
            IValidator<ContractUpsert> contractUpsertValidator,
            IOptions<CompanySetting> serverCompanySetting,
            IMapper mapper
            )
        {
            _contractRepository = contractRepository;
            _allowanceRepository = allowanceRepository;
            _insuranceRepository = insuranceRepository;
            _contractAllowanceRepository = contractAllowanceRepository;
            _contractInsuranceRepository = contractInsuranceRepository;
            _contractTypeRepository = contractTypeRepository;
            _contractSalaryRepository = contractSalaryRepository;
            _positionRepository = positionRepository;
             _departmentRepository = departmentRepository;
            _emailService = emailService;
            _contractAddValidator = contractAddValidator;
            _contractUpdateValidator = contractUpdateValidator;
            _contractUpsertValidator = contractUpsertValidator;
            _applicantsRepository = applicantsRepository;
            _serverCompanySetting = serverCompanySetting.Value;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<ContractResult>>> GetAllContract()
        {
            try
            {
                var contracts = await (from c in _contractRepository.GetAllQueryAble()
                                    join p in _positionRepository.GetAllQueryAble() on c.PositionId equals p.Id
                                    join d in _departmentRepository.GetAllQueryAble() on p.DepartmentId equals d.Id
                                    join ct in _contractTypeRepository.GetAllQueryAble() on c.ContractTypeId equals ct.Id
                                    join cs in _contractSalaryRepository.GetAllQueryAble() on c.ContractSalaryId equals cs.Id
                                    select new ContractResult
                                    {
                                        Id = c.Id,
                                        ContractTypeId = c.ContractTypeId,
                                        DepartmentId = d.Id,
                                        PositionId = c.PositionId,
                                        ContractSalaryId = c.ContractSalaryId,
                                        BaseSalary = cs.BaseSalary,
                                        BaseInsurance = cs.BaseInsurance,
                                        RequiredDays = cs.RequiredDays,
                                        WageDaily = cs.WageDaily,
                                        RequiredHours = cs.RequiredHours,
                                        WageHourly = cs.WageHourly,
                                        Factor = cs.Factor,
                                        ContractTypeName = ct.Name,
                                        PositionName = p.Name,
                                        DepartmentName = d.Name,
                                        TypeContract = c.TypeContract,
                                        ContractStatus = c.ContractStatus,
                                        StartDate = c.StartDate,
                                        EndDate = c.EndDate,
                                        EmployeeSignStatus = c.EmployeeSignStatus,
                                        CompanySignStatus = c.CompanySignStatus,
                                        Name = c.Name,
                                        DateOfBirth = c.DateOfBirth,
                                        Gender = c.Gender,
                                        Address = c.Address,
                                        CountrySide = c.CountrySide,
                                        NationalID = c.NationalID,
                                        NationalStartDate = c.NationalStartDate,
                                        NationalAddress = c.NationalAddress,
                                        Level = c.Level,
                                        Major = c.Major,
                                        AllowanceResults = (from ca in _contractAllowanceRepository.GetAllQueryAble()
                                                            join a in _allowanceRepository.GetAllQueryAble() on ca.AllowanceId equals a.Id
                                                            where ca.ContractId == c.Id
                                                            select new AllowanceResult
                                                            {
                                                                Id = a.Id,
                                                                Name = a.Name,
                                                                Terms = a.Terms,
                                                                Amount = a.Amount,
                                                                ParameterName = a.ParameterName
                                                            }).ToList(),
                                        InsuranceResults = (from ci in _contractInsuranceRepository.GetAllQueryAble()
                                                            join insurance in _insuranceRepository.GetAllQueryAble() on ci.InsuranceId equals insurance.Id
                                                            where ci.ContractId == c.Id
                                                            select new InsuranceResult
                                                            {
                                                                Id = insurance.Id,
                                                                Name = insurance.Name,
                                                                PercentCompany = insurance.PercentCompany,
                                                                PercentEmployee = insurance.PercentEmployee
                                                            }).ToList()
                                    }).ToListAsync();
                return new ApiResponse<IEnumerable<ContractResult>>
                {

                    Metadata = contracts,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddNewContract(ContractUpsert contractAdd)
        {
            try
            {
                var resultValidation = _contractUpsertValidator.Validate(contractAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var newContract = new Contract
                {
                    ContractSalaryId = contractAdd.ContractSalaryId,
                    ContractTypeId = contractAdd.ContractTypeId,
                    StartDate = contractAdd.StartDate,
                    EndDate = contractAdd.EndDate,
                    CompanySignStatus = CompanySignStatus.NotSigned,
                    TypeContract = contractAdd.TypeContract,
                    PositionId = contractAdd.PositionId,
                    Name = contractAdd.Name,
                    DateOfBirth = contractAdd.DateOfBirth,
                    Gender = contractAdd.Gender,
                    Address = contractAdd.Address,
                    CountrySide = contractAdd.CountrySide,
                    NationalID = contractAdd.NationalID,
                    NationalStartDate = contractAdd.NationalStartDate,
                    NationalAddress = contractAdd.NationalAddress,
                    Level = contractAdd.Level,
                    Major = contractAdd.Major,
                };
                using (var transaction = await _contractRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _contractRepository.AddAsync(newContract);
                        await _contractRepository.SaveChangeAsync();

                        //Lưu các allowance
                        var contractAllowance = new List<ContractAllowance>();
                        if (contractAdd.AllowanceIds != null)
                        {
                            foreach (var allowanceId in contractAdd.AllowanceIds)
                            {
                                contractAllowance.Add(new ContractAllowance
                                {
                                    ContractId = newContract.Id,
                                    AllowanceId = allowanceId
                                });
                            }
                            await _contractAllowanceRepository.AddRangeAsync(contractAllowance);
                            await _contractAllowanceRepository.SaveChangeAsync();
                        }
                        //Lưu các insurance
                        var contractInsurances = new List<ContractInsurance>();
                        if (contractAdd.InsuranceIds != null)
                        {
                            foreach (var insuranceId in contractAdd.InsuranceIds)
                            {
                                contractInsurances.Add(new ContractInsurance
                                {
                                    ContractId = newContract.Id,
                                    InsuranceId = insuranceId
                                });
                            }
                            await _contractInsuranceRepository.AddRangeAsync(contractInsurances);
                            await _contractInsuranceRepository.SaveChangeAsync();
                        }
                        //Commit dữ liệu
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        //Dữ liệu bị lỗi thì rollback
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }                    
                }
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateNewContract(int applicantId, ContractAdd contractAdd)
        {
            //Tạo mới hợp đồng và gửi mail cho ứng cử viên
            try
            {
                //Check validate khi HR tạo mới hợp đồng
                var resultValidation = _contractAddValidator.Validate(contractAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var applicant = await _applicantsRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == applicantId)
                    .FirstAsync();

                //Thêm mới hợp đồng với 1 vài trường cơ bản, sau khi đã bàn bạc với ứng viên

                var id = 0;

                using (var transaction = await _contractRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var contract = new Contract()
                        {
                            Name = contractAdd.Name,
                            ContractSalaryId = contractAdd.ContractSalaryId,
                            ContractTypeId = contractAdd.ContractTypeId,
                            StartDate = contractAdd.StartDate,
                            EndDate = contractAdd.EndDate,
                            CompanySignStatus = CompanySignStatus.NotSigned,
                            EmployeeSignStatus = EmployeeSignStatus.NotSigned, // Chờ nhân viên kí
                            TypeContract = contractAdd.TypeContract, 
                            ContractStatus = ContractStatus.Pending // Chờ duyệt
                        };
                        // 0 Chờ duyệt. ContractStatus: Pending : đã xong giao diện, api
                        // 1 Manager duyệt. ContractStatus: Pending, CompanySignStatus =  CompanySignStatus.Signed,// Làm đến bước này
                        // 2 Employee kí. ContractStatus: Valid,  EmployeeSignStatus = EmployeeSignStatus.Signed,


                        // Thêm trường position vào
                        contract.PositionId = applicant.PositionId;

                        await _contractRepository.AddAsync(contract);
                        await _contractRepository.SaveChangeAsync();

                        id = contract.Id;
                        //Lưu các allowance
                        var contractAllowance = new List<ContractAllowance>();
                        if (contractAdd.AllowanceIds != null)
                        {
                            foreach (var allowanceId in contractAdd.AllowanceIds)
                            {
                                contractAllowance.Add(new ContractAllowance
                                {
                                    ContractId = contract.Id,
                                    AllowanceId = allowanceId
                                });
                            }
                            await _contractAllowanceRepository.AddRangeAsync(contractAllowance);
                            await _contractAllowanceRepository.SaveChangeAsync();
                        }

                        //Lưu các insurance
                        var contractInsurance = new List<ContractInsurance>();
                        if (contractAdd.InsuranceIds != null)
                        {
                            foreach (var insuranceId in contractAdd.InsuranceIds)
                            {
                                contractInsurance.Add(new ContractInsurance
                                {
                                    ContractId = contract.Id,
                                    InsuranceId = insuranceId
                                });
                            }
                            await _contractInsuranceRepository.AddRangeAsync(contractInsurance);
                            await _contractInsuranceRepository.SaveChangeAsync();
                        }

                        //Commit dữ liệu
                        await transaction.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        //Dữ liệu bị lỗi thì rollback
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }

                }


                /*Sau đó gửi mail cho ứng cử viên về việc hợp đồng đã được tạo xong
                 và trong mail sẽ redirect đến 1 trang để điền thông tin hợp đồng
                */

                //linkWebsite: HttpClient:/localhoset... employee - shared / emnployee - information / id
                if(id != 0)
                {
                    var bodyContentEmail = HandleFile.READ_FILE(FOLER, CONTRACT_NOTIFICATION_FILE)
                        .Replace("{applicantName}", applicant.Name)
                        .Replace("{linkWebsite}", "http://localhost:3000/employee-shared/employee-information/" + id)
                        .Replace("{companyName}", _serverCompanySetting.CompanyName);

                    var bodyEmail = _emailService.TemplateContent
                        .Replace("{content}", bodyContentEmail);

                    var email = new Email()
                    {
                        To = applicant.Email,
                        Body = bodyEmail,
                        Subject = CONTRACT_NOTIFICATION
                    };
                    await _emailService.SendEmailToRecipient(email);                   
                }
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateContract(int id, ContractUpsert contractUpdate)
        {
            try
            {
                var resultValidation = _contractUpsertValidator.Validate(contractUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                // Retrieve the existing contract
                var contract = await _contractRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == id)
                    .FirstOrDefaultAsync();

                if (contract == null)
                {
                    throw new Exception("Contract not found.");
                }

                // Update the contract properties
                contract.Name = contractUpdate.Name;
                contract.DateOfBirth = contractUpdate.DateOfBirth;
                contract.Gender = contractUpdate.Gender;
                contract.Address = contractUpdate.Address;
                contract.CountrySide = contractUpdate.CountrySide;
                contract.NationalID = contractUpdate.NationalID;
                contract.NationalStartDate = contractUpdate.NationalStartDate;
                contract.NationalAddress = contractUpdate.NationalAddress;
                contract.Level = contractUpdate.Level;
                contract.Major = contractUpdate.Major;
                contract.ContractSalaryId = contractUpdate.ContractSalaryId;
                contract.ContractTypeId = contractUpdate.ContractTypeId;
                contract.StartDate = contractUpdate.StartDate;
                contract.EndDate = contractUpdate.EndDate;
                contract.TypeContract = contractUpdate.TypeContract;
                contract.PositionId = contractUpdate.PositionId;

                using (var transaction = await _contractRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Update allowances
                        var listAllowanceRemove = _contractAllowanceRepository.GetAllQueryAble().Where(a => a.ContractId == id).ToList();
                        _contractAllowanceRepository.RemoveRange(listAllowanceRemove);
                        var contractAllowances = new List<ContractAllowance>();
                        if (contractUpdate.AllowanceIds != null)
                        {
                            foreach (var allowanceId in contractUpdate.AllowanceIds)
                            {
                                contractAllowances.Add(new ContractAllowance
                                {
                                    ContractId = contract.Id,
                                    AllowanceId = allowanceId
                                });
                            }
                            await _contractAllowanceRepository.AddRangeAsync(contractAllowances);
                        }

                        // Update insurances
                        var listInsuranceRemove = _contractInsuranceRepository.GetAllQueryAble().Where(a => a.ContractId == id).ToList();
                        _contractInsuranceRepository.RemoveRange(listInsuranceRemove);
                        var contractInsurances = new List<ContractInsurance>();
                        if (contractUpdate.InsuranceIds != null)
                        {
                            foreach (var insuranceId in contractUpdate.InsuranceIds)
                            {
                                contractInsurances.Add(new ContractInsurance
                                {
                                    ContractId = contract.Id,
                                    InsuranceId = insuranceId
                                });
                            }
                            await _contractInsuranceRepository.AddRangeAsync(contractInsurances);
                        }
                        _contractRepository.Update(contract);
                        await _contractAllowanceRepository.SaveChangeAsync();
                        await _contractInsuranceRepository.SaveChangeAsync();
                        await _contractRepository.SaveChangeAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Transaction failed: " + ex.Message);
                    }
                }

                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception("Update failed: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateContractStatus(int id, ContractStatus status)
        {
            try
            {
                var selectedContract = await _contractRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                if (selectedContract == null)
                {
                    return new ApiResponse<bool> { IsSuccess = false, Message = new List<string>() { "Không tìm thấy " } };

                }
                selectedContract.ContractStatus = status;
                _contractRepository.Update(selectedContract);
                await _contractRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message }
                };
            }
        }


        public async Task<ApiResponse<bool>> FillContractDetails(int id, ContractUpdate contractUpdate)
        {
            try
            {
                //Check validate khi ứng cử viên điền thông tin 
                var resultValidation = _contractUpdateValidator.Validate(contractUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                //Update thông tin về hợp đồng sau khi ứng viên điền
                var contract = await _contractRepository
                    .GetAllQueryAble()
                    .Include(e => e.ContractAllowances)
                    ///.Include(e => e.Department)
                    .Include(e => e.Position)
                    .Include(e => e.ContractSalary)
                    .Where(e => e.Id == id)
                    .FirstAsync();

                //Sửa thông tin
                contract.Name = contractUpdate.Name;
                contract.DateOfBirth = contractUpdate.DateOfBirth;
                contract.Gender = contractUpdate.Gender;
                contract.Address = contractUpdate.Address;
                contract.CountrySide = contractUpdate.CountrySide;
                contract.NationalID = contractUpdate.NationalID;
                contract.NationalStartDate = contractUpdate.NationalStartDate;
                contract.NationalAddress = contractUpdate.NationalAddress;
                contract.Level = contractUpdate.Level;
                contract.Major = contractUpdate.Major;

                //Lưu thông tin
                _contractRepository.Update(contract);


                //Tạo mới file PDF hợp đồng và trả về URL cho ứng cử viên xem lại
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", CONTRACT_FOLDER, CONTRACT_TEMPLATE_WORD_FILE);
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", CONTRACT_FOLDER, $"{contractUpdate.Name}-hop-dong-so-{id}.docx");

                //Tạo bản sao mới của file
                File.Copy(templatePath, outputPath, true);

                //Mở file và ghi file
                try
                {
                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputPath, true))
                    {
                        if (wordDoc.MainDocumentPart != null)
                        {
                            var body = wordDoc.MainDocumentPart.Document.Body;
                            if (body != null)
                            {
                                //Tạo dictionary
                                var replacements = new Dictionary<string, string>
                                {
                                    { "{{DAY}}", ConvertDayOfWeekFromEngToVn(DateTime.Now.DayOfWeek.ToString()) },
                                    { "{{DATE}}", DateTime.Now.Day.ToString() },
                                    { "{{MONTH}}", DateTime.Now.Month.ToString() },
                                    { "{{YEAR}}", DateTime.Now.Year.ToString() },
                                    { "{{CONTRACT_ID}}", id.ToString() },
                                    { "{{COMPANY_NAME}}", _serverCompanySetting.CompanyName },
                                    { "{{COMPANY_LOCATION}}", _serverCompanySetting.CompanyLocation },
                                    { "{{COMPANY_PHONENUMBER}}", _serverCompanySetting.CompanyPhonenumber },
                                    { "{{COMPANY_TAX_NUMBER}}", _serverCompanySetting.CompanyTax },
                                    { "{{COMPANY_BANK_ACCOUNT}}", _serverCompanySetting.CompanyBankAccount },
                                    { "{{COMPANY_BANK}}", _serverCompanySetting.CompanyBank },
                                    { "{{APPLICANT_FULL_NAME}}", contractUpdate.Name },
                                    { "{{APPLICANT_DOB}}", contractUpdate.DateOfBirth.ToString("dd/MM/yyyy") },
                                    { "{{APPLICANT_SEX}}", contractUpdate.Gender == true ? "Nam" : "Nữ" },
                                    { "{{APPLICANT_COUNTRYSIDE}}", contractUpdate.CountrySide },
                                    { "{{APPLICANT_ADDRESS}}", contractUpdate.Address },
                                    { "{{APPLICANT_NATIONALID}}", contractUpdate.NationalID },
                                    { "{{APPLICANT_START_DATE}}", contractUpdate.NationalStartDate.ToString("dd/MM/yyyy") },
                                    { "{{APPLICANT_NATIONAL_ADDRESS}}", contractUpdate.NationalAddress },
                                    { "{{APPLICANT_LEVEL}}", contractUpdate.Level },
                                    { "{{APPLICANT_MAJOR}}", contractUpdate.Major },
                                    { "{{CONTRACT_TYPE}}", contract.TypeContract.ToString() },
                                    { "{{CONTRACT_TIME}}", CalculateDifferenceInYearsOrMonths(contract.StartDate,contract.EndDate) },
                                    { "{{CONTRACT_START_DATE}}", contract.StartDate.ToString("dd/MM/yyyy") },
                                    { "{{CONTRACT_END_DATE}}", contract.EndDate.ToString("dd/MM/yyyy") },
                                   // { "{{EMPLOYEE_DEPARTMENT}}", contract.Department.Name },
                                    { "{{EMPLOYEE_POSITION}}", contract.Position.Name },
                                    { "{{COMPANY_CEO}}", _serverCompanySetting.CEO },
                                    { "{{CONTRACT_WORK_TIME}}", contract.ContractSalary.RequiredHours.ToString() },
                                    { "{{CONTRACT_TYPE_SALARY}}",contract.ContractSalary.BaseSalary.ToString() },
                                    { "{{ALLOWANCE}}", "" },
                                    { "{{COMPANY_SIGNATURE}}", DateTime.Now.Date.ToString() },
                                };

                                Console.WriteLine($"{_serverCompanySetting.CompanyTax}  {contractUpdate.Name}");
                                // Thay thế các placeholder với thông tin thực tế
                                foreach (var replacement in replacements)
                                {
                                    ReplacePlaceholder(body, replacement.Key, replacement.Value);
                                }

                            }
                            wordDoc.MainDocumentPart.Document.Save();
                        }
                    }

                }
                catch (FileFormatException ex)
                {
                    Console.WriteLine($"File format exception: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"I/O exception: {ex.Message}");
                }

                //Chuyển file word thành file PDF
                AsposeDocument doc = new AsposeDocument(outputPath);
                string pdfOutputPath = Path.ChangeExtension(outputPath, ".pdf");
                doc.Save(pdfOutputPath, SaveFormat.Pdf);

                //Xóa bản word cũ
                HandleFile.DELETE_FILE(FOLER, outputPath);
                contract.FireUrlBase = $"{contractUpdate.Name}-hop-dong-so-{id}.pdf";
                contract.FileUrlSigned = $"{contractUpdate.Name}-hop-dong-so-{id}_signed.pdf";
                await _contractRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        private void ReplacePlaceholder(Body body, string placeholder, string value)
        {
            foreach (var text in body.Descendants<Text>())
            {
                if (text.Text.Trim().Contains(placeholder.Trim()))
                {
                    text.Text = text.Text.Replace(placeholder.Trim(), value);
                }
            }
        }
        
        private string CalculateDifferenceInYearsOrMonths(DateTime startDate, DateTime endDate)
        {
            var difference = endDate - startDate;
            double totalDays = difference.TotalDays;
            double totalYears = totalDays / 365.25;
            if (totalYears >= 1)
            {
                return $"{Math.Floor(totalYears)} năm";
            }
            else
            {
                double totalMonths = totalDays / 30.44;
                return $"{Math.Floor(totalMonths)} tháng";
            }
        }
        
        private string ConvertDayOfWeekFromEngToVn(string dayOfWeek)
        {
            var dayOfWeeks = new Dictionary<string, string>()
            {
                { "Monday", "Thứ Hai" },
                { "Tuesday", "Thứ Ba" },
                { "Wednesday", "Thứ Tư" },
                { "Thursday", "Thứ Năm" },
                { "Friday", "Thứ Sáu" },
                { "Saturday", "Thứ Bảy" },
                { "Sunday", "Chủ Nhật" }
            };
            if (dayOfWeeks.TryGetValue(dayOfWeek, out string vietnameseDay))
            {
                return vietnameseDay;
            }
            else
            {
                throw new ArgumentException("Invalid day of the week: " + dayOfWeek);
            }
        }

        public async Task<ApiResponse<bool>> RemoveContract(int id)
        {
            try
            {
                await _contractRepository.RemoveAsync(id);
                await _contractRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> SignContract(int contractId,DigitalSignature signatureModel)
        {
            try
            {
                // Kiểm tra xem file có hợp lệ không
                if (signatureModel.CertificateFile == null || signatureModel.CertificateFile.Length == 0)
                {
                    throw new Exception("File chứng thư không được trống!");
                }

                var selectedContract = await _contractRepository.GetAllQueryAble().FirstOrDefaultAsync(x => x.Id == contractId);
                if (selectedContract == null) throw new Exception("Không tìm thấy hợp đồng tương ứng!");

                // Step 2: Load PDF Document
                var contractFileName = $"{selectedContract.Name}-hop-dong-so-{selectedContract.Id}";
                var fileURLRaw = $"{Directory.GetCurrentDirectory()}/wwwroot/Contract/{contractFileName}";
                string pdfFilePath = $"{fileURLRaw}.pdf";
                PdfReader pdfReader = new PdfReader(pdfFilePath);

                // Step 3: Load PFX Certificate
                var cerFile = signatureModel.CertificateFile.OpenReadStream();
                var pfxKeyStore = new Pkcs12Store(cerFile, signatureModel.Password.ToCharArray());

                // Step 4: Initialize the PDF Stamper And Creating the Signature Appearance
                Stream fileStream = new FileStream($"{fileURLRaw}_signed.pdf", FileMode.Create);
                PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, fileStream, '\0', null, true);

                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                signatureAppearance.SignDate = DateTime.Now;
                signatureAppearance.Reason = signatureModel.Reason;
               // signatureAppearance.Location = signatureModel.Location;

                if (signatureModel.SignatureImageFile != null && signatureModel.SignatureImageFile.Length != 0)
                {
                    signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
                    signatureAppearance.SignatureGraphic = Image.GetInstance(signatureModel.SignatureImageFile.OpenReadStream());
                }

                // Set the signature appearance location (in points)
                float x = 360;
                float y = 530;
                float width = 200; // Width of the signature rectangle
                float height = 100; // Height of the signature rectangle
                int page = 5;
                signatureAppearance.Acro6Layers = false;
                //signatureAppearance.Layer4Text = PdfSignatureAppearance.questionMark;
                signatureAppearance.Layer4Text = "Signature valid";

                signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + width, y + height), page, "signature");

                // Step 5: Sign the Document
                string alias = pfxKeyStore.Aliases.Cast<string>().FirstOrDefault(entryAlias => pfxKeyStore.IsKeyEntry(entryAlias));

                if (alias != null)
                {
                    ICipherParameters privateKey = pfxKeyStore.GetKey(alias).Key;
                    IExternalSignature pks = new PrivateKeySignature(privateKey, DigestAlgorithms.SHA256);
                    MakeSignature.SignDetached(signatureAppearance, pks, new X509Certificate[] { pfxKeyStore.GetCertificate(alias).Certificate }, null, null, null, 0, CryptoStandard.CMS);
                }
                else
                {
                    throw new Exception("Private key not found in the PFX certificate.");
                }

                // Step 6: Save the Signed PDF
                pdfStamper.Close();

                selectedContract.FileUrlSigned = contractFileName + "_signed.pdf";
                _contractRepository.Update(selectedContract);
                await _contractRepository.SaveChangeAsync();

                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new ApiResponse<bool>() { IsSuccess = false, Message = new List<string>() { e.Message } };
            }
        }

    }
}
