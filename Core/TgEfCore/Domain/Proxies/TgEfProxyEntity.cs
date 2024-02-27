// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Proxies;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableProxies)]
public sealed class TgEfProxyEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(TgEnumProxyType.None)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnType)]
    [SQLite.Indexed]
    public TgEnumProxyType Type { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnHostName)]
    [SQLite.Indexed]
    public string HostName { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnPort)]
    [SQLite.Indexed]
    public ushort Port { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnUserName)]
    [SQLite.Indexed]
    public string UserName { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnPassword)]
    [SQLite.Indexed]
    public string Password { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnSecret)]
    [SQLite.Indexed]
    public string Secret { get; set; }

    public TgEfProxyEntity() : base()
    {
        Type = this.GetPropertyDefaultValueAsGeneric<TgEnumProxyType>(nameof(Type));
        HostName = this.GetPropertyDefaultValue(nameof(HostName));
        Port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(Port));
        UserName = this.GetPropertyDefaultValue(nameof(UserName));
        Password = this.GetPropertyDefaultValue(nameof(Password));
        Secret = this.GetPropertyDefaultValue(nameof(Secret));
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{base.ToDebugString()} | {Type} | {HostName} | {Port} | {UserName} | {Password} | " +
        $"{TgCommonUtils.GetIsFlag(!string.IsNullOrEmpty(Secret), Secret, "<No secret>")}";

    #endregion
}