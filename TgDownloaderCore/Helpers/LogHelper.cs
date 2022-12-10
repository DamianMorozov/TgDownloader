// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Helpers;

public class LogHelper
{
    #region Design pattern "Lazy Singleton"

    private static LogHelper _instance;
    public static LogHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

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

    public LogHelper()
    {
        _markupLineStamp = _ => { };
        _askString = _ => string.Empty;
        _askInt = _ => 0;
        _askBool = _ => false;
    }

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

    public string GetMarkupString(string line) =>
        line.Replace("[", "[[").Replace("]", "]]");

    public void MarkupLineStamp(string message)
    {
        message = message.Replace("[", "[[").Replace("]", "]]");
        message = message.Replace("'", "''");
        string result = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
        _markupLineStamp(result);
    }

    public string AskString(string message) => _askString(message);

    public int AskInt(string message) => _askInt(message);

    public bool AskBool(string message) => _askBool(message);

    #endregion
}
