// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperItemTests
{
	#region Public and private methods

	[Test]
	public void TgStorage_GetTableModels_DoesNotThrow()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableBase> sqlTables = TgStorageTestsUtils.DataCore.ContextManager.GetTableModels();
			foreach (TgSqlTableBase sqlTable in sqlTables)
			{
				TestContext.WriteLine(sqlTable.GetType());
			}
		});
	}

	private TgSqlTableBase NewItem<T>() where T : TgSqlTableBase, new()
	{
		TestContext.WriteLine($"AddItem for {typeof(T)}");
		TgSqlTableBase item = new();
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgSqlTableAppModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableApps.NewItem();
				break;
			case var cls when cls == typeof(TgSqlTableDocumentModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableDocuments.NewItem();
				break;
			case var cls when cls == typeof(TgSqlTableFilterModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableFilters.NewItem();
				break;
			case var cls when cls == typeof(TgSqlTableMessageModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableMessages.NewItem();
				break;
			case var cls when cls == typeof(TgSqlTableProxyModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableProxies.NewItem();
				break;
			case var cls when cls == typeof(TgSqlTableSourceModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableSources.NewItem();
				break;
			case var cls when cls == typeof(TgSqlTableVersionModel):
				item = TgStorageTestsUtils.DataCore.ContextManager.ContextTableVersions.NewItem();
				break;
		}
		TestContext.WriteLine(item);
		return item;
	}

	[Test]
	public void TgStorage_NewItem_DoesNotThrow()
	{
		Assert.DoesNotThrow(() =>
		{
			List<Type> sqlTypes = TgStorageTestsUtils.DataCore.ContextManager.GetTableTypes();
			foreach (Type sqlType in sqlTypes)
			{
				switch (sqlType)
				{
					case var cls when cls == typeof(TgSqlTableAppModel):
						if (NewItem<TgSqlTableAppModel>() is TgSqlTableAppModel app)
							TestContext.WriteLine(app);
						break;
					case var cls when cls == typeof(TgSqlTableDocumentModel):
						if (NewItem<TgSqlTableDocumentModel>() is TgSqlTableDocumentModel doc)
							TestContext.WriteLine(doc);
						break;
					case var cls when cls == typeof(TgSqlTableFilterModel):
						if (NewItem<TgSqlTableFilterModel>() is TgSqlTableFilterModel filter)
							TestContext.WriteLine(filter);
						break;
					case var cls when cls == typeof(TgSqlTableMessageModel):
						if (NewItem<TgSqlTableMessageModel>() is TgSqlTableMessageModel message)
							TestContext.WriteLine(message);
						break;
					case var cls when cls == typeof(TgSqlTableProxyModel):
						if (NewItem<TgSqlTableProxyModel>() is TgSqlTableProxyModel proxy)
							TestContext.WriteLine(proxy);
						break;
					case var cls when cls == typeof(TgSqlTableSourceModel):
						if (NewItem<TgSqlTableSourceModel>() is TgSqlTableSourceModel source)
							TestContext.WriteLine(source);
						break;
					case var cls when cls == typeof(TgSqlTableVersionModel):
						if (NewItem<TgSqlTableVersionModel>() is TgSqlTableVersionModel version)
							TestContext.WriteLine(version);
						break;
				}
				TestContext.WriteLine();
			}
		});
	}

	[Test]
	public void TgStorage_GetItem_DoesNotThrow()
	{
		Assert.DoesNotThrow(() =>
		{
			List<Type> sqlTypes = TgStorageTestsUtils.DataCore.ContextManager.GetTableTypes();
			foreach (Type sqlType in sqlTypes)
			{
				switch (sqlType)
				{
					case var cls when cls == typeof(TgSqlTableAppModel):
						if (NewItem<TgSqlTableAppModel>() is TgSqlTableAppModel app)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableApps.GetItem(app);
						break;
					case var cls when cls == typeof(TgSqlTableDocumentModel):
						if (NewItem<TgSqlTableDocumentModel>() is TgSqlTableDocumentModel doc)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableDocuments.GetItem(doc);
						break;
					case var cls when cls == typeof(TgSqlTableFilterModel):
						if (NewItem<TgSqlTableFilterModel>() is TgSqlTableFilterModel filter)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableFilters.GetItem(filter);
						break;
					case var cls when cls == typeof(TgSqlTableMessageModel):
						if (NewItem<TgSqlTableMessageModel>() is TgSqlTableMessageModel message)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableMessages.GetItem(message);
						break;
					case var cls when cls == typeof(TgSqlTableProxyModel):
						if (NewItem<TgSqlTableProxyModel>() is TgSqlTableProxyModel proxy)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableProxies.GetItem(proxy);
						break;
					case var cls when cls == typeof(TgSqlTableSourceModel):
						if (NewItem<TgSqlTableSourceModel>() is TgSqlTableSourceModel source)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableSources.GetItem(source);
						break;
					case var cls when cls == typeof(TgSqlTableVersionModel):
						if (NewItem<TgSqlTableVersionModel>() is TgSqlTableVersionModel version)
							TgStorageTestsUtils.DataCore.ContextManager.ContextTableVersions.GetItem(version);
						break;
				}
				TestContext.WriteLine();
			}
		});
	}

	#endregion
}