using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class Meta
{
    [JSExport]
    public static string GetVersion()
    {
        return "1.0";
    }
}