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
    [SQLite.Indexed]
    public Guid ApiHash { get; set; }
    [NotMapped]
    public string ApiHashString
	{
	    get => ApiHash.ToString();
	    set => ApiHash = Guid.TryParse(value, out Guid apiHash) ? apiHash : Guid.Empty;
    }

	[DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnApiId)]
    [SQLite.Indexed]
    public int ApiId { get; set; }

	[NotMapped]
	public string ApiIdString
	{
		get => ApiId.ToString();
		set => ApiId = int.TryParse(value, out int apiId) ? apiId : 0;
	}

	[DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(16)]
    [Column(TgSqlConstants.ColumnPhoneNumber)]
    [SQLite.Indexed]
    public string PhoneNumber { get; set; }

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnProxyUid)]
    [SQLite.Indexed]
    public Guid ProxyUid { get; set; }
	[NotMapped]
	public string ProxyUidString
	{
		get => ProxyUid.ToString();
		set => ProxyUid = Guid.TryParse(value, out Guid proxyUid) ? proxyUid : Guid.Empty;
	}

	public TgEfProxyEntity? Proxy { get; set; }

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