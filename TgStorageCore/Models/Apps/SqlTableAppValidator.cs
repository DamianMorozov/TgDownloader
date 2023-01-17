// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using FluentValidation;

namespace TgStorageCore.Models.Apps;

[DebuggerDisplay("{nameof(TableAppValidator)}")]
public class SqlTableAppValidator : AbstractValidator<SqlTableAppModel>
{
    #region Public and private fields, properties, constructor

    public SqlTableAppValidator()
    {
        RuleFor(item => item.ApiHash)
                .NotEmpty()
                .NotNull();
        RuleFor(item => item.PhoneNumber)
                .NotEmpty()
                .NotNull();
    }

    #endregion
}