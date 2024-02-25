// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Apps;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableApps)]
public sealed class TgEfAppEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnApiHash)]
    public Guid ApiHash { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnApiId)]
    public int ApiId { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(16)]
    [Column(TgSqlConstants.ColumnPhoneNumber)]
    public string PhoneNumber { get; set; }

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnProxyUid)]
    public Guid ProxyUid { get; set; }

    public TgEfProxyEntity Proxy { get; set; }

    public TgEfAppEntity() : base()
    {
        ApiHash = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
        ApiId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(ApiId));
        PhoneNumber = this.GetPropertyDefaultValue(nameof(PhoneNumber));
        ProxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ProxyUid));
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{base.ToDebugString()} | {ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

    #endregion
}