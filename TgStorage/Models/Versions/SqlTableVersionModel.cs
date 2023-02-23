// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Versions;

[DebuggerDisplay("{nameof(SqlTableVersionModel)} | {Uid} | {Version} | {Description}")]
[Persistent("VERSIONS")]
public class SqlTableVersionModel : SqlTableXpLiteBase
{
    #region Public and private fields, properties, constructor

    private ushort _version;
    [DefaultValue(1)]
    [Persistent("VERSION")]
    public ushort Version { get => _version; set => SetPropertyValue(nameof(_version), ref _version, value); }

    [DefaultValue("")]
    private string _description;
    [Size(128)]
    [Persistent("DESCRIPTION")]
    public string Description { get => _description; set => SetPropertyValue(nameof(_description), ref _description, value); }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlTableVersionModel()
    {
        _version = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_version));
        _description = this.GetPropertyDefaultValue(nameof(_description));
    }

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public SqlTableVersionModel(Session session) : base(session)
    {
        _version = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_version));
        _description = this.GetPropertyDefaultValue(nameof(_description));
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableVersionModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _version = info.GetUInt16(nameof(_version));
        _description = info.GetString(nameof(_description)) ?? string.Empty;
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(_version), _version);
        info.AddValue(nameof(_description), _description);
    }

    #endregion

    #region Public and private methods

    public override string ToString() =>
        base.ToString() + " | " +
        $"{nameof(Version)} = {Version} | " +
        $"{nameof(Description)} = {Description}";

    #endregion
}