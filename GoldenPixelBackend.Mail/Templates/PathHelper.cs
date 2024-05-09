namespace GoldenPixelBackend.Mail.Templates;
/// <summary>
/// Методы для работы с путями в контексте почтового сервиса
/// </summary>
public static class PathHelpers
{
    /// <summary>
    /// Получить директорию шаблонов
    /// </summary>
    public static Func<string> GetLocalPath = () =>
    Path.GetFullPath(Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(a => a.GetName().Name == "GoldenPixelBackend.Mail")?.Location), "Templates"));

    /// <summary>
    /// Получить путь шаблона
    /// </summary>
    public static Func<string, string> GetTemplatePath = templateName =>
        Path.Combine(GetLocalPath(), templateName);
}
