// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgEfConstants.TableProxies)]
[Index(nameof(Type))]
[Index(nameof(HostName))]
[Index(nameof(Port))]
[Index(nameof(UserName))]
[Index(nameof(Password))]
[Index(nameof(Secret))]
public sealed partial class TgEfProxyEntity : TgEfEntityBase, ITgDbProxy
{
    #region Public and private fields, properties, constructor

    [DefaultValue(TgEnumProxyType.None)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnType, TypeName = "INT")]
    public TgEnumProxyType Type { get; set; }

    [DefaultValue("No proxy")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnHostName, TypeName = "INT")]
    public string HostName { get; set; } = default!;

    [DefaultValue(404)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnPort, TypeName = "INT(5)")]
    public ushort Port { get; set; }

    [DefaultValue("No user")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnUserName, TypeName = "NVARCHAR(128)")]
    public string UserName { get; set; } = default!;

    [DefaultValue("No password")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnPassword, TypeName = "NVARCHAR(128)")]
    public string Password { get; set; } = default!;

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnSecret, TypeName = "NVARCHAR(128)")]
    public string Secret { get; set; } = default!;

    [NotMapped] 
    public ICollection<TgEfAppEntity> Apps { get; set; } = default!;

	public TgEfProxyEntity() : base()
    {
        Default();
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{TgEfConstants.TableProxies} | {base.ToDebugString()} | {Type} | {HostName} | {Port} | {UserName} | {Password} | " +
        $"{TgCommonUtils.GetIsFlag(!string.IsNullOrEmpty(Secret), Secret, "<No secret>")}";

    public override void Default()
    {
	    base.Default();
	    Type = this.GetDefaultPropertyGeneric<TgEnumProxyType>(nameof(Type));
	    HostName = this.GetDefaultPropertyString(nameof(HostName));
	    Port = this.GetDefaultPropertyUshort(nameof(Port));
	    UserName = this.GetDefaultPropertyString(nameof(UserName));
	    Password = this.GetDefaultPropertyString(nameof(Password));
	    Secret = this.GetDefaultPropertyString(nameof(Secret));
	    Apps = new List<TgEfAppEntity>();
    }

    public override void Fill(object item)
    {
	    if (item is not TgEfProxyEntity proxy)
		    throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfProxyEntity)}!");

	    Type = proxy.Type;
	    HostName = proxy.HostName;
	    Port = proxy.Port;
	    UserName = proxy.UserName;
	    Password = proxy.Password;
	    Secret = proxy.Secret;
    }

    public override void Backup(object item)
    {
	    Fill(item);
	    base.Backup(item);
    }

	#endregion
}