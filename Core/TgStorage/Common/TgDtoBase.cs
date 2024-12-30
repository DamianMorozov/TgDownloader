// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary> Base class for TgMvvmModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public abstract partial class TgDtoBase : ObservableRecipient
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private bool _isLoad;
	[ObservableProperty]
	private Guid _uid;

	#endregion

	#region Public and private methods

	public string ToDebugString() => TgObjectUtils.ToDebugString(this);

	#endregion
}