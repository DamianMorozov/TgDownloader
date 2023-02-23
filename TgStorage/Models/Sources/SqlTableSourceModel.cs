// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Sources;

[DebuggerDisplay("{nameof(TableSourcesModel)} | {Id} | {UserName}")]
[SQLite.Table("SOURCES")]
public class SqlTableSourceModel : SqlTableBase
{
    #region Public and private fields, properties, constructor

    [SQLite.PrimaryKey]
    [SQLite.Column("ID")]
    [DefaultValue(0)]
    public long Id { get; set; }
    
    [SQLite.Indexed]
    [SQLite.Column("USER_NAME")]
    [DefaultValue("")]
    public string UserName { get; set; }
    
    [SQLite.Column("TITLE")]
    [DefaultValue("")]
    public string Title { get; set; }
    
    [SQLite.Column("ABOUT")]
    [DefaultValue("")]
    public string About { get; set; }
    
    [SQLite.Column("COUNT")]
    [DefaultValue(0)]
    public int Count { get; set; }

    public SqlTableSourceModel()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        UserName = this.GetPropertyDefaultValue(nameof(UserName));
        Title = this.GetPropertyDefaultValue(nameof(Title));
        About = this.GetPropertyDefaultValue(nameof(About));
        Count = this.GetPropertyDefaultValueAsInt(nameof(Count));
    }

    public SqlTableSourceModel(long id, string userName, string title, string about, int count)
    {
        Id = id;
        UserName = userName;
        Title = title;
        About = about;
        Count = count;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableSourceModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetInt64(nameof(Id));
        UserName = info.GetString(nameof(UserName)) ?? this.GetPropertyDefaultValue(nameof(UserName));
        Title = info.GetString(nameof(Title)) ?? this.GetPropertyDefaultValue(nameof(Title));
        About = info.GetString(nameof(About)) ?? this.GetPropertyDefaultValue(nameof(About));
        Count = info.GetInt32(nameof(Count));
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
        info.AddValue(nameof(Title), Title);
        info.AddValue(nameof(About), About);
        info.AddValue(nameof(Count), Count);
    }

    #endregion
}