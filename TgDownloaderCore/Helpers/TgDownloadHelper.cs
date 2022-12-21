// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Helpers;

public class TgDownloadHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgDownloadHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgDownloadHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    public TgLogHelper TgLog => TgLogHelper.Instance;
    public long? SourceId { get; private set; }
    public string SourceUserName { get; private set; }
    public string DestDirectory { get; private set; }
    public int MessageCurrentId { get; private set; }
    public int MessageCount { get; private set; }
    public bool IsRewriteFiles { get; private set; }
    public bool IsReady
    {
        get
        {
            if (!IsReadySourceUserName)
                return false;
            if (!IsReadyDestDirectory)
                return false;
            return true;
        }
    }
    public bool IsReadySourceUserName => !string.IsNullOrEmpty(SourceUserName);
    public bool IsReadyDestDirectory => !string.IsNullOrEmpty(DestDirectory);

    public TgDownloadHelper()
    {
        SourceUserName = string.Empty;
        SourceId = -1;
        DestDirectory = string.Empty;
        MessageCurrentId = -1;
        MessageCount = 0;
        IsRewriteFiles = false;
    }

    #endregion

    #region Public and private methods

    private void SetDefault(int messageCurrentId)
    {
        SourceUserName = string.Empty;
        SourceId = 0;
        DestDirectory = string.Empty;
        MessageCurrentId = messageCurrentId;
        MessageCount = 0;
        IsRewriteFiles = false;
    }

    public void SetSourceUserName()
    {
        SetDefault(1);
        bool isCheck;
        do
        {
            string userName = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceUserName));
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
        do
        {
            DestDirectory = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TypeDestDirectory));
            if (!Directory.Exists(DestDirectory))
                TgLog.Info(TgLocale.DirIsNotExistsSpecify(DestDirectory));
        } while (!Directory.Exists(DestDirectory));
    }

    public void SetMessageCurrentId()
    {
        MessageCurrentId = TgLog.AskInt(TgLocale.TypeTgMessageStartId);
        MessageCurrentId = MessageCurrentId < 1 ? 1 : MessageCurrentId;
    }

    public void SetIsRewriteFiles() =>
        IsRewriteFiles = TgLog.AskBool(TgLocale.TypeTgIsMessageRewrite);

    public void SetMessageCurrentIdDefault() => MessageCurrentId = 1;

    public void AddMessageCurrentId(int count = 1) => MessageCurrentId += count;

    public void SetMessageCount(int count) => MessageCount = count;

    public void SetSourceId(long? id)
    {
        SourceId = id;
    }

    #endregion
}