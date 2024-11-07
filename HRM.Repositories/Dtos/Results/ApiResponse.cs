using FluentValidation.Results;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class ApiResponse<T>
    {
        public T? Metadata { get; set; }
        public List<string>? Message { get; set; }
        public bool IsSuccess { get; set; } = false;
        public static ApiResponse<T> FailtureValidation(IEnumerable<ValidationFailure> failures)
        {
            return new ApiResponse<T>
            {
                Message = failures.Select(f => f.ErrorMessage).ToList(),
            };
        }

		public static ApiResponse<bool> FailtureValidation(Applicants? applicant)
		{
			throw new NotImplementedException();
		}
	}
}
