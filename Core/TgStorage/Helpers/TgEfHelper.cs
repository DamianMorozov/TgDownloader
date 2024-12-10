// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

public static class TgEfHelper
{
	public static TgEfSourceEntity ConvertToEntity(this TgEfSourceDto sourceDto)
	{
		return new TgEfSourceEntity()
		{
			Uid = sourceDto.Uid,
			DtChanged = sourceDto.DtChanged,
			Id = sourceDto.Id,
			IsActive = sourceDto.IsActive,
			UserName = sourceDto.UserName,
			Title = sourceDto.Title,
			Count = sourceDto.Count,
			Directory = sourceDto.Directory,
			FirstId = sourceDto.FirstId,
			IsAutoUpdate = sourceDto.IsAutoUpdate,
		};
	}

	public static TgEfSourceDto ConvertToDto(this TgEfSourceEntity sourceEntity)
	{
		return new TgEfSourceDto()
		{
			Uid = sourceEntity.Uid,
			DtChanged = sourceEntity.DtChanged,
			Id = sourceEntity.Id,
			IsActive = sourceEntity.IsActive,
			UserName = sourceEntity.UserName,
			Title = sourceEntity.Title,
			Count = sourceEntity.Count,
			Directory = sourceEntity.Directory,
			FirstId = sourceEntity.FirstId,
			IsAutoUpdate = sourceEntity.IsAutoUpdate,
		};
	}
}
