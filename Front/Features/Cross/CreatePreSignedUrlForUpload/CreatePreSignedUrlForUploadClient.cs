namespace Syki.Front.Features.Cross.CreatePreSignedUrlForUpload;

public class CreatePreSignedUrlForUploadClient(HttpClient http) : ICrossClient
{
    public async Task<OneOf<CreatePreSignedUrlForUploadOut, ErrorOut>> Create(StorageContainer container, string fileName)
    {
        var data = new CreatePreSignedUrlForUploadIn { Container = container, FileName = fileName };

        var response = await http.PutAsJsonAsync("/files/pre-signed-url", data);

        return await response.Resolve<CreatePreSignedUrlForUploadOut>();
    }
}
