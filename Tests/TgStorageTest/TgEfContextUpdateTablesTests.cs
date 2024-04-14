// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest;

[TestFixture]
internal class TgEfContextUpdateTablesTests : TgDbContextTestsBase
{
	#region Public and private methods

	// TODO: fix here
	[Test]
	public void Change_table_apps()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfAppEntity> operResult = await EfProdContext.AppRepository.GetFirstAsync(isNoTracking: false);
			if (operResult.IsExist)
			{
				// Backup.
				string phoneNumber = operResult.Item.PhoneNumber;
				// Update.
				operResult.Item.PhoneNumber = "+1234567890";
				operResult = await EfProdContext.AppRepository.SaveAsync(operResult.Item);
				Assert.That(operResult.IsExist);
				// Restore backup.
				operResult.Item.PhoneNumber = phoneNumber;
				operResult = await EfProdContext.AppRepository.SaveAsync(operResult.Item);
				Assert.That(operResult.IsExist);
			}
		});
	}

	[Test]
	public void Change_table_documents()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			
		});
	}

	[Test]
	public void Change_table_filters()
	{
		Assert.DoesNotThrowAsync(async () =>
		{

		});
	}

	[Test]
	public void Change_table_messages()
	{
		Assert.DoesNotThrowAsync(async () =>
		{

		});
	}

	[Test]
	public void Change_table_proxies()
	{
		Assert.DoesNotThrowAsync(async () =>
		{

		});
	}

	[Test]
	public void Change_table_sources()
	{
		Assert.DoesNotThrowAsync(async () =>
		{

		});
	}

	[Test]
	public void Change_table_versions()
	{
		Assert.DoesNotThrowAsync(async () =>
		{

		});
	}

	#endregion
}