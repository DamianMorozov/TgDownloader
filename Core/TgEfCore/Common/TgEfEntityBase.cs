// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Common;

[DebuggerDisplay("{ToDebugString()}")]
public abstract class TgEfEntityBase : ITgSqlTable
{
    #region Public and private fields, properties, constructor

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [Key]
    [Required]
    [Column(TgSqlConstants.ColumnUid)]
    public Guid Uid { get; set; }

    [NotMapped]
	public bool IsNotExists => Equals(Uid, Guid.Empty);
	[NotMapped]
	public bool IsExists => !IsNotExists;

    protected TgEfEntityBase()
    {
        Uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
    }

    #endregion

    #region Public and private methods

    public virtual string ToDebugString() => $"{TgCommonUtils.GetIsExists(IsExists)} | {Uid}";

    #endregion
}