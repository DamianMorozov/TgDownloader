// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

public static class TgEfHelper
{
	public static string GetStringForFilterType(this TgEfFilterEntity entity) => entity.FilterType switch
	{
		TgEnumFilterType.SingleName => TgLocaleHelper.Instance.MenuFiltersSetSingleName,
		TgEnumFilterType.SingleExtension => TgLocaleHelper.Instance.MenuFiltersSetSingleExtension,
		TgEnumFilterType.MultiName => TgLocaleHelper.Instance.MenuFiltersSetMultiName,
		TgEnumFilterType.MultiExtension => TgLocaleHelper.Instance.MenuFiltersSetMultiExtension,
		TgEnumFilterType.MinSize => TgLocaleHelper.Instance.MenuFiltersSetMinSize,
		TgEnumFilterType.MaxSize => TgLocaleHelper.Instance.MenuFiltersSetMaxSize,
		_ => $"<{TgLocaleHelper.Instance.MenuFiltersError}>",
	};

	public static string GetDtAsDateString(DateTime dt) => $"{dt:yyyy-MM-dd}";

	public static TgEfSourceEntity ConvertToEntity(this TgEfSourceDto sourceDto) => new TgEfSourceEntity()
	{
		Uid = sourceDto.Uid,
		DtChanged = sourceDto.SourceDtChanged,
		Id = sourceDto.Id,
		AccessHash = sourceDto.AccessHash,
		IsActive = sourceDto.IsSourceActive,
		UserName = sourceDto.UserName,
		Title = sourceDto.Title,
		About = sourceDto.About,
		FirstId = sourceDto.FirstId,
		Count = sourceDto.Count,
		Directory = sourceDto.Directory,
		IsAutoUpdate = sourceDto.IsAutoUpdate,
	};

	public static TgEfSourceDto ConvertToDto(this TgEfSourceEntity sourceEntity) => new()
	{
		Uid = sourceEntity.Uid,
		Id = sourceEntity.Id,
		UserName = sourceEntity.UserName ?? string.Empty,
		DtChanged = GetDtAsDateString(sourceEntity.DtChanged),
		IsSourceActive = sourceEntity.IsActive,
		IsAutoUpdate = sourceEntity.IsAutoUpdate,
		Title = sourceEntity.Title ?? string.Empty,
		FirstId = sourceEntity.FirstId,
		Count = sourceEntity.Count,
	};
}
