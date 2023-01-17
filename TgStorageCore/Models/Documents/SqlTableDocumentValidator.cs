// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using FluentValidation;

namespace TgStorageCore.Models.Documents;

[DebuggerDisplay("{nameof(TableDocumentValidator)}")]
public class SqlTableDocumentValidator : AbstractValidator<SqlTableDocumentModel>
{
    #region Public and private fields, properties, constructor

    public SqlTableDocumentValidator()
    {
        RuleFor(item => item.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        RuleFor(item => item.MessageId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0);
        RuleFor(item => item.SourceId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0);
        RuleFor(item => item.FileName)
                .NotNull();
        RuleFor(item => item.FileSize)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        RuleFor(item => item.AccessHash)
                .NotNull()
                .GreaterThanOrEqualTo(0);
    }

    #endregion
}