// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Common;

/// <summary>
/// Base class for page.
/// </summary>
public partial class TgBasePage<T> : UiPage where T : TgBaseViewModel, INotifyPropertyChanged
{
	//#region INotifyPropertyChanged

	//public event PropertyChangedEventHandler? PropertyChanged;

	//private void OnPropertyChanged([CallerMemberName] string memberName = "")
	//{
	//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
	//}

	//protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
	//{
	//	PropertyChanged?.Invoke(this, eventArgs);
	//}

	//#endregion

	#region Public and private fields, properties, constructor

	public T BaseViewModel { get; set; }
	//public static EnumToBooleanConverter EnumToBooleanConverter { get; set; }

	public TgBasePage(T viewModel)
	{
		BaseViewModel = viewModel;
	}

	#endregion
}