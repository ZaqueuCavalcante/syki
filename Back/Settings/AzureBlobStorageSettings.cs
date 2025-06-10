namespace Syki.Back.Settings;

public class AzureBlobStorageSettings
{
    public string ConnectionString { get; set; }

    public AzureBlobStorageSettings(IConfiguration configuration)
    {
        configuration.GetSection("AzureBlobStorage").Bind(this);
    }
}
