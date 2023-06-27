﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Proxies;

[DebuggerDisplay("{ToString()}")]
[DoNotNotify]
public sealed class TgSqlTableProxyValidator : TgSqlTableValidatorBase<TgSqlTableProxyModel>
{
    #region Public and private fields, properties, constructor

    public TgSqlTableProxyValidator()
    {
        RuleFor(item => item.Type)
            .NotNull();
        RuleFor(item => item.HostName)
            .NotEmpty()
            .NotNull();
        RuleFor(item => item.Port)
            .NotNull()
            .GreaterThan(ushort.MinValue)
            .LessThan(ushort.MaxValue);
    }

    #endregion
}