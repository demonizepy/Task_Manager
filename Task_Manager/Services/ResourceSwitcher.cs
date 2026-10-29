using System.Windows;

namespace Task_Manager.Services;

internal static class ResourceSwitcher
{
    // меняет один из словарей в App.xaml (тему или язык) на другой файл из той же папки
    public static void Replace(string folder, string fileName)
    {
        var dictionaries = Application.Current.Resources.MergedDictionaries;
        var newDictionary = new ResourceDictionary
        {
            Source = new Uri($"/{folder}/{fileName}.xaml", UriKind.Relative)
        };

        for (int i = 0; i < dictionaries.Count; i++)
        {
            string? source = dictionaries[i].Source?.OriginalString;

            if (source != null && source.StartsWith($"/{folder}/"))
            {
                dictionaries[i] = newDictionary;
                return;
            }
        }

        dictionaries.Insert(0, newDictionary);
    }
}
