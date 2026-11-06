using System.Security.Cryptography;
using PkCompletionist.Core;

namespace PkCompletionist.Tests;

internal sealed record DescriptorResult(string SaveFile, string SaveSha256, string VersionHint,
    string[] Descriptor, string[] Messages);

internal static class DescriptorSnapshot
{
    public static IEnumerable<ValidatorCase> Cases => ValidatorCase.All().Where(c => c.Objective == "normal");

    public static DescriptorResult Capture(string savesDirectory, ValidatorCase testCase)
    {
        var input = File.ReadAllBytes(Path.Combine(savesDirectory, testCase.SaveFile));
        var hash = Convert.ToHexString(SHA256.HashData(input));
        Command.LastCommand = null;
        try
        {
            return new(testCase.SaveFile, hash, testCase.VersionHint,
                SavDescriptor.Execute(input, testCase.VersionHint), Command.GetMessages());
        }
        finally
        {
            Command.LastCommand = null;
        }
    }
}
