// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgInfrastructure.License;

public sealed partial class TgLicensePremium : TgLicensePaid
{
	#region Public and private fields, properties, constructor

	public bool PrioritySupport { get; set; }

	#endregion

	#region Public and private methods

	public override bool IsValid()
	{
		return false;
	}

	#endregion
}
