using Syki.Back.Storage;

namespace Syki.Back.Features.Cross.CreatePreSignedUrlForUpload;

public class CreatePreSignedUrlForUploadService(IStorageService service) : ICrossService
{
    public async Task<OneOf<string, SykiError>> Create(Guid institutionId, Guid userId, CreatePreSignedUrlForUploadIn data)
    {
        var path = $"{data.Container.GetDescription()}/{institutionId}/{userId}/{data.FileName}";

        var url = await service.CreatePreSignedUrlForUpload(data.Container, path);

        return url;
    }
}
