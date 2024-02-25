// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Common;

[Table(TgSqlConstants.TableApps)]
public class TgEfRepositoryBase(TgEfContext context) : TgCommonBase
{
    #region Public and private fields, properties, constructor

    protected TgEfContext Context { get; private set; } = context;

    #endregion
}