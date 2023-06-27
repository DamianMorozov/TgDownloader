// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Models;

/// <summary>
/// Wrapper for exception.
/// </summary>
public class TgExceptionModel : TgMvvmBase
{
	#region Public and private fields, properties, constructor

	private Exception? _exception;

	private Exception? Exception
	{
		get => _exception;
		set
		{
			_exception = value;
			Message = value is null ? string.Empty : GetInnerException(value);
			SetProperty(ref _exception, value);
		}
	}
	public bool IsExists => Exception is not null;
	public string Message { get; private set; }

	public TgExceptionModel()
	{
		Message = string.Empty;
		Exception = null;
	}

	#endregion

	#region Public and private methods

	public void Set(Exception ex)
	{
		Exception = ex;
	}

	public void Clear()
	{
		Exception = null;
	}

	private string GetInnerException(Exception ex) => ex.InnerException is null 
		? ex.Message : GetInnerException(ex.InnerException);

	#endregion
}