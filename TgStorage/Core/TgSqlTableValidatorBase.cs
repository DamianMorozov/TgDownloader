// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Core;

/// <summary>
/// SQL validator base.
/// </summary>
/// <typeparam name="T"></typeparam>
public class TgSqlTableValidatorBase<T> : AbstractValidator<T> where T : TgSqlTableBase
{
    #region Public and private fields, properties, constructor

    public TgSqlTableValidatorBase()
    {
        RuleFor(item => item.Uid)
            .NotNull();
    }

    #endregion
}