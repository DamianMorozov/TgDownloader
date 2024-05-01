// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary> Base class for TgMvvmModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public abstract class TgViewModelBase : ObservableObject, ITgViewModelBase
{
    #region Public and private fields, properties, constructor

	public bool IsLoad { get; set; }

    #endregion

    #region Public and private methods

    public virtual string ToDebugString() => $"{TgCommonUtils.GetIsLoad(IsLoad)}";

    #endregion
}