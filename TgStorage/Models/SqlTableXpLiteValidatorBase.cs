// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

[DebuggerDisplay("{nameof(SqlTableXpLiteValidator)}")]
public class SqlTableXpLiteValidatorBase<T> : AbstractValidator<T> where T : SqlTableXpLiteBase
{
    #region Public and private fields, properties, constructor

    public SqlTableXpLiteValidatorBase()
    {
        RuleFor(item => item.Uid)
            .NotNull();
        RuleFor(item => item.DtCreated)
            .NotEmpty()
            .NotNull();
        RuleFor(item => item.DtChanged)
            .NotEmpty()
            .NotNull();
    }

    #endregion
}