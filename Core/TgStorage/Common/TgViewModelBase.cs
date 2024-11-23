// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary> Base class for TgMvvmModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public abstract partial class TgViewModelBase : ObservableRecipient, ITgViewModelBase
{
    #region Public and private fields, properties, constructor

    [ObservableProperty]
	private bool _isLoad;

    #endregion

    #region Public and private methods

    public virtual string ToDebugString() => $"{TgCommonUtils.GetIsLoad(IsLoad)}";

    #endregion
}