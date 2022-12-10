// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgDownloaderCore.Helpers;
using TgDownloaderCore.Locales;

namespace TgDownloaderCore.Models;

public class TgSettingsModel
{
    #region Public and private fields, properties, constructor

    public LocaleHelper Locale => LocaleHelper.Instance;
    public LogHelper Log => LogHelper.Instance;
    public string SourceUserName { get; private set; }
    public string DestDirectory { get; private set; }
    public int MessageStartId { get; private set; }
    public int MessageCount { get; private set; }

    public TgSettingsModel()
    {
        SourceUserName = string.Empty;
        DestDirectory = string.Empty;
        MessageStartId = -1;
        MessageCount = -1;
    }

    #endregion

    #region Public and private methods

    public void SetSourceUserName()
    {
        SourceUserName = string.Empty;
        bool isCheck;
        do
        {
            string userName = Log.AskString(Locale.Question.TypeTgSourceUserName);
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
                    Log.MarkupLineStamp(Locale.Warning.DirNotFound(DestDirectory));
                DestDirectory = Log.AskString(Locale.Info.DestDirectory);
            } while (!Directory.Exists(DestDirectory));
            if (!Directory.Exists(DestDirectory))
            {
                DestDirectory = string.Empty;
                Log.MarkupLineStamp(Locale.Warning.DirIsNotExists);
            }
            else
            {
                isCheck = true;
            }
        } while (!isCheck);
    }

    public void SetMessageStartId()
    {
        MessageStartId = Log.AskInt(Locale.Question.TypeTgMessageStartId);
        MessageStartId = MessageStartId < 1 ? 1 : MessageStartId;
    }

    public void AddMessageStartId(int count = 1)
    {
        MessageStartId += count;
    }

    public void SetMessageCount()
    {
        MessageCount = Log.AskInt(Locale.Question.TypeTgMessageCount);
        MessageCount = MessageCount < 0 ? 0 : MessageCount;
    }

    #endregion
}