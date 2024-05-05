﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgEfConstants.TableVersions)]
[Index(nameof(Version), IsUnique = true)]
[Index(nameof(Description))]
public sealed partial class TgEfVersionEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(1024)]
    [MaxLength(4)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnVersion, TypeName = "SMALLINT")]
    public short Version { get; set; }

    [DefaultValue("New version")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnDescription, TypeName = "NVARCHAR(128)")]
    public string Description { get; set; } = default!;

    public TgEfVersionEntity() : base()
    {
        Default();
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{TgEfConstants.TableVersions} | {base.ToDebugString()} | {Version} | {Description}";

    public override void Default()
    {
	    base.Default();
	    Version = this.GetDefaultPropertyShort(nameof(Version));
	    Description = this.GetDefaultPropertyString(nameof(Description));
    }

    public override void Fill(object item)
    {
		if (item is not TgEfVersionEntity version)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfVersionEntity)}!");

		Version = version.Version;
		Description = version.Description;
	}

    public override void Backup(object item)
    {
	    Fill(item);
	    base.Backup(item);
    }

	#endregion
}