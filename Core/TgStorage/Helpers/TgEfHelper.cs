// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

public static class TgEfHelper
{
	public static TgEfSourceEntity ConvertToEntity(this TgEfSourceDto sourceDto) => new TgEfSourceEntity()
	{
		Uid = sourceDto.Uid,
		DtChanged = sourceDto.DtChanged,
		Id = sourceDto.Id,
		IsActive = sourceDto.IsActive,
		UserName = sourceDto.UserName,
		Title = sourceDto.Title,
		Count = sourceDto.Count,
		FirstId = sourceDto.FirstId,
		IsAutoUpdate = sourceDto.IsAutoUpdate,
	};

	public static TgEfSourceDto ConvertToDto(this TgEfSourceEntity sourceEntity) => new TgEfSourceDto()
	{
		Uid = sourceEntity.Uid,
		Id = sourceEntity.Id,
		UserName = sourceEntity.UserName ?? string.Empty,
		DtChanged = sourceEntity.DtChanged,
		IsActive = sourceEntity.IsActive,
		IsAutoUpdate = sourceEntity.IsAutoUpdate,
		Title = sourceEntity.Title ?? string.Empty,
		FirstId = sourceEntity.FirstId,
		Count = sourceEntity.Count,
	};
}
