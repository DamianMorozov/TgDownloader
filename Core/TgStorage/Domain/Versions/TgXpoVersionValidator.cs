// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgXpoVersionValidator : TgXpoTableValidatorBase<TgXpoVersionEntity>
{
	#region Public and private fields, properties, constructor

	public TgXpoVersionValidator()
	{
		RuleFor(item => item.Version)
			.NotNull()
			.GreaterThanOrEqualTo((short)0);
		RuleFor(item => item.Description)
			.NotNull()
			.NotEmpty();
	}

	#endregion
}