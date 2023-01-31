// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Apps;

[Table("APPS")]
[Obsolete(@"Use SqlTableAppDeprecatedModel")]
public class SqlTableAppDeprecatedModel : SqlTableBase
{
    #region Public and private fields, properties, constructor

    [PrimaryKey]
    [Column("API_HASH")]
    [DefaultValue("")]
    public string ApiHash { get; set; }
    [Column("PHONE_NUMBER")]
    [DefaultValue("")]
    public string PhoneNumber { get; set; }

    public SqlTableAppDeprecatedModel()
    {
        ApiHash = this.GetPropertyDefaultValueAsString(nameof(ApiHash));
        PhoneNumber = this.GetPropertyDefaultValueAsString(nameof(PhoneNumber));
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableAppDeprecatedModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ApiHash = info.GetString(nameof(ApiHash)) ?? this.GetPropertyDefaultValueAsString(nameof(ApiHash));
        PhoneNumber = info.GetString(nameof(PhoneNumber)) ?? this.GetPropertyDefaultValueAsString(nameof(PhoneNumber));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ApiHash), ApiHash);
        info.AddValue(nameof(PhoneNumber), PhoneNumber);
    }

    #endregion
}