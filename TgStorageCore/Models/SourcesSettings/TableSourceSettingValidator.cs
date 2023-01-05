// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using FluentValidation;

namespace TgStorageCore.Models.SourcesSettings;

[DebuggerDisplay("{nameof(TableSourceSettingValidator)}")]
public class TableSourceSettingValidator : AbstractValidator<TableSourceSettingModel>
{
    #region Public and private fields, properties, constructor

    public TableSourceSettingValidator()
    {
        //RuleFor(item => item.Id)
        //        .NotEmpty()
        //        .NotNull()
        //        .NotEqual(0);
        RuleFor(item => item.SourceId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0);
        RuleFor(item => item.Directory)
                .NotEmpty()
                .NotNull();
    }

    #endregion
}