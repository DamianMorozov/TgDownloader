// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgLocaleCore.Interfaces;

namespace TgStorageCore.Models;

public class TableBase : ITable
{
    #region Public and private methods - ISerializable

    /// <summary>
    /// Default constructor.
    /// </summary>
    public TableBase()
    {
        //
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected TableBase(SerializationInfo info, StreamingContext context)
    {
        //
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        //
    }

    #endregion
}