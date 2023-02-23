// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Versions;

[DebuggerDisplay("{nameof(SqlTableVersionValidator)}")]
public class SqlTableVersionValidator : SqlTableXpLiteValidatorBase<SqlTableVersionModel>
{
    #region Public and private fields, properties, constructor

    public SqlTableVersionValidator()
    {
        RuleFor(item => item.Version)
            .NotEmpty()
            .NotNull()
            .GreaterThan((ushort)0);
        RuleFor(item => item.Description)
                .NotEmpty()
                .NotNull();
    }

    #endregion
}