// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Messages;

[DebuggerDisplay("{nameof(SqlTableMessageValidator)}")]
public sealed class TgSqlTableMessageValidator : TgSqlTableValidatorBase<TgSqlTableMessageModel>
{
    #region Public and private fields, properties, constructor

    public TgSqlTableMessageValidator()
    {
        RuleFor(item => item.Id)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        RuleFor(item => item.SourceId)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        RuleFor(item => item.DtCreated)
	        .NotEmpty()
	        .NotNull();
        RuleFor(item => item.Message)
                .NotNull();
    }

    #endregion
}