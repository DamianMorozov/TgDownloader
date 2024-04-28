// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgStorageConstants.TableApps)]
[Index(nameof(ApiHash), IsUnique = true)]
[Index(nameof(ApiId))]
[Index(nameof(PhoneNumber))]
[Index(nameof(ProxyUid))]
public sealed class TgEfAppEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnApiHash)]
    public Guid ApiHash { get; set; }
    
    [NotMapped]
    public string ApiHashString
	{
	    get => ApiHash.ToString();
	    set => ApiHash = Guid.TryParse(value, out Guid apiHash) ? apiHash : Guid.Empty;
    }

	[DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnApiId)]
    public int ApiId { get; set; }

	[NotMapped]
	public string ApiIdString
	{
		get => ApiId.ToString();
		set => ApiId = int.TryParse(value, out int apiId) ? apiId : 0;
	}

	[DefaultValue("+00000000000")]
	[ConcurrencyCheck]
	[MaxLength(16)]
	[Column(TgStorageConstants.ColumnPhoneNumber)]
	public string PhoneNumber { get; set; } = default!;

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnProxyUid)]
    public Guid? ProxyUid { get; set; }
	
    [NotMapped]
	public string ProxyUidString
	{
		get => ProxyUid is null ? Guid.Empty.ToString() : ProxyUid.ToString();
		set => ProxyUid = Guid.TryParse(value, out Guid proxyUid) ? proxyUid : Guid.Empty;
	}

	public TgEfProxyEntity? Proxy { get; set; }

    public TgEfAppEntity() : base()
    {
	    Default();
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{TgStorageConstants.TableApps} | {base.ToDebugString()} | {ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

    public override void Default()
    {
        base.Default();
        ApiHash = this.GetDefaultPropertyGuid(nameof(ApiHash));
        ApiId = this.GetDefaultPropertyInt(nameof(ApiId));
        PhoneNumber = this.GetDefaultPropertyString(nameof(PhoneNumber));
		ProxyUid = this.GetDefaultPropertyGuid(nameof(ProxyUid));
    }

	public override void Fill(object item)
    {
		if (item is not TgEfAppEntity app)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfAppEntity)}!");
	    
	    ApiHash = app.ApiHash;
	    ApiId = app.ApiId;
	    PhoneNumber = app.PhoneNumber;
	    ProxyUid = app.ProxyUid;
    }

	public override void Backup(object item)
	{
		Fill(item);
		base.Backup(item);
	}

	#endregion
}