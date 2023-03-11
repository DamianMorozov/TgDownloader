// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models.Filters;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
	#region Public and private methods

	private MenuFilter SetMenuFilters()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgLocale.MenuSwitchNumber)
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuMainReturn,
					TgLocale.MenuFiltersView,
					TgLocale.MenuFiltersAdd,
					TgLocale.MenuFiltersEdit,
					TgLocale.MenuFiltersRemove,
					TgLocale.MenuFiltersReset
				));
		return prompt switch
		{
			"View filters" => MenuFilter.FiltersView,
			"Add filter" => MenuFilter.FiltersAdd,
			"Edit filter" => MenuFilter.FiltersEdit,
			"Remove filter" => MenuFilter.FiltersRemove,
			"Reset filters" => MenuFilter.FiltersReset,
			_ => MenuFilter.Return
		};
	}

	public void SetupFilters(TgDownloadSettingsModel tgDownloadSettings)
	{
		MenuFilter menu;
		do
		{
			ShowTableFiltersSettings(tgDownloadSettings);
			menu = SetMenuFilters();
			switch (menu)
			{
				case MenuFilter.FiltersView:
					TgFiltersView();
					break;
				case MenuFilter.FiltersReset:
					SetTgFiltersReset();
					break;
				case MenuFilter.FiltersAdd:
					SetTgFiltersAdd();
					break;
				case MenuFilter.FiltersEdit:
					SetTgFiltersEdit();
					break;
				case MenuFilter.FiltersRemove:
					SetTgFiltersRemove();
					break;
			}
		} while (menu is not MenuFilter.Return);
	}

	private void TgFiltersView()
	{
		TgStorage.FiltersView();
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void SetTgFiltersAdd()
	{
		SqlTableFilterModel filter = TgStorage.NewEmptyFilter();
		string type = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(TgLocale.MenuFiltersSetType)
			.PageSize(10)
			.AddChoices(TgLocale.MenuMainReturn, TgLocale.MenuFiltersSetSingleName, TgLocale.MenuFiltersSetSingleExtension,
				TgLocale.MenuFiltersSetMultiName, TgLocale.MenuFiltersSetMultiExtension,
				TgLocale.MenuFiltersSetMinSize, TgLocale.MenuFiltersSetMaxSize));
		if (Equals(type, TgLocale.MenuMainReturn)) return;

		//filter.IsActive = AskQuestionReturnPositive(TgLocale.MenuFiltersSetIsActive, true);
		filter.IsEnabled = true;
		filter.Name = AnsiConsole.Ask<string>(TgLog.GetMarkupString($"{TgLocale.MenuFiltersSetName}:"));
		switch (type)
		{
			case "Single name":
				filter.FilterType = FilterType.SingleName;
				break;
			case "Single extension":
				filter.FilterType = FilterType.SingleExtension;
				break;
			case "Multi name":
				filter.FilterType = FilterType.MultiName;
				break;
			case "Multi extension":
				filter.FilterType = FilterType.MultiExtension;
				break;
			case "File minimum size":
				filter.FilterType = FilterType.MinSize;
				break;
			case "File maximum size":
				filter.FilterType = FilterType.MaxSize;
				break;
		}
		switch (filter.FilterType)
		{
			case FilterType.SingleName:
			case FilterType.SingleExtension:
			case FilterType.MultiName:
			case FilterType.MultiExtension:
				filter.Mask = AnsiConsole.Ask<string>(TgLog.GetMarkupString($"{TgLocale.MenuFiltersSetMask}:"));
				break;
			case FilterType.MinSize:
				SetFilterSize(filter, TgLocale.MenuFiltersSetMinSize);
				break;
			case FilterType.MaxSize:
				SetFilterSize(filter, TgLocale.MenuFiltersSetMaxSize);
				break;
		}

		TgStorage.AddOrUpdateItem(filter);
		TgStorage.DeleteDefaultFilter();
		TgFiltersView();
	}

	private void SetTgFiltersEdit()
	{
		List<SqlTableFilterModel> filters = TgStorage.GetFiltersList();
		SqlTableFilterModel filter = AnsiConsole.Prompt(new SelectionPrompt<SqlTableFilterModel>()
			.Title(TgLocale.MenuFiltersSetEnabled)
			.PageSize(10)
			.AddChoices(filters));
		filter.IsEnabled = AskQuestionReturnPositive(TgLocale.MenuFiltersSetIsEnabled, true);
		TgStorage.AddOrUpdateItem(filter);
		TgFiltersView();
	}

	private void SetFilterSize(SqlTableFilterModel filter, string question)
	{
		filter.SizeType = AnsiConsole.Prompt(new SelectionPrompt<FileSizeType>()
			.Title(TgLocale.MenuFiltersSetSizeType)
			.PageSize(5)
			.AddChoices(FileSizeType.Bytes, FileSizeType.KBytes, FileSizeType.MBytes, FileSizeType.GBytes, FileSizeType.TBytes));
		filter.Size = AnsiConsole.Ask<uint>(TgLog.GetMarkupString($"{question}:"));
	}

	private void SetTgFiltersRemove()
	{
		List<SqlTableFilterModel> filters = TgStorage.GetFiltersList();
		SqlTableFilterModel filter = AnsiConsole.Prompt(new SelectionPrompt<SqlTableFilterModel>()
			.Title(TgLocale.MenuFiltersSetType)
			.PageSize(10)
			.AddChoices(filters));
		TgStorage.DeleteFilter(filter);
		TgFiltersView();
	}

	private void SetTgFiltersReset()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuFiltersReset)) return;
		TgStorage.DeleteAllFilters();
		TgFiltersView();
	}

	#endregion
}