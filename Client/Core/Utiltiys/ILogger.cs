
namespace KIComunication.Core.Utiltiys
{
    internal class ILogger
    {
        public static void WrittenLog(string msg) => File.AppendAllText(Path.Combine(Mapper.Main, "AppLogs.txt"), $"{DateTime.Now} | {msg}{Environment.NewLine}");
    }
}
