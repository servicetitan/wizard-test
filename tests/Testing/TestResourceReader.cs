using System.Resources;
using System.Text.Json;

namespace WizardTest.Testing;

public class TestResourceReader(Type testType)
{
    public Stream Read(string relativePath)
    {
        var manifestResourceStream =
            testType.Assembly.GetManifestResourceStream($"{testType.Namespace}.{relativePath}");
        if (manifestResourceStream != null) {
            return manifestResourceStream;
        }

        var resourceNames = testType.Assembly.GetManifestResourceNames();
        throw new MissingManifestResourceException($"{testType.FullName}," +
                                                   $" {relativePath}" +
                                                   "Found resources: " +
                                                   $"{string.Join(", ", resourceNames)}");
    }

    public async Task<string> ReadAsTextAsync(string relativePath)
    {
        await using var _ = Read(relativePath);
        using var sr = new StreamReader(_);
        return await sr.ReadToEndAsync();
    }

    public async Task<T> DeserializeAsJsonAsync<T>(string relativePath, bool caseInsensitive = true)
    {
        await using var stream = Read(relativePath);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var jsonElement = JsonSerializer.Deserialize<T>(
            ms.ToArray(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = caseInsensitive
            });
        return jsonElement;
    }
}
