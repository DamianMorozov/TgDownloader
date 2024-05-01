// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

/// <summary>
/// SQL table interface.
/// </summary>
public interface ITgDbEntity : ITgCommon
{
	#region Public and private fields, properties, constructor

	public Guid Uid { get; set; }

	#endregion

	#region Public and private methods

	public void Default();
    public void Fill(object item);
    public void Backup(object item);

    #endregion
}