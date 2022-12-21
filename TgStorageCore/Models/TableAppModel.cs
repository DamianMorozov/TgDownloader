// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Diagnostics;

namespace TgStorageCore.Models;

[DebuggerDisplay("Type = {nameof(TableAppModel)}")]
[Table("APPS")]
public class TableAppModel
{
    #region Public and private fields, properties, constructor

    [PrimaryKey]
    [Column("API_HASH")]
    public string ApiHash { get; set; }
    [Column("PHONE_NUMBER")]
    public string PhoneNumber { get; set; }

    public TableAppModel()
    {
        ApiHash = string.Empty;
        PhoneNumber = string.Empty;
    }

    public TableAppModel(string apiHash, string phoneNumber)
    {
        ApiHash = apiHash;
        PhoneNumber = phoneNumber;
    }

    #endregion
}