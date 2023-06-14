// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Models;

[DebuggerDisplay("{ToString()}")]
public class AppXmlSettingModel
{
	#region Public and private fields, properties, constructor

	public string Name { get; set; }
	public string Value { get; set; }

	public AppXmlSettingModel()
	{
		Name = string.Empty;
		Value = string.Empty;
	}

	public AppXmlSettingModel(string name, string value)
	{
		Name = name;
		Value = value;
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Name} | {Value}";

	#endregion
}