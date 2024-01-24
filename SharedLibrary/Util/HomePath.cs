namespace SharedLibrary.Util;

public static class HomePath
{
    public static string ParseHome(this string path)
    {
        var home = Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        return path.Replace("~", home);
    }

    public static string MyDir(this string path)
    {
        var home = Environment.CurrentDirectory;
        return path.Replace("~", home);
    }
}