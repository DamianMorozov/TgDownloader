// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Models;

public class ExceptionModel
{
    #region Public and private fields, properties, constructor

    public Exception? Exception { get; set; }
    public bool IsExists => Exception is not null;
    public string Message => Exception is not null ? Exception.InnerException is not null
        ? Exception.InnerException.Message + " | " + Exception.Message : Exception.Message : string.Empty;

    public ExceptionModel()
    {
        Exception = null;
    }

    #endregion

    #region Public and private methods

    public void Set(Exception ex)
    {
        Exception = ex;
    }

    #endregion
}