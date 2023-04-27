// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgMenuFilter SetMenuFilters()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgConstants.MenuMainReturn,
					TgConstants.MenuFiltersView,
					TgConstants.MenuFiltersAdd,
					TgConstants.MenuFiltersEdit,
					TgConstants.MenuFiltersRemove,
					TgConstants.MenuFiltersReset
				));
		return prompt switch
		{
			TgConstants.MenuFiltersView => TgMenuFilter.FiltersView,
			TgConstants.MenuFiltersAdd => TgMenuFilter.FiltersAdd,
			TgConstants.MenuFiltersEdit => TgMenuFilter.FiltersEdit,
			TgConstants.MenuFiltersRemove => TgMenuFilter.FiltersRemove,
			TgConstants.MenuFiltersReset => TgMenuFilter.FiltersReset,
			_ => TgMenuFilter.Return
		};
	}

	public void SetupFilters(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgMenuFilter menu;
		do
		{
			ShowTableFiltersSettings(tgDownloadSettings);
			menu = SetMenuFilters();
			switch (menu)
			{
				case TgMenuFilter.FiltersView:
					TgFiltersView();
					break;
				case TgMenuFilter.FiltersReset:
					SetTgFiltersReset();
					break;
				case TgMenuFilter.FiltersAdd:
					SetTgFiltersAdd();
					break;
				case TgMenuFilter.FiltersEdit:
					SetTgFiltersEdit();
					break;
				case TgMenuFilter.FiltersRemove:
					SetTgFiltersRemove();
					break;
			}
		} while (menu is not TgMenuFilter.Return);
	}

	private void TgFiltersView()
	{
		ContextManager.FiltersView();
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void SetTgFiltersAdd()
	{
		TgSqlTableFilterModel filter = ContextManager.Filters.NewItem();
		string type = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(TgConstants.MenuFiltersSetType)
			.PageSize(10)
			.AddChoices(TgConstants.MenuMainReturn, TgConstants.MenuFiltersSetSingleName, TgConstants.MenuFiltersSetSingleExtension,
				TgConstants.MenuFiltersSetMultiName, TgConstants.MenuFiltersSetMultiExtension,
				TgConstants.MenuFiltersSetMinSize, TgConstants.MenuFiltersSetMaxSize));
		if (Equals(type, TgConstants.MenuMainReturn)) return;

		//filter.IsActive = AskQuestionReturnPositive(TgConstants.MenuFiltersSetIsActive, true);
		filter.IsEnabled = true;
		filter.Name = AnsiConsole.Ask<string>(TgLog.GetMarkupString($"{TgConstants.MenuFiltersSetName}:"));
		switch (type)
		{
			case "Single name":
				filter.FilterType = TgFilterType.SingleName;
				break;
			case "Single extension":
				filter.FilterType = TgFilterType.SingleExtension;
				break;
			case "Multi name":
				filter.FilterType = TgFilterType.MultiName;
				break;
			case "Multi extension":
				filter.FilterType = TgFilterType.MultiExtension;
				break;
			case "File minimum size":
				filter.FilterType = TgFilterType.MinSize;
				break;
			case "File maximum size":
				filter.FilterType = TgFilterType.MaxSize;
				break;
		}
		switch (filter.FilterType)
		{
			case TgFilterType.SingleName:
			case TgFilterType.SingleExtension:
			case TgFilterType.MultiName:
			case TgFilterType.MultiExtension:
				filter.Mask = AnsiConsole.Ask<string>(TgLog.GetMarkupString($"{TgConstants.MenuFiltersSetMask}:"));
				break;
			case TgFilterType.MinSize:
				SetFilterSize(filter, TgConstants.MenuFiltersSetMinSize);
				break;
			case TgFilterType.MaxSize:
				SetFilterSize(filter, TgConstants.MenuFiltersSetMaxSize);
				break;
		}

		ContextManager.Filters.AddOrUpdateItem(filter);
		ContextManager.Filters.DeleteDefaultItem();
		TgFiltersView();
	}

	private void SetTgFiltersEdit()
	{
		List<TgSqlTableFilterModel> filters = ContextManager.Filters.GetList(false);
		TgSqlTableFilterModel filter = AnsiConsole.Prompt(new SelectionPrompt<TgSqlTableFilterModel>()
			.Title(TgConstants.MenuFiltersSetEnabled)
			.PageSize(10)
			.AddChoices(filters));
		filter.IsEnabled = AskQuestionReturnPositive(TgConstants.MenuFiltersSetIsEnabled, true);
		ContextManager.Filters.AddOrUpdateItem(filter);
		TgFiltersView();
	}

	private void SetFilterSize(TgSqlTableFilterModel filter, string question)
	{
		filter.SizeType = AnsiConsole.Prompt(new SelectionPrompt<TgFileSizeType>()
			.Title(TgConstants.MenuFiltersSetSizeType)
			.PageSize(5)
			.AddChoices(TgFileSizeType.Bytes, TgFileSizeType.KBytes, TgFileSizeType.MBytes, TgFileSizeType.GBytes, TgFileSizeType.TBytes));
		filter.Size = AnsiConsole.Ask<uint>(TgLog.GetMarkupString($"{question}:"));
	}

	private void SetTgFiltersRemove()
	{
		List<TgSqlTableFilterModel> filters = ContextManager.Filters.GetList(false);
		TgSqlTableFilterModel filter = AnsiConsole.Prompt(new SelectionPrompt<TgSqlTableFilterModel>()
			.Title(TgConstants.MenuFiltersSetType)
			.PageSize(10)
			.AddChoices(filters));
		ContextManager.Filters.DeleteItem(filter);
		TgFiltersView();
	}

	private void SetTgFiltersReset()
	{
		if (AskQuestionReturnNegative(TgConstants.MenuFiltersReset)) return;
		ContextManager.Filters.DeleteAllItems();
		TgFiltersView();
	}

	#endregion
}