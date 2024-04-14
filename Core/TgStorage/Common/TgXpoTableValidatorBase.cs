// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary>
/// SQL validator base.
/// </summary>
/// <typeparam name="T"></typeparam>
[DoNotNotify]
public class TgXpoTableValidatorBase<T> : AbstractValidator<T>, ITgCommon where T : ITgDbEntity
{
    #region Public and private fields, properties, constructor

    public TgXpoTableValidatorBase()
	{
		RuleFor(item => item.Uid)
			.NotNull();
	}

    #endregion

    #region Public and private methods

    public string ToDebugString() => this is T item ? $"{item.Uid}" : string.Empty;

    #endregion
}