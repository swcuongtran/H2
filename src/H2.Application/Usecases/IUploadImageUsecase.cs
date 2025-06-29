using H2.Application.DTOs;

namespace H2.Application.Usecases
{
    public interface IUploadImageUsecase
    {
        Task ExecuteAsync(UploadImageDto dto, CancellationToken cancellationToken = default);
    }
}