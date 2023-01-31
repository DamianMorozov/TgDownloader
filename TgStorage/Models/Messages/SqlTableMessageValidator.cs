// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Messages;

[DebuggerDisplay("{nameof(SqlTableMessageValidator)}")]
public class SqlTableMessageValidator : AbstractValidator<SqlTableMessageModel>
{
    #region Public and private fields, properties, constructor

    public SqlTableMessageValidator()
    {
        RuleFor(item => item.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        RuleFor(item => item.SourceId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0);
        RuleFor(item => item.Message)
                .NotNull();
    }

    #endregion
}