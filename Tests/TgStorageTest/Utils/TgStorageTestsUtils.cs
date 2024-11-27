// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

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
			using var efContext = CreateEfContext();
			TgEfVersionRepository versionRepository = new(efContext);
			versionRepository.FillTableVersionsAsync().GetAwaiter().GetResult();

			var versionLast = versionRepository.GetLastVersionAsync().GetAwaiter().GetResult();

			Assert.That(versionLast.Version == versionRepository.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			var versions = versionRepository.GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true)
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
		});
	}

	[Test]
	public void Check_version_last_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			await using var efContext = CreateEfContext();
			TgEfVersionRepository versionRepository = new(efContext);
			await versionRepository.FillTableVersionsAsync();

			var versionLast = await versionRepository.GetLastVersionAsync();

			Assert.That(versionLast.Version == versionRepository.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			var versions = (await versionRepository.GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: true))
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count()} items");
		});
	}

	[Test]
	public void Check_version_last_with_add_default()
	{
		Assert.DoesNotThrow(() =>
		{
			using var efContext = CreateEfContext();
			TgEfVersionRepository versionRepository = new(efContext);
			versionRepository.FillTableVersionsAsync().GetAwaiter().GetResult();

			versionRepository.CreateNew();
			var versionLast = versionRepository.GetLastVersionAsync().GetAwaiter().GetResult();

			Assert.That(versionLast.Version == versionRepository.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			List<TgEfVersionEntity> versions = versionRepository.GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true)
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
		});
	}

	[Test]
	public void Check_version_last_with_add_default_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			await using var efContext = CreateEfContext();
			TgEfVersionRepository versionRepository = new(efContext);
			await versionRepository.FillTableVersionsAsync();

			await versionRepository.CreateNewAsync();
			var versionLast = await versionRepository.GetLastVersionAsync();

			Assert.That(versionLast.Version == versionRepository.LastVersion);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			var versions = (await versionRepository.GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: true))
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
		});
	}

	[Test]
	public void Check_version_last_when_only_default()
	{
		Assert.DoesNotThrow(() =>
		{
			using var efContext = CreateEfContext();
			TgEfVersionRepository versionRepository = new(efContext);
			versionRepository.FillTableVersionsAsync().GetAwaiter().GetResult();

			versionRepository.DeleteAll();
			versionRepository.CreateNew();
			var versionLast = versionRepository.GetLastVersionAsync().GetAwaiter().GetResult();

			Assert.That(versionLast.Version == new TgEfVersionEntity().Version);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			var versions = versionRepository.GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true)
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
			versionRepository.DeleteNew();
			versionRepository.FillTableVersionsAsync().GetAwaiter().GetResult();
		});
	}
	
	[Test]
	public void Check_version_last_when_only_default_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			await using var efContext = CreateEfContext();
			TgEfVersionRepository versionRepository = new(efContext);
			await versionRepository.FillTableVersionsAsync();

			await versionRepository.DeleteAllAsync();
			await versionRepository.CreateNewAsync();
			var versionLast = await versionRepository.GetLastVersionAsync();

			Assert.That(versionLast.Version == new TgEfVersionEntity().Version);
			TestContext.WriteLine($"{nameof(versionLast)}.{nameof(versionLast.Version)}: {versionLast.Version}");
			var versions = (await versionRepository.GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: true))
				.Items.ToList();
			TestContext.WriteLine($"Found {versions.Count} items");
			await versionRepository.DeleteNewAsync();
			await versionRepository.FillTableVersionsAsync();
		});
	}

	#endregion
}