using H2.Application.DTOs;
using H2.Application.Interfaces.Messaging;

namespace H2.Application.Usecases
{
    public class UploadImageUsecase : IUploadImageUsecase
    {
        private readonly IMessageQueue _messageQueue;
        public UploadImageUsecase(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
        }
        public async Task ExecuteAsync(UploadImageDto dto, CancellationToken cancellationToken = default)
        {
            using var ms = new MemoryStream();
            await dto.FileStream.CopyToAsync(ms, cancellationToken);
            var base64 = Convert.ToBase64String(ms.ToArray());

            var message = new
            {
                deviceId = dto.DeviceId,
                fileName = dto.FileName,
                base64Image = base64,
                contentType = dto.ContentType,
                capturedAt = dto.CapturedAt
            };
            await _messageQueue.PublishAsync(
                queueName: "upload-to-s3",
                message: message,
                cancellationToken: cancellationToken
            );
        }
    }
}
