// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuFilter SetMenuFilters()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuMainReturn,
					TgLocale.MenuFiltersView,
					TgLocale.MenuFiltersAdd,
					TgLocale.MenuFiltersEdit,
					TgLocale.MenuFiltersRemove,
					TgLocale.MenuFiltersReset
				));
		if (prompt.Equals(TgLocale.MenuFiltersView))
			return TgEnumMenuFilter.FiltersView;
		if (prompt.Equals(TgLocale.MenuFiltersAdd))
			return TgEnumMenuFilter.FiltersAdd;
		if (prompt.Equals(TgLocale.MenuFiltersEdit))
			return TgEnumMenuFilter.FiltersEdit;
		if (prompt.Equals(TgLocale.MenuFiltersRemove))
			return TgEnumMenuFilter.FiltersRemove;
		if (prompt.Equals(TgLocale.MenuFiltersReset))
			return TgEnumMenuFilter.FiltersReset;
		return TgEnumMenuFilter.Return;
	}

	public void SetupFilters(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgEnumMenuFilter menu;
		do
		{
			ShowTableFiltersSettings(tgDownloadSettings);
			menu = SetMenuFilters();
			switch (menu)
			{
				case TgEnumMenuFilter.FiltersView:
					TgFiltersView();
					break;
				case TgEnumMenuFilter.FiltersReset:
					SetTgFiltersReset();
					break;
				case TgEnumMenuFilter.FiltersAdd:
					SetTgFiltersAdd();
					break;
				case TgEnumMenuFilter.FiltersEdit:
					SetTgFiltersEdit();
					break;
				case TgEnumMenuFilter.FiltersRemove:
					SetTgFiltersRemove();
					break;
			}
		} while (menu is not TgEnumMenuFilter.Return);
	}

	private void TgFiltersView()
	{
		ContextManager.FiltersView();
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void SetTgFiltersAdd()
	{
		TgSqlTableFilterModel filter = TgSqlUtils.CreateNewFilter();
		string type = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(TgLocale.MenuFiltersSetType)
			.PageSize(Console.WindowHeight - 17)
			.AddChoices(TgLocale.MenuMainReturn, TgLocale.MenuFiltersSetSingleName, TgLocale.MenuFiltersSetSingleExtension,
				TgLocale.MenuFiltersSetMultiName, TgLocale.MenuFiltersSetMultiExtension,
				TgLocale.MenuFiltersSetMinSize, TgLocale.MenuFiltersSetMaxSize));
		if (Equals(type, TgLocale.MenuMainReturn))
			return;

		//filter.IsActive = AskQuestionReturnPositive(TgLocale.MenuFiltersSetIsActive, true);
		filter.IsEnabled = true;
		filter.Name = AnsiConsole.Ask<string>(TgLog.GetMarkupString($"{TgLocale.MenuFiltersSetName}:"));
		switch (type)
		{
			case "Single name":
				filter.FilterType = TgEnumFilterType.SingleName;
				break;
			case "Single extension":
				filter.FilterType = TgEnumFilterType.SingleExtension;
				break;
			case "Multi name":
				filter.FilterType = TgEnumFilterType.MultiName;
				break;
			case "Multi extension":
				filter.FilterType = TgEnumFilterType.MultiExtension;
				break;
			case "File minimum size":
				filter.FilterType = TgEnumFilterType.MinSize;
				break;
			case "File maximum size":
				filter.FilterType = TgEnumFilterType.MaxSize;
				break;
		}
		switch (filter.FilterType)
		{
			case TgEnumFilterType.SingleName:
			case TgEnumFilterType.SingleExtension:
			case TgEnumFilterType.MultiName:
			case TgEnumFilterType.MultiExtension:
				filter.Mask = AnsiConsole.Ask<string>(TgLog.GetMarkupString($"{TgLocale.MenuFiltersSetMask}:"));
				break;
			case TgEnumFilterType.MinSize:
				SetFilterSize(filter, TgLocale.MenuFiltersSetMinSize);
				break;
			case TgEnumFilterType.MaxSize:
				SetFilterSize(filter, TgLocale.MenuFiltersSetMaxSize);
				break;
		}

		ContextManager.FilterRepository.Save(filter);
		TgFiltersView();
	}

	private void SetTgFiltersEdit()
	{
		IEnumerable<TgSqlTableFilterModel> filters = ContextManager.FilterRepository.GetEnumerable();
		TgSqlTableFilterModel filter = AnsiConsole.Prompt(new SelectionPrompt<TgSqlTableFilterModel>()
			.Title(TgLocale.MenuFiltersSetEnabled)
			.PageSize(Console.WindowHeight - 17)
			.AddChoices(filters));
		filter.IsEnabled = AskQuestionReturnPositive(TgLocale.MenuFiltersSetIsEnabled, true);
		ContextManager.FilterRepository.Save(filter);
		TgFiltersView();
	}

	private void SetFilterSize(TgSqlTableFilterModel filter, string question)
	{
		filter.SizeType = AnsiConsole.Prompt(new SelectionPrompt<TgEnumFileSizeType>()
			.Title(TgLocale.MenuFiltersSetSizeType)
			.PageSize(Console.WindowHeight - 17)
			.AddChoices(TgEnumFileSizeType.Bytes, TgEnumFileSizeType.KBytes, TgEnumFileSizeType.MBytes, TgEnumFileSizeType.GBytes, TgEnumFileSizeType.TBytes));
		filter.Size = AnsiConsole.Ask<uint>(TgLog.GetMarkupString($"{question}:"));
	}

	private void SetTgFiltersRemove()
	{
		IEnumerable<TgSqlTableFilterModel> filters = ContextManager.FilterRepository.GetEnumerable();
		TgSqlTableFilterModel filter = AnsiConsole.Prompt(new SelectionPrompt<TgSqlTableFilterModel>()
			.Title(TgLocale.MenuFiltersSetType)
			.PageSize(Console.WindowHeight - 17)
			.AddChoices(filters));
		ContextManager.FilterRepository.Delete(filter);
		TgFiltersView();
	}

	private void SetTgFiltersReset()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuFiltersReset))
			return;
		ContextManager.FilterRepository.DeleteAllItems();
		TgFiltersView();
	}

	#endregion
}