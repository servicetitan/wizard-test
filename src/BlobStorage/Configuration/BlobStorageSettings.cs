using ServiceTitan.Platform.BlobStorage.AzureSdk;

namespace WizardTest.BlobStorage;

public class BlobStorageSettings
{
    public string? ContainerName { get; set; }
    public string? ConnectionString { get; set; }
    public bool? SkipContainerExistenceCheck { get; set; }
    public bool? RetryOperationOnContainerAbsence { get; set; }
    public string TemporaryDirectory { get; set; } = "Temp";

    public static implicit operator AzureSdkBlobStorageSettings(BlobStorageSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings.ContainerName);
        ArgumentNullException.ThrowIfNull(settings.ConnectionString);
        ArgumentNullException.ThrowIfNull(settings.TemporaryDirectory);

        return new AzureSdkBlobStorageSettings(settings.ContainerName, settings.ConnectionString,
            settings.TemporaryDirectory) {
            SkipContainerExistenceCheck = settings.SkipContainerExistenceCheck ?? true,
            RetryOperationOnContainerAbsence = settings.RetryOperationOnContainerAbsence ?? true
        };
    }
}
