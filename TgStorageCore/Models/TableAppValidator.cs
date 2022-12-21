// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using FluentValidation;
using System.Diagnostics;

namespace TgStorageCore.Models;

[DebuggerDisplay("Type = {nameof(TableAppValidator)}")]
public class TableAppValidator : AbstractValidator<TableAppModel>
{
    #region Public and private fields, properties, constructor

    public TableAppValidator()
    {
        RuleFor(item => item.ApiHash)
                .NotEmpty()
                .NotNull();
        RuleFor(item => item.PhoneNumber)
                .NotEmpty()
                .NotNull();
    }

    #endregion
}