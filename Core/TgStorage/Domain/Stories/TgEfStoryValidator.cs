// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Stories;

[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfStoryValidator : TgEfValidatorBase<TgEfStoryEntity>
{
	#region Public and private fields, properties, constructor

	public TgEfStoryValidator()
	{
		//RuleFor(item => item.Id)
		//	.NotNull().GreaterThan(0);
	}

	#endregion
}