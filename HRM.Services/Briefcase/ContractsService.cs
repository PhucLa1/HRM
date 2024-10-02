using Aspose.Words;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using HRM.Repositories.Setting;
using HRM.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AsposeDocument = Aspose.Words.Document;
using Body = DocumentFormat.OpenXml.Wordprocessing.Body;
namespace HRM.Services.Briefcase
{
    public interface IContractsService
    {
        Task<ApiResponse<bool>> CreateNewContract(int applicantId, ContractAdd contractAdd);
        Task<ApiResponse<bool>> FillContractDetails(int id, ContractUpdate contractUpdate);
        Task<ApiResponse<bool>> SignContract();
    }
    public class ContractsService : IContractsService
    {
        private readonly IBaseRepository<Applicants> _applicantsRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<ContractAllowance> _contractAllowanceRepository;
        private readonly IEmailService _emailService;
        private readonly IValidator<ContractAdd> _contractAddValidator;
        private readonly IValidator<ContractUpdate> _contractUpdateValidator;
        private readonly CompanySetting _serverCompanySetting;
        private const string CONTRACT_NOTIFICATION = "Thông báo về việc tạo hợp đồng lao động .";
        private const string FOLER = "Email";
        private const string CONTRACT_NOTIFICATION_FILE = "ContractNotification.html";
        private const string CONTRACT_FOLDER = "Contract";
        private const string CONTRACT_TEMPLATE_WORD_FILE = "mau-hop-dong-lao-dong-chung.docx";
        public ContractsService(
            IBaseRepository<Applicants> applicantsRepository,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<ContractAllowance> contractAllowanceRepository,
            IEmailService emailService,
            IValidator<ContractAdd> contractAddValidator,
            IValidator<ContractUpdate> contractUpdateValidator,
            IOptions<CompanySetting> serverCompanySetting
            )
        {
            _contractRepository = contractRepository;
            _contractAllowanceRepository = contractAllowanceRepository;
            _emailService = emailService;
            _contractAddValidator = contractAddValidator;
            _contractUpdateValidator = contractUpdateValidator;
            _applicantsRepository = applicantsRepository;
            _serverCompanySetting = serverCompanySetting.Value;
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

                //Thêm mới hợp đồng với 1 vài trường cơ bản, sau khi đã bàn bạc với ứng viên

                using (var transaction = await _contractRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var contract = new Contract()
                        {
                            ContractSalaryId = contractAdd.ContractSalaryId,
                            ContractTypeId = contractAdd.ContractTypeId,
                            StartDate = contractAdd.StartDate,
                            EndDate = contractAdd.EndDate,
                            CompanySignStatus = CompanySignStatus.Signed,
                            TypeContract = contractAdd.TypeContract,
                            PositionId = contractAdd.PositionId,
                            //DepartmentId = contractAdd.DepartmentId,
                        };
                        await _contractRepository.AddAsync(contract);
                        await _contractRepository.SaveChangeAsync();

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
                var applicant = await _applicantsRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == applicantId)
                    .FirstAsync();

                var bodyContentEmail = HandleFile.READ_FILE(FOLER, CONTRACT_NOTIFICATION_FILE)
                    .Replace("{applicantName}", applicant.Name)
                    .Replace("{linkWebsite}", "https://www.youtube.com/watch?v=PRKYGpc44R0&list=RDMMmkRZ625Pvok&index=14")
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
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
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

        public Task<ApiResponse<bool>> SignContract()
        {
            throw new NotImplementedException();
        }
    }
}
