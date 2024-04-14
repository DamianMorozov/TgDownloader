// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary>
/// Base class for TgMvvmModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public abstract class TgViewModelBase : ObservableObject, ITgViewModelBase
{
    #region Public and private fields, properties, constructor

    protected TgEfContext EfContext { get; } = default!;
    protected TgXpoContext XpoContext { get; } = new(TgEnumStorageType.Prod);
	public bool IsLoad { get; set; }
    public bool IsNotLoad => !IsLoad;

    protected TgViewModelBase()
    {
		EfContext = TgStorageUtils.GetEfContextProd();
	}

    #endregion

    #region Public and private methods

    public virtual string ToDebugString() => $"{TgCommonUtils.GetIsLoad(IsLoad)}";

    #endregion
}