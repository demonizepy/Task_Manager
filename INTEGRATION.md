# Интеграция модуля Task_Manager.Core

В этом документе показано, как устроен модуль с логикой менеджера задач, как он подключён к WPF-приложению и как тот же модуль используется в другом приложении без изменений.

---

## 1. Что такое модуль и зачем он отдельный

Решение состоит из трёх проектов:

| Проект | Тип | Зависит от | Что внутри |
|---|---|---|---|
| `Task_Manager.Core` | библиотека классов (`.dll`) | ни от чего | задачи, список задач, работа с файлом |
| `Task_Manager` | WPF-приложение (`.exe`) | `Task_Manager.Core` | окна, стили, темы, языки |
| `Task_Manager.Demo` | консольное приложение (`.exe`) | `Task_Manager.Core` | сценарии работы с модулем без окон |

`Task_Manager.Core` ничего не знает про окна: в нём нет ни одного `using System.Windows`, нет `MessageBox`, нет текста для пользователя. Модуль только хранит задачи, проверяет данные и сохраняет их в файл. Всё, что видит пользователь, находится в `Task_Manager`.

Что это даёт:

* **Переиспользование.** Один и тот же модуль работает и в WPF-приложении, и в консольном демо. Можно сделать третье приложение (например, веб), не трогая логику.
* **Проверка без интерфейса.** Логику можно запустить и проверить из консоли, не открывая окна и не нажимая кнопки.
* **Независимые изменения.** Переход с Windows Forms на WPF не потребовал менять ни строчки в логике: изменились только окна.

Модуль физически не может зависеть от WPF: в его `.csproj` указан `net8.0`, а не `net8.0-windows`, и нет `<UseWPF>`. Если написать в нём `using System.Windows;`, проект просто не соберётся.

---

## 2. Что модуль даёт наружу

Все классы модуля публичные (`public`), поэтому их видят проекты, которые на него ссылаются.

**`Task_Manager.Core.Models.TaskItem`** — одна задача.

| Член | Назначение |
|---|---|
| `int Id` | номер задачи, задаётся при создании |
| `string Description` | описание |

**`Task_Manager.Core.Models.RemoveResult`** — итог удаления по списку ID.

| Член | Назначение |
|---|---|
| `List<int> Removed` | какие задачи удалены |
| `List<int> NotFound` | каких ID не было в списке |
| `List<string> Invalid` | что введено не числом |
| `bool IsEmpty` | пользователь ничего не ввёл |

**`Task_Manager.Core.Services.TaskStorage`** — чтение и запись файла `id|описание`.

| Член | Назначение |
|---|---|
| `TaskStorage(string filePath)` | путь к файлу передаётся снаружи |
| `List<TaskItem> Load()` | прочитать задачи, пропуская испорченные строки |
| `void Save(IEnumerable<TaskItem>)` | перезаписать файл |

**`Task_Manager.Core.Services.TaskManager`** — главный класс модуля, с ним работают окна.

| Член | Назначение |
|---|---|
| `TaskManager(TaskStorage storage)` | хранилище передаётся снаружи |
| `int Count` | сколько задач |
| `List<TaskItem> GetAll()` | все задачи по возрастанию ID |
| `bool Contains(int id)` | занят ли ID |
| `int GetNextId()` | следующий свободный ID |
| `bool Add(int id, string description)` | добавить или заменить; `true`, если задача новая |
| `bool Remove(int id)` | удалить одну задачу |
| `RemoveResult RemoveByIds(string input)` | удалить по строке вида `1, 2, 5` |
| `void Clear()` | очистить список |
| `void Load()` / `void Save()` | загрузить из файла / сохранить в файл |

Обратите внимание: `TaskManager` не создаёт `TaskStorage` сам, а получает его в конструкторе. Поэтому путь к файлу выбирает приложение: WPF-версия хранит задачи рядом с exe, а демо — во временной папке, чтобы не испортить настоящие данные.

---

## 3. Как модуль подключён к приложению

### Шаг 1. Ссылка на проект

В файле `Task_Manager/Task_Manager.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="..\Task_Manager.Core\Task_Manager.Core.csproj" />
</ItemGroup>
```

Эта строка говорит сборщику: перед сборкой приложения собрать модуль, а получившийся `Task_Manager.Core.dll` положить рядом с `Task_Manager.exe`.

В Visual Studio то же самое делается мышкой: правый клик по проекту `Task_Manager` → «Добавить» → «Ссылка на проект…» → отметить `Task_Manager.Core`. В обозревателе решения ссылка видна в разделе «Зависимости» → «Проекты».

### Шаг 2. Подключение пространств имён

В файлах, которые пользуются модулем:

```csharp
using Task_Manager.Core.Models;
using Task_Manager.Core.Services;
```

### Шаг 3. Точка сборки — `App.xaml.cs`

Объекты модуля создаются в одном месте, при запуске программы:

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    var settingsStorage = new SettingsStorage(Path.Combine(AppContext.BaseDirectory, "settings.json"));
    AppSettings settings = settingsStorage.Load();

    ThemeManager.Init();
    ThemeManager.Apply(settings.Theme);
    LanguageManager.Apply(settings.Language);

    // здесь модуль Task_Manager.Core подключается к окнам
    string path = Path.Combine(AppContext.BaseDirectory, "tasks.txt");
    var manager = new TaskManager(new TaskStorage(path));

    var window = new MainWindow(manager, settingsStorage, settings);
    MainWindow = window;
    window.Show();
}
```

Здесь приложение решает, где лежит файл, создаёт хранилище, передаёт его менеджеру, а менеджер — главному окну.

### Шаг 4. Передача в окно через конструктор

```csharp
public partial class MainWindow : Window
{
    private readonly TaskManager manager;

    public MainWindow(TaskManager manager, SettingsStorage settingsStorage, AppSettings settings)
    {
        InitializeComponent();
        this.manager = manager;
        ...
    }
}
```

Окно не создаёт менеджер само, а получает готовый. Окно знает, *что* умеет менеджер, но не знает, *откуда* он берёт данные.

---

## 4. Примеры вызова модуля из окон

### Пример 1. Загрузка при открытии окна

```csharp
private void Window_Loaded(object sender, RoutedEventArgs e)
{
    try
    {
        manager.Load();
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
        Dialogs.Error(this, LanguageManager.Get("Msg_LoadError") + Environment.NewLine + ex.Message);
    }

    RefreshList();
}

private void RefreshList()
{
    gridTasks.ItemsSource = manager.GetAll();
    txtCount.Text = LanguageManager.Format("Status_Count", manager.Count);
}
```

Модуль выбрасывает исключение, если файл нельзя прочитать, а окно решает, как об этом сообщить. Модуль не показывает окна сам: если бы он это делал, его нельзя было бы использовать в консоли.

`GetAll()` возвращает обычный `List<TaskItem>`, а таблица `DataGrid` показывает его через привязки `{Binding Id}` и `{Binding Description}` в `MainWindow.xaml`.

### Пример 2. Добавление задачи через отдельное окно

```csharp
private void btnAdd_Click(object sender, RoutedEventArgs e)
{
    var window = new AddTaskWindow(manager.GetNextId(), manager.Contains) { Owner = this };

    if (window.ShowDialog() != true)
        return;

    manager.Add(window.TaskId, window.TaskDescription);
    SaveTasks();
    RefreshList();
}
```

Окну `AddTaskWindow` не передаётся весь менеджер. Оно получает только две вещи:

* `manager.GetNextId()` — число, которое нужно подставить в поле ID;
* `manager.Contains` — метод как значение типа `Func<int, bool>`, чтобы окно могло спросить «такой ID уже есть?» и предложить замену.

Так окно добавления не может случайно удалить задачи или очистить список: у него просто нет к этому доступа.

### Пример 3. Удаление по ID и разбор результата

```csharp
RemoveResult result = manager.RemoveByIds(window.IdsText);

if (result.Removed.Count > 0)
{
    SaveTasks();
    RefreshList();
}

if (result.NotFound.Count > 0)
    lines.Add(LanguageManager.Format("Msg_NotFound", string.Join(", ", result.NotFound)));
```

Модуль возвращает не готовый текст, а данные: списки удалённых, ненайденных и неверных ID. Поэтому одно и то же значение показывается на русском или английском в зависимости от выбранного языка, а в демо выводится в консоль.

### Пример 4. Сохранение

```csharp
private void SaveTasks()
{
    try
    {
        manager.Save();
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
        Dialogs.Error(this, LanguageManager.Get("Msg_SaveError") + Environment.NewLine + ex.Message);
    }
}
```

---

## 5. Тот же модуль в другом приложении — `Task_Manager.Demo`

Консольный проект подключает модуль так же, одной строкой в `.csproj`:

```xml
<ProjectReference Include="..\Task_Manager.Core\Task_Manager.Core.csproj" />
```

и вызывает те же методы, но без окон:

```csharp
static void RemoveByIds()
{
    Title("4. Удаление по списку ID: \"2, 7, abc\"");

    TaskManager manager = CreateManager();
    manager.Load();

    RemoveResult result = manager.RemoveByIds("2, 7, abc");
    manager.Save();

    checker.Check("удалена задача 2", result.Removed.SequenceEqual(new[] { 2 }));
    checker.Check("задача 7 не найдена", result.NotFound.SequenceEqual(new[] { 7 }));
    checker.Check("abc распознан как неверный ID", result.Invalid.SequenceEqual(new[] { "abc" }));
    checker.Check("в файле осталось 2 строки", File.ReadAllLines(FilePath).Length == 2);
}
```

Демо проходит семь сценариев. В каждом создаётся новый `TaskManager`, работающий с одним и тем же временным файлом, поэтому проверяется не только логика, но и то, что данные действительно доходят до файла и читаются обратно.

Как запустить: правый клик по `Task_Manager.Demo` → «Отладка» → «Запустить новый экземпляр». Вывод:

```text
Демонстрация модуля Task_Manager.Core
Файл: C:\Users\...\AppData\Local\Temp\task_manager_demo.txt

1. Добавление задач и сохранение в файл
  Содержимое файла:
    1|Сделать лабораторную
    2|Купить хлеб | молоко
    5|Подготовить отчёт
  [OK]     в менеджере 3 задачи
  [OK]     в файле 3 строки
  [OK]     следующий свободный ID равен 6

2. Загрузка тех же задач новым экземпляром менеджера
  ID: 1, описание: Сделать лабораторную
  ID: 2, описание: Купить хлеб | молоко
  ID: 5, описание: Подготовить отчёт
  [OK]     загружено 3 задачи
  [OK]     символ | в описании не потерялся
  [OK]     задачи идут по возрастанию ID

3. Добавление задачи с уже существующим ID
  [OK]     Add вернул false, задача заменена
  [OK]     количество не изменилось
  [OK]     в файле новое описание

4. Удаление по списку ID: "2, 7, abc"
  Удалено: 2
  Не найдено: 7
  Неверный ID: abc
  [OK]     удалена задача 2
  [OK]     задача 7 не найдена
  [OK]     abc распознан как неверный ID
  [OK]     в файле осталось 2 строки

5. Чтение файла старого формата и строк с мусором
  [OK]     мусорная строка пропущена
  [OK]     лишний пробел после | обрезан

6. Очистка списка
  [OK]     после очистки задач нет
  [OK]     для пустого списка следующий ID равен 1

7. Перенос строки внутри описания
  [OK]     перенос заменён пробелом
  [OK]     в файле одна строка

Итог: пройдено 19, ошибок 0
```

Программа возвращает код 0, если всё прошло, и число ошибок в противном случае, поэтому её можно использовать и как простой интеграционный тест.

---

## 6. Как одно действие проходит через слои

Пользователь добавляет задачу:

```text
 Пользователь          Task_Manager (WPF)                    Task_Manager.Core              Диск
 ────────────          ──────────────────                    ─────────────────              ────
 «Добавить…»  ──────▶  MainWindow.btnAdd_Click
                       │  manager.GetNextId() ─────────────▶ TaskManager.GetNextId()
                       ▼
                       AddTaskWindow (ввод ID и описания)
                       │  idExists(id) ────────────────────▶ TaskManager.Contains()
                       ▼  «Добавить»
                       MainWindow
                       │  manager.Add(id, text) ───────────▶ TaskManager.Add()
                       │  manager.Save() ──────────────────▶ TaskManager.Save()
                       │                                     └▶ TaskStorage.Save() ───────▶ tasks.txt
                       │  manager.GetAll() ────────────────▶ TaskManager.GetAll()
                       ▼
 видит задачу ◀──────  DataGrid обновлён
```

Стрелки идут только слева направо: приложение вызывает модуль, модуль не вызывает приложение.

---

## 7. Интеграция тем и языков

Темы и языки — это тоже подключаемые части, только на уровне ресурсов WPF.

### Как подключено

В `App.xaml` три словаря ресурсов:

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="/Themes/Light.xaml" />
    <ResourceDictionary Source="/Languages/ru.xaml" />
    <ResourceDictionary Source="/Styles/Controls.xaml" />
</ResourceDictionary.MergedDictionaries>
```

* `Themes/Light.xaml` и `Themes/Dark.xaml` содержат одинаковый набор ключей с разными цветами:

  ```xml
  <SolidColorBrush x:Key="WindowBackground" Color="#F3F3F3" />   <!-- Light -->
  <SolidColorBrush x:Key="WindowBackground" Color="#1E1E1E" />   <!-- Dark -->
  ```

* `Languages/ru.xaml` и `Languages/en.xaml` содержат одинаковый набор ключей с разным текстом:

  ```xml
  <sys:String x:Key="Btn_Add">Добавить…</sys:String>   <!-- ru -->
  <sys:String x:Key="Btn_Add">Add…</sys:String>        <!-- en -->
  ```

* Окна обращаются к ресурсам по ключу через `DynamicResource`:

  ```xml
  <Button Content="{DynamicResource Btn_Add}" ... />
  ```

  а стили — к цветам:

  ```xml
  <Setter Property="Background" Value="{DynamicResource ButtonBackground}" />
  ```

### Как работает переключение

`ResourceSwitcher` находит в `App.xaml` словарь из нужной папки и заменяет его другим файлом:

```csharp
ResourceSwitcher.Replace("Themes", "Dark");       // было /Themes/Light.xaml, стало /Themes/Dark.xaml
ResourceSwitcher.Replace("Languages", "en");      // было /Languages/ru.xaml, стало /Languages/en.xaml
```

`DynamicResource`, в отличие от `StaticResource`, следит за ключом. Когда словарь заменяется, все элементы, которые ссылаются на `Btn_Add` или `ButtonBackground`, сами берут новое значение. Поэтому окна перекрашиваются и переводятся сразу, без перезапуска.

Текст, который собирается в коде (например, «Задач: 3»), берётся через `LanguageManager`:

```csharp
txtCount.Text = LanguageManager.Format("Status_Count", manager.Count);
```

Его нужно обновить вручную после смены языка, поэтому `MainWindow` после закрытия окна настроек вызывает `RefreshList()`.

### Пример расширения: добавить третий язык

1. Скопировать `Languages/en.xaml` в `Languages/de.xaml` и перевести строки.
2. Добавить значение `German` в перечисление `AppLanguage` (`Settings/AppSettings.cs`).
3. В `LanguageManager.Apply` сопоставить `AppLanguage.German` с файлом `de`.
4. Добавить `RadioButton` «Deutsch» в `SettingsWindow.xaml` и учесть его в `SettingsWindow.xaml.cs`.

Ни окна, ни модуль `Task_Manager.Core` при этом не меняются: все надписи уже берутся по ключам.

---

## 8. Как показать интеграцию на защите

1. Открыть обозреватель решения и показать три проекта. У `Task_Manager` и `Task_Manager.Demo` в «Зависимости» → «Проекты» есть `Task_Manager.Core`, у самого `Task_Manager.Core` проектов в зависимостях нет.
2. Открыть `Task_Manager.Core.csproj` и показать, что там `net8.0` без `UseWPF`: модуль не зависит от интерфейса.
3. Открыть `App.xaml.cs` и показать место, где создаются `TaskStorage` и `TaskManager` и передаются в `MainWindow`.
4. Запустить `Task_Manager`, добавить пару задач, удалить одну по ID, закрыть программу и открыть `tasks.txt` в папке `bin\Debug\net8.0-windows`.
5. Запустить `Task_Manager.Demo` и показать, что тот же модуль работает в консоли и все проверки проходят.
6. В `Task_Manager` открыть «Настройки…», переключить тему и язык, показать, что всё поменялось сразу, и открыть `settings.json`.
