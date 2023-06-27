// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Apps;

[DebuggerDisplay("{ToString()}")]
[DoNotNotify]
public sealed class TgSqlTableAppValidator : TgSqlTableValidatorBase<TgSqlTableAppModel>
{
    #region Public and private fields, properties, constructor

    public TgSqlTableAppValidator()
    {
        RuleFor(item => item.ApiHash)
            .NotNull();
        RuleFor(item => item.ApiId)
            .NotNull();
        RuleFor(item => item.PhoneNumber)
	        .NotEmpty()
	        .NotNull();
		RuleFor(item => item.ProxyUid)
			.NotNull();
    }

	#endregion
}