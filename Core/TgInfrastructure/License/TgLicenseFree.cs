// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgInfrastructure.License;

public partial class TgLicenseFree : TgLicense
{
	#region Public and private methods

	public override bool IsValid() => true;

	#endregion
}
