// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Runtime.Serialization;
using TgLocaleCore.Interfaces;
using TgLocaleCore.Utils;

namespace TgDownloaderCore.Models;

public class TgDownloadModel : IModel
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgDownloadModel _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgDownloadModel Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    public TgLogHelper TgLog => TgLogHelper.Instance;

    [DefaultValue(null)]
    public long? SourceId { get; private set; }
    [DefaultValue("")]
    public string SourceUserName { get; private set; }
    [DefaultValue("")]
    public string DestDirectory { get; private set; }
    [DefaultValue(-1)]
    public int MessageCurrentId { get; private set; }
    [DefaultValue(0)]
    public int MessageCount { get; private set; }
    [DefaultValue(false)]
    public bool IsRewriteFiles { get; private set; }
    [DefaultValue(false)]
    public bool IsRewriteMessages { get; private set; }
    [DefaultValue(true)]
    public bool IsJoinFileNameWithMessageId { get; private set; }

    public bool IsReady => (IsReadySourceId || IsReadySourceUserName) && IsReadyDestDirectory;
    public bool IsReadySourceId => SourceId is not null && SourceId is not 0;
    public bool IsReadySourceUserName => !Equals(SourceUserName, string.Empty);
    public bool IsReadyDestDirectory => !string.IsNullOrEmpty(DestDirectory);

    public TgDownloadModel()
    {
        DestDirectory = this.GetPropertyDefaultValueAsString(nameof(DestDirectory));
        IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsBool(nameof(IsJoinFileNameWithMessageId));
        IsRewriteFiles = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteFiles));
        IsRewriteMessages = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteMessages));
        MessageCount = this.GetPropertyDefaultValueAsInt(nameof(MessageCount));
        MessageCurrentId = this.GetPropertyDefaultValueAsInt(nameof(MessageCurrentId));
        SourceId = this.GetPropertyDefaultValueAsInt(nameof(SourceId));
        SourceUserName = this.GetPropertyDefaultValueAsString(nameof(SourceUserName));
    }

    #endregion

    #region Public and private methods

    private void SetDefault(int messageCurrentId)
    {
        DestDirectory = this.GetPropertyDefaultValueAsString(nameof(DestDirectory));
        IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsBool(nameof(IsJoinFileNameWithMessageId));
        IsRewriteFiles = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteFiles));
        IsRewriteMessages = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteMessages));
        MessageCount = this.GetPropertyDefaultValueAsInt(nameof(MessageCount));
        MessageCurrentId = messageCurrentId;
        SourceId = this.GetPropertyDefaultValueAsInt(nameof(SourceId));
        SourceUserName = this.GetPropertyDefaultValueAsString(nameof(SourceUserName));
    }

    public void SetSourceIdByAsk()
    {
        SetDefault(1);
        bool isCheck;
        do
        {
            SourceId = TgLog.AskLong(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceId));
            isCheck = IsReadySourceId;
        } while (!isCheck);
    }

    public void SetSourceUserNameByAsk()
    {
        SetDefault(1);
        bool isCheck;
        do
        {
            string sourceUserName = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceUserName));
            if (!string.IsNullOrEmpty(sourceUserName))
            {
                SourceUserName = sourceUserName.StartsWith(@"https://t.me/")
                    ? sourceUserName.Replace("https://t.me/", string.Empty) : sourceUserName;
            }
            isCheck = !string.IsNullOrEmpty(SourceUserName);
        } while (!isCheck);
    }

    public void SetSourceUserNameByName(string sourceUserName)
    {
        SourceUserName = sourceUserName;
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
        IsRewriteFiles = TgLog.AskBool(TgLocale.TypeTgIsRewriteFiles);

    public void SetIsRewriteMessages() =>
        IsRewriteMessages = TgLog.AskBool(TgLocale.TypeTgIsRewriteMessages);

    public void SetIsAddMessageId()
    {
        IsJoinFileNameWithMessageId = TgLog.AskBool(TgLocale.TypeTgIsAddMessageId);
    }

    public void SetMessageCurrentIdDefault() => MessageCurrentId = 1;

    public void AddMessageCurrentId(int count = 1) => MessageCurrentId += count;

    public void SetMessageCount(int count) => MessageCount = count;

    public void SetSourceId(long? id)
    {
        SourceId = id;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected TgDownloadModel(SerializationInfo info, StreamingContext context)
    {
        DestDirectory = info.GetString(nameof(DestDirectory)) ?? this.GetPropertyDefaultValueAsString(nameof(DestDirectory));
        IsJoinFileNameWithMessageId = info.GetBoolean(nameof(IsJoinFileNameWithMessageId));
        IsRewriteFiles = info.GetBoolean(nameof(IsRewriteFiles));
        IsRewriteMessages = info.GetBoolean(nameof(IsRewriteMessages));
        MessageCount = info.GetInt32(nameof(MessageCount));
        MessageCurrentId = info.GetInt32(nameof(MessageCurrentId));
        SourceId = info.GetInt64(nameof(SourceId));
        SourceUserName = info.GetString(nameof(SourceUserName)) ?? this.GetPropertyDefaultValueAsString(nameof(SourceUserName));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(DestDirectory), DestDirectory);
        info.AddValue(nameof(IsJoinFileNameWithMessageId), IsJoinFileNameWithMessageId);
        info.AddValue(nameof(IsRewriteFiles), IsRewriteFiles);
        info.AddValue(nameof(IsRewriteMessages), IsRewriteMessages);
        info.AddValue(nameof(MessageCount), MessageCount);
        info.AddValue(nameof(MessageCurrentId), MessageCurrentId);
        info.AddValue(nameof(SourceId), SourceId);
        info.AddValue(nameof(SourceUserName), SourceUserName);
    }

    #endregion
}