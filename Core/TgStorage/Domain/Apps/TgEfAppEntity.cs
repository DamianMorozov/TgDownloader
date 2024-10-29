// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgEfConstants.TableApps)]
[Index(nameof(Uid), IsUnique = true)]
[Index(nameof(ApiHash), IsUnique = true)]
[Index(nameof(ApiId))]
[Index(nameof(PhoneNumber))]
[Index(nameof(ProxyUid))]
public sealed class TgEfAppEntity : ITgDbEntity, ITgDbFillEntity<TgEfAppEntity>
{
	#region Public and private fields, properties, constructor

	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Key]
	//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Required]
	[Column(TgEfConstants.ColumnUid, TypeName = "CHAR(36)")]
	[SQLite.Collation("NOCASE")]
	public Guid Uid { get; set; }

	[DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnApiHash, TypeName = "CHAR(36)")]
    [SQLite.Collation("NOCASE")]
    public Guid ApiHash { get; set; }
    
    [NotMapped]
    public string ApiHashString
	{
	    get => ApiHash.ToString();
	    set => ApiHash = Guid.TryParse(value, out Guid apiHash) ? apiHash : Guid.Empty;
    }

	[DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnApiId, TypeName = "INT")]
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
	[Column(TgEfConstants.ColumnPhoneNumber, TypeName = "NVARCHAR(16)")]
	public string PhoneNumber { get; set; } = default!;

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnProxyUid, TypeName = "CHAR(36)")]
    [SQLite.Collation("NOCASE")]
	public Guid? ProxyUid { get; set; }

	public TgEfProxyEntity? Proxy { get; set; }

    public TgEfAppEntity() : base()
    {
	    Default();
    }

    #endregion

    #region Public and private methods

    public string ToDebugString() =>
        $"{TgEfConstants.TableApps} | {Uid} | {ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

    public void Default()
    {
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		ApiHash = this.GetDefaultPropertyGuid(nameof(ApiHash));
        ApiId = this.GetDefaultPropertyInt(nameof(ApiId));
        PhoneNumber = this.GetDefaultPropertyString(nameof(PhoneNumber));
		//ProxyUid = this.GetDefaultPropertyGuid(nameof(ProxyUid));
		ProxyUid = null;
    }

	public TgEfAppEntity Fill(TgEfAppEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		//Uid = item is { } entity ? entity.Uid : Guid.Empty;
		ApiHash = item.ApiHash;
	    ApiId = item.ApiId;
	    PhoneNumber = item.PhoneNumber;
	    ProxyUid = item.ProxyUid;
		return this;
    }

	public TgEfAppEntity Backup(object item)
	{
		return this;
	}

	#endregion
}