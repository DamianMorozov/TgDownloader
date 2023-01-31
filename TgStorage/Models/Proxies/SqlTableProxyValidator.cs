// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Proxies;

[DebuggerDisplay("{nameof(SqlTableProxyValidator)}")]
public class SqlTableProxyValidator : SqlTableXpLiteValidatorBase<SqlTableProxyModel>
{
    #region Public and private fields, properties, constructor

    public SqlTableProxyValidator()
    {
        RuleFor(item => item.Type)
                .NotNull();
        RuleFor(item => item.HostName)
                .NotEmpty()
                .NotNull();
        RuleFor(item => item.Port)
                .NotEmpty()
                .NotNull()
            .GreaterThan(ushort.MinValue)
                .LessThan(ushort.MaxValue);
    }

    #endregion
}