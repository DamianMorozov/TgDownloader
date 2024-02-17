// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Downloads;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSqlTableDownloadViewModel : TgViewModelBase
{
    #region Public and private fields, properties, constructor

    public TgDownloadSettingsModel DownloadSetting { get; } = new();

    #endregion

    #region Public and private methods

    public override string ToDebugString() => $"{base.ToDebugString()} | {DownloadSetting.ToDebugString()}";

    #endregion
}