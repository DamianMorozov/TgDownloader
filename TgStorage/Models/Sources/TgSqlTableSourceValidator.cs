// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Sources;

[DebuggerDisplay("{nameof(SqlTableSourceValidator)}")]
public sealed class TgSqlTableSourceValidator : TgSqlTableValidatorBase<TgSqlTableSourceModel>
{
    #region Public and private fields, properties, constructor

    public TgSqlTableSourceValidator()
    {
        RuleFor(item => item.Id)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        RuleFor(item => item.UserName)
                .NotNull();
        RuleFor(item => item.Title)
                .NotNull();
        RuleFor(item => item.About)
                .NotNull();
		RuleFor(item => item.Count)
				.NotNull()
				.GreaterThanOrEqualTo(0);
		RuleFor(item => item.Directory)
				.NotNull();
		RuleFor(item => item.FirstId)
			.NotNull()
			.GreaterThanOrEqualTo(0);
		RuleFor(item => item.IsAutoUpdate)
			.NotNull();
	}

	#endregion
}