// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Utils;

namespace TgCoreTests.Models;

[TestFixture]
internal class DataFormatUtilsTests
{
	#region Public and private methods

	[Test]
	public void DataFormatUtils_CheckFileAtMask_AreEqual()
	{
		Assert.DoesNotThrow(() =>
		{
			string fileName = "RW-Kotlin-Cheatsheet-1.1.pdf";
			bool result = DataFormatUtils.CheckFileAtMask(fileName, "kotlin");
			Assert.True(result);
			result = DataFormatUtils.CheckFileAtMask(fileName, "PDF");
			Assert.True(result);

			fileName = "C# Generics.ZIP";
			result = DataFormatUtils.CheckFileAtMask(fileName, "c*#");
			Assert.True(result);
			result = DataFormatUtils.CheckFileAtMask(fileName, "zip");
			Assert.True(result);
		});
	}

	#endregion
}