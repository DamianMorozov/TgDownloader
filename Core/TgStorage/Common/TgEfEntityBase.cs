﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

[DebuggerDisplay("{ToDebugString()}")]
[Index(nameof(Uid), IsUnique = true)]
public abstract class TgEfEntityBase : ITgDbEntity
{
	#region Public and private fields, properties, constructor

	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Required]
	[System.ComponentModel.DataAnnotations.Schema.Column(TgEfConstants.ColumnUid, TypeName = "CHAR(36)")]
	[SQLite.Collation("NOCASE")]
	public Guid Uid { get; set; }

	//[NotMapped]
	//public byte[] RowVersion { get; set; }
	//public static readonly string RowVersion = nameof(RowVersion);

	//[NotMapped]
	//[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	//[JsonPropertyName("@odata.etag")]
	//public string ETag
	//{
	//	get;
	//	set;
	//}

	protected TgEfEntityBase()
    {
	    Default();
    }

    #endregion

    #region Public and private methods

    public virtual string ToDebugString() => $"{Uid}";

    public virtual void Default() => Uid = this.GetDefaultPropertyGuid(nameof(Uid));

    public virtual void Fill(object item) => throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);
    
    public virtual void Backup(object item) => Uid = (item as TgEfEntityBase)!.Uid;

    #endregion
}