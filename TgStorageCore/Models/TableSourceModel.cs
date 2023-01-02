// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageCore.Models;

[DebuggerDisplay("{nameof(TableSourcesModel)} | {Id} | {UserName}")]
[Table("SOURCES")]
public class TableSourceModel : TableBase
{
    #region Public and private fields, properties, constructor

    [PrimaryKey]
    [Column("ID")]
    [DefaultValue(0)]
    public long Id { get; set; }
    [Indexed]
    [Column("USER_NAME")]
    [DefaultValue("")]
    public string UserName { get; set; }

    public TableSourceModel()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        UserName = this.GetPropertyDefaultValueAsString(nameof(UserName));
    }

    public TableSourceModel(long id, string userName)
    {
        Id = id;
        UserName = userName;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected TableSourceModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetInt64(nameof(Id));
        UserName = info.GetString(nameof(UserName)) ?? this.GetPropertyDefaultValueAsString(nameof(UserName));
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
        info.AddValue(nameof(UserName), UserName);
    }

    #endregion
}