// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgLocaleCore.Helpers;

public class TgLogHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgLogHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgLogHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public delegate void MarkupLineStampDelegate(string message);
    private MarkupLineStampDelegate _markupLineStamp;
    //public delegate T AskDelegate<T>(string message);
    //private AskDelegate<T> _ask<T>;
    public delegate string AskStringDelegate(string message);
    private AskStringDelegate _askString;
    public delegate int AskIntDelegate(string message);
    private AskIntDelegate _askInt;
    public delegate bool AskBoolDelegate(string message);
    private AskBoolDelegate _askBool;

    public TgLogHelper()
    {
        _markupLineStamp = _ => { };
        _askString = _ => string.Empty;
        _askInt = _ => 0;
        _askBool = _ => false;
    }

    #endregion

    #region Public and private methods - main

    public void Line(string message) => _markupLineStamp(GetLineStamp(message));

    public void Info(string message) => _markupLineStamp(GetLineStampInfo(message));

    public void Warning(string message) => _markupLineStamp(GetLineStampWarning(message));

    #endregion

    #region Public and private methods

    public void SetMarkupLineStamp(MarkupLineStampDelegate markupLineStamp)
    {
        _markupLineStamp = markupLineStamp;
    }

    //public void SetAsk<T>(AskDelegate<T> ask)
    //{
    //    AskDelegate<T> foo = ask;
    //}

    public void SetAskString(AskStringDelegate askString)
    {
        _askString = askString;
    }

    public void SetAskInt(AskIntDelegate askInt)
    {
        _askInt = askInt;
    }

    public void SetAskBool(AskBoolDelegate askBool)
    {
        _askBool = askBool;
    }

    public string GetMarkupString(string message) =>
        message
            .Replace("[", "[[").Replace("]", "]]")
            .Replace("'", "''")
        ;

    public string GetDtStamp() => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

    public string GetDtShortStamp() => $"{DateTime.Now:HH:mm:ss}";

    public string GetLineStamp(string message) => 
        $" {GetDtStamp()} | {GetMarkupString(message)}";

    public string GetLineStampInfo(string message)
    {
        message = message.Replace("[", "[[").Replace("]", "]]");
        message = message.Replace("'", "''");
        return $"[green] {GetDtStamp()} | i {message}[/]";
    }

    public string GetLineStampWarning(string message)
    {
        message = message.Replace("[", "[[").Replace("]", "]]");
        message = message.Replace("'", "''");
        return $"[red] {GetDtStamp()} | x {message}[/]";
    }

    public string AskString(string message) => _askString(message);

    public int AskInt(string message) => _askInt(message);

    public bool AskBool(string message) => _askBool(message);

    #endregion
}
