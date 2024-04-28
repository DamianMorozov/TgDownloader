//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//namespace TgStorage.Domain.Documents;

//[DebuggerDisplay("{ToDebugString()}")]
//[DoNotNotify]
//public sealed class TgXpoDocumentValidator : TgXpoTableValidatorBase<TgXpoDocumentEntity>
//{
//	#region Public and private fields, properties, constructor

//	public TgXpoDocumentValidator()
//	{
//		RuleFor(item => item.Id)
//				.NotNull()
//				.GreaterThanOrEqualTo(0);
//		RuleFor(item => item.MessageId)
//				.NotNull()
//				.GreaterThanOrEqualTo(0);
//		RuleFor(item => item.SourceId)
//				.NotNull()
//				.GreaterThanOrEqualTo(0);
//		RuleFor(item => item.FileName)
//				.NotNull();
//		RuleFor(item => item.FileSize)
//				.NotNull()
//				.GreaterThanOrEqualTo(0);
//		RuleFor(item => item.AccessHash)
//				.NotNull()
//				.GreaterThanOrEqualTo(0);
//	}

//	#endregion
//}