using System.Text.Json;
using System.Text.Json.Serialization;

namespace Task_Manager.Settings;

public class SettingsStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string filePath;

    public SettingsStorage(string filePath)
    {
        this.filePath = filePath;
    }

    public AppSettings Load()
    {
        try
        {
            if (File.Exists(filePath))
                return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(filePath), Options) ?? new AppSettings();
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException)
        {
            // испорченный файл настроек - просто запускаемся со стандартными
        }

        return new AppSettings();
    }

    public void Save(AppSettings settings)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(settings, Options));
    }
}
