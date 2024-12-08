namespace ServiceBusTwin.Extensions;

internal static class ConfigurationFileExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        WriteIndented       = true
    };

    public static void SaveChanges(this ConfigurationFile file, out string absolutePath)
    {
        var jsonConfig = JsonSerializer.Serialize(file, SerializerOptions);

        absolutePath = Path.Combine(Environment.CurrentDirectory, "config.json");
        File.WriteAllText(absolutePath, jsonConfig);
    }
}