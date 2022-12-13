// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgDownloaderCore.Helpers;

namespace TgDownloaderCore.Models;

public class TgSettingsModel
{
    #region Public and private fields, properties, constructor

    public LocaleHelper Locale => LocaleHelper.Instance;
    public LogHelper Log => LogHelper.Instance;
    public string SourceUserName { get; private set; }
    public string DestDirectory { get; private set; }
    public int MessageCurrentId { get; private set; }
    public int MessageCount { get; private set; }

    public TgSettingsModel()
    {
        SetDefault();
    }

    #endregion

    #region Public and private methods

    private void SetDefault()
    {
        SourceUserName = string.Empty;
        DestDirectory = string.Empty;
        MessageCurrentId = 1;
        MessageCount = 0;
    }

    public void SetSourceUserName()
    {
        SetDefault();
        bool isCheck;
        do
        {
            string userName = Log.AskString(Locale.TypeTgSourceUserName);
            if (!string.IsNullOrEmpty(userName))
            {
                SourceUserName = userName.StartsWith(@"https://t.me/")
                    ? userName.Replace("https://t.me/", string.Empty) : userName;
            }
            isCheck = !string.IsNullOrEmpty(SourceUserName);
        } while (!isCheck);
    }
    
    public void SetDestDirectory()
    {
        DestDirectory = string.Empty;
        bool isCheck = false;
        do
        {
            do
            {
                if (!string.IsNullOrEmpty(DestDirectory) &&
                    !Directory.Exists(DestDirectory))
                    Log.MarkupLineStamp(Locale.DirNotFound(DestDirectory));
                DestDirectory = Log.AskString(Locale.DestDirectory);
            } while (!Directory.Exists(DestDirectory));
            if (!Directory.Exists(DestDirectory))
            {
                DestDirectory = string.Empty;
                Log.MarkupLineStamp(Locale.DirIsNotExists);
            }
            else
            {
                isCheck = true;
            }
        } while (!isCheck);
    }

    public void SetMessageCurrentId()
    {
        MessageCurrentId = Log.AskInt(Locale.TypeTgMessageStartId);
        MessageCurrentId = MessageCurrentId < 1 ? 1 : MessageCurrentId;
    }

    public void SetMessageCurrentIdDefault() => MessageCurrentId = 1;

    public void AddMessageCurrentId(int count = 1) => MessageCurrentId += count;

    public void SetMessageCount(int count) => MessageCount = count;

    #endregion
}