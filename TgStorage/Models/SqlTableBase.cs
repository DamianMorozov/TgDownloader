// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models;

public class SqlTableBase : IBase //ISqlTable
{
    #region Public and private methods - ISerializable

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlTableBase()
    {
        //
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableBase(SerializationInfo info, StreamingContext context)
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