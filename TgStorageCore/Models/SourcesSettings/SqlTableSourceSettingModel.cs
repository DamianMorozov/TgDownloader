// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Utils;

namespace TgStorageCore.Models.SourcesSettings;

[DebuggerDisplay("{nameof(TableSourceSettingModel)} | {Id} | {SourceId} | {Directory}")]
[Table("SOURCES_SETTINGS")]
public class SqlTableSourceSettingModel : SqlTableBase
{
    #region Public and private fields, properties, constructor

    [PrimaryKey, AutoIncrement]
    [Column("ID")]
    [DefaultValue(0)]
    public long Id { get; set; }
    [Indexed]
    [Column("SOURCE_ID")]
    [DefaultValue(0)]
    public long SourceId { get; set; }
    [Column("DIRECTORY")]
    [DefaultValue("")]
    public string Directory { get; set; }
    [Column("FIRST_ID")]
    [DefaultValue(1)]
    public int FirstId { get; set; }
    [Column("IS_AUTO_UPDATE")]
    [DefaultValue(0)]
    public bool IsAutoUpdate { get; set; }

    public SqlTableSourceSettingModel()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        SourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
        Directory = this.GetPropertyDefaultValueAsString(nameof(Directory));
        FirstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(FirstId));
        IsAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
    }

    public SqlTableSourceSettingModel(long sourceId, string directory, int firstId, bool isAutoUpdate) : this()
    {
        SourceId = sourceId;
        Directory = directory;
        FirstId = firstId;
        IsAutoUpdate = isAutoUpdate;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableSourceSettingModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetInt64(nameof(Id));
        SourceId = info.GetInt64(nameof(SourceId));
        Directory = info.GetString(nameof(Directory)) ?? this.GetPropertyDefaultValueAsString(nameof(Directory));
        FirstId = info.GetInt32(nameof(FirstId));
        IsAutoUpdate = info.GetBoolean(nameof(IsAutoUpdate));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Id), Id);
        info.AddValue(nameof(SourceId), SourceId);
        info.AddValue(nameof(Directory), Directory);
        info.AddValue(nameof(FirstId), FirstId);
        info.AddValue(nameof(IsAutoUpdate), IsAutoUpdate);
    }

    #endregion
}