//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//namespace TgStorage.Domain.Bots;

///// <summary> Bot DTO </summary>
//public sealed partial class TgEfBotDto : TgDtoBase, ITgDto<TgEfBotDto, TgEfBotEntity>
//{
//	#region Public and private fields, properties, constructor

//	[ObservableProperty]
//	public partial string BotToken { get; set; } = string.Empty;

//	#endregion

//	#region Public and private methods

//	public override string ToString() => $"{BotToken}";
	
//	public TgEfBotDto Fill(TgEfBotDto dto, bool isUidCopy)
//	{
//		if (isUidCopy)
//			Uid = dto.Uid;
//		BotToken = dto.BotToken;
//		return this;
//	}

//	public TgEfBotDto Fill(TgEfBotEntity item, bool isUidCopy)
//	{
//		if (isUidCopy)
//			Uid = item.Uid;
//		BotToken = item.BotToken;
//		return this;
//	}

//	public TgEfBotDto GetDto(TgEfBotEntity item)
//	{
//		var dto = new TgEfBotDto();
//		dto.Fill(item, isUidCopy: true);
//		return dto;
//	}

//	public TgEfBotEntity GetEntity() => new()
//	{
//		Uid = Uid,
//		BotToken = BotToken,
//	};

//	#endregion
//}
