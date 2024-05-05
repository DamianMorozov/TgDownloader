// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Utils;

[TestFixture]
internal sealed class TgStorageTestsUtils : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void Check_version_last()
	{
		Assert.DoesNotThrow(() =>
		{
			using TgEfContext efContext = CreateEfContext();
			TgEfUtils.FillTableVersions(efContext);
			TgEfVersionRepository versionRepository = new(efContext);

			TgEfVersionEntity versionLast = TgEfUtils.GetLastVersion(efContext);

			Assert.That(versionLast.Version == TgEfUtils.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			List<TgEfVersionEntity> versions = versionRepository.GetList(TgEnumTableTopRecords.All, isNoTracking: true)
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
		});
	}

	[Test]
	public void Check_version_last_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			await using TgEfContext efContext = CreateEfContext();
			await TgEfUtils.FillTableVersionsAsync(efContext);
			TgEfVersionRepository versionRepository = new(efContext);

			TgEfVersionEntity versionLast = TgEfUtils.GetLastVersion(efContext);

			Assert.That(versionLast.Version == TgEfUtils.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			List<TgEfVersionEntity> versions = (await versionRepository.GetListAsync(TgEnumTableTopRecords.All, isNoTracking: true))
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count()} items");
		});
	}

	[Test]
	public void Check_version_last_with_add_default()
	{
		Assert.DoesNotThrow(() =>
		{

			using TgEfContext efContext = CreateEfContext();
			TgEfUtils.FillTableVersions(efContext);
			TgEfVersionRepository versionRepository = new(efContext);

			versionRepository.CreateNew();
			TgEfVersionEntity versionLast = TgEfUtils.GetLastVersion(efContext);

			Assert.That(versionLast.Version == TgEfUtils.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			List<TgEfVersionEntity> versions = versionRepository.GetList(TgEnumTableTopRecords.All, isNoTracking: true)
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
		});
	}

	[Test]
	public void Check_version_last_with_add_default_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			await using TgEfContext efContext = CreateEfContext();
			await TgEfUtils.FillTableVersionsAsync(efContext);
			TgEfVersionRepository versionRepository = new(efContext);

			await versionRepository.CreateNewAsync();
			TgEfVersionEntity versionLast = TgEfUtils.GetLastVersion(efContext);

			Assert.That(versionLast.Version == TgEfUtils.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			List<TgEfVersionEntity> versions = (await versionRepository.GetListAsync(TgEnumTableTopRecords.All, isNoTracking: true))
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
		});
	}

	#endregion
}