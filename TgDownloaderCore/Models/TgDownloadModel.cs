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

    [DefaultValue(0)]
    public long SourceId { get; set; }
    [DefaultValue("")]
    public string SourceUserName { get; set; }
    [DefaultValue("")]
    public string SourceTitle { get; private set; }
    [DefaultValue("")]
    public string SourceAbout { get; private set; }
    [DefaultValue("")]
    public string DestDirectory { get; set; }
    [DefaultValue(1)]
    public int SourceFirstId { get; set; }
    [DefaultValue(1)]
    public int SourceLastId { get; set; }
    [DefaultValue(false)]
    public bool IsRewriteFiles { get; set; }
    [DefaultValue(false)]
    public bool IsRewriteMessages { get; set; }
    [DefaultValue(true)]
    public bool IsJoinFileNameWithMessageId { get; set; }

    public bool IsReady => IsReadySourceId && IsReadyDestDirectory;
    public bool IsReadySourceId => SourceId is not 0;
    public bool IsReadySourceFirstId => SourceFirstId > 0;
    public bool IsReadySourceUserName => !Equals(SourceUserName, string.Empty);
    public bool IsReadyDescription => !string.IsNullOrEmpty(SourceAbout);
    public bool IsReadyDestDirectory => !string.IsNullOrEmpty(DestDirectory);

    public TgDownloadModel()
    {
        DestDirectory = this.GetPropertyDefaultValueAsString(nameof(DestDirectory));
        IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsBool(nameof(IsJoinFileNameWithMessageId));
        IsRewriteFiles = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteFiles));
        IsRewriteMessages = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteMessages));
        SourceLastId = this.GetPropertyDefaultValueAsInt(nameof(SourceLastId));
        SourceFirstId = this.GetPropertyDefaultValueAsInt(nameof(SourceFirstId));
        SourceId = this.GetPropertyDefaultValueAsInt(nameof(SourceId));
        SourceUserName = this.GetPropertyDefaultValueAsString(nameof(SourceUserName));
        SourceTitle = this.GetPropertyDefaultValueAsString(nameof(SourceTitle));
        SourceAbout= this.GetPropertyDefaultValueAsString(nameof(SourceAbout));
    }

    #endregion

    #region Public and private methods

    public void SetDefault(int messageCurrentId)
    {
        DestDirectory = this.GetPropertyDefaultValueAsString(nameof(DestDirectory));
        IsJoinFileNameWithMessageId = this.GetPropertyDefaultValueAsBool(nameof(IsJoinFileNameWithMessageId));
        IsRewriteFiles = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteFiles));
        IsRewriteMessages = this.GetPropertyDefaultValueAsBool(nameof(IsRewriteMessages));
        SourceLastId = this.GetPropertyDefaultValueAsInt(nameof(SourceLastId));
        SourceFirstId = messageCurrentId;
        SourceId = this.GetPropertyDefaultValueAsInt(nameof(SourceId));
        SourceUserName = this.GetPropertyDefaultValueAsString(nameof(SourceUserName));
        SourceTitle = this.GetPropertyDefaultValueAsString(nameof(SourceTitle));
        SourceAbout = this.GetPropertyDefaultValueAsString(nameof(SourceAbout));
    }

    public void SetSource(long id, string title, string about)
    {
        SourceId = id;
        SourceTitle = title;
        SourceAbout = about;
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
        SourceLastId = info.GetInt32(nameof(SourceLastId));
        SourceFirstId = info.GetInt32(nameof(SourceFirstId));
        SourceId = info.GetInt64(nameof(SourceId));
        SourceFirstId = info.GetInt32(nameof(SourceFirstId));
        SourceUserName = info.GetString(nameof(SourceUserName)) ?? this.GetPropertyDefaultValueAsString(nameof(SourceUserName));
        SourceTitle = info.GetString(nameof(SourceTitle)) ?? this.GetPropertyDefaultValueAsString(nameof(SourceTitle));
        SourceAbout = info.GetString(nameof(SourceAbout)) ?? this.GetPropertyDefaultValueAsString(nameof(SourceAbout));
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
        info.AddValue(nameof(SourceLastId), SourceLastId);
        info.AddValue(nameof(SourceFirstId), SourceFirstId);
        info.AddValue(nameof(SourceId), SourceId);
        info.AddValue(nameof(SourceFirstId), SourceFirstId);
        info.AddValue(nameof(SourceUserName), SourceUserName);
        info.AddValue(nameof(SourceTitle), SourceTitle);
        info.AddValue(nameof(SourceAbout), SourceAbout);
    }

    #endregion
}