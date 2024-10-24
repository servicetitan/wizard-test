using ServiceTitan.Platform.BlobStorage;
using ServiceTitan.Platform.BlobStorage.AzureSdk;
using WizardTest.Core;

namespace WizardTest.BlobStorage;

internal class CheckBlobStoragePlanStep(IAzureSdkBlobStorage blobStorage) : IPlanStep
{
    public async Task ValidateAsync()
    {
        var tempName = blobStorage.GetTemporaryName();
        await blobStorage.UploadAsync(tempName, new MemoryStream("Startup probe"u8.ToArray()));
        await blobStorage.RemoveBlobAsync(tempName);
    }
}
