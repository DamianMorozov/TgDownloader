// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Sources;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableSources)]
public sealed class TgEfSourceEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnId)]
    [SQLite.Indexed]
    public long Id{ get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(256)]
    [Column(TgSqlConstants.ColumnUserName)]
    [SQLite.Indexed]
    public string UserName { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(1024)]
    [Column(TgSqlConstants.ColumnTitle)]
    [SQLite.Indexed]
    public string Title { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnAbout)]
    public string About { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnCount)]
    [SQLite.Indexed]
    public int Count { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(1024)]
    [Column(TgSqlConstants.ColumnDirectory)]
    [SQLite.Indexed]
    public string Directory { get; set; }

    [DefaultValue(1)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnFirstId)]
    [SQLite.Indexed]
    public int FirstId { get; set; }

    [DefaultValue(false)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnIsAutoUpdate)]
    [SQLite.Indexed]
    public bool IsAutoUpdate { get; set; }

    [DefaultValue("0001-01-01 00:00:00")]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnDtChanged)]
    [SQLite.Indexed]
    public DateTime DtChanged { get; set; }

    public TgEfSourceEntity() : base()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        UserName = this.GetPropertyDefaultValue(nameof(UserName));
        Title = this.GetPropertyDefaultValue(nameof(Title));
        About = this.GetPropertyDefaultValue(nameof(About));
        Count = this.GetPropertyDefaultValueAsGeneric<int>(nameof(Count));
        Directory = this.GetPropertyDefaultValue(nameof(Directory));
        FirstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(FirstId));
        IsAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
        DtChanged = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(DtChanged));
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {Id} | {(IsAutoUpdate ? "a" : " ")} | {(FirstId == Count ? "v" : "x")} | {UserName} | " +
        $"{TgDataFormatUtils.TrimStringEnd(Title)} | {FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

    #endregion
}