using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class FaceRegisUpdate : FaceRegis
    {
        public int Id { get; set; }
    }

    public class FaceRegisUpdateValidator : AbstractValidator<FaceRegisUpdate>
    {
        public FaceRegisUpdateValidator()
        {
            RuleFor(x => x.FaceFile)
                .NotEmpty()
                .WithMessage("Không được bỏ trống ảnh. ");
            RuleFor(x => x.StatusFaceTurn)
                .NotEmpty()
                .WithMessage("Không được để trống trạng thái mặt .")
                .IsInEnum()
                .WithMessage("Trạng thái mặt truyền vào không đúng .");
            RuleFor(x => x.Descriptor)
                .NotEmpty()
                .WithMessage("Không được để trống mô tả của ảnh");
            /*
                .Must(BeValidFloat32Array)
                .WithMessage("Mô tả của ảnh không hợp lệ."); */
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Không được để trống id");
        }
        private bool BeValidFloat32Array(string? descriptor)
        {
            if (string.IsNullOrWhiteSpace(descriptor)) return false;

            try
            {
                var parsedArray = System.Text.Json.JsonSerializer.Deserialize<float[]>(descriptor);
                return parsedArray != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
