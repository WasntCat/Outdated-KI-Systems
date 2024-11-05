
namespace KIComunication.Core.Utiltiys
{
    internal class Mapper
    {
        public static readonly string Rsc = Path.Combine(Main, @"Rsc");

        public static bool dsc => File.Exists(Path.Combine(Rsc, "DscData.Darling"));
        public static bool key => File.Exists(Path.Combine(Rsc, "KeyData.Darling"));
    }
}
