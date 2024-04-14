// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfMessageValidator : TgEfValidatorBase<TgEfMessageEntity>
{
	#region Public and private fields, properties, constructor

	public TgEfMessageValidator()
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