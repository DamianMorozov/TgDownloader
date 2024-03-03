// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Versions;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableVersions)]
public sealed class TgEfVersionEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(1)]
    [MaxLength(4)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnVersion)]
    [SQLite.Indexed]
    public short Version { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnDescription)]
    [SQLite.Indexed]
    public string Description { get; set; }

    public TgEfVersionEntity() : base()
    {
        Version = this.GetPropertyDefaultValueAsGeneric<short>(nameof(Version));
        Description = this.GetPropertyDefaultValue(nameof(Description));
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {Version} | {Description}";

    #endregion
}