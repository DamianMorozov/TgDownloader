// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com


namespace TgDownloaderBlazor;

public partial class App
{
    private static bool IsCurrentDirSet { get; set; }

    public App()
    {
#if DEBUG
        if (!IsCurrentDirSet)
        {
            IsCurrentDirSet = true;
            string debugAnyCpu =
#if NET8_0
                "bin\\Debug_AnyCPU\\net9.0";
#else
                "bin\\Debug_AnyCPU";
#endif
            if (!Environment.CurrentDirectory.Contains(debugAnyCpu) && Environment.Is64BitProcess)
                Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, debugAnyCpu);
        }
#endif
    }
}