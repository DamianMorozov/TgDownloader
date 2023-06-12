// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Models;

[TestFixture]
internal class TgModelTests
{
	#region Public and private methods

	[Test]
	public void TgStorage_App_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableAppModel app = new();
			TestContext.WriteLine(app);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(Guid.Empty, app.ApiHash));
				Assert.That(Equals(0, app.ApiId));
				Assert.That(Equals(string.Empty, app.PhoneNumber));
				Assert.That(Equals(Guid.Empty, app.ProxyUid));
			});
		});
	}

	[Test]
	public void TgStorage_Document_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableDocumentModel document = new();
			TestContext.WriteLine(document);
			Assert.Multiple(() =>
			{
				Assert.That(Equals((long)0, document.Id));
				Assert.That(Equals((long)0, document.SourceId));
				Assert.That(Equals((long)0, document.MessageId));
				Assert.That(Equals(string.Empty, document.FileName));
				Assert.That(Equals((long)0, document.FileSize));
				Assert.That(Equals((long)0, document.AccessHash));
			});
		});
	}

	[Test]
	public void TgStorage_Filter_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableFilterModel filter = new();
			TestContext.WriteLine(filter);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(false, filter.IsEnabled));
				Assert.That(Equals(TgEnumFilterType.None, filter.FilterType));
				Assert.That(Equals("Any", filter.Name));
				Assert.That(Equals("*", filter.Mask));
				Assert.That(Equals((long)0, filter.Size));
				Assert.That(Equals(TgEnumFileSizeType.Bytes, filter.SizeType));
			});
		});
	}

	[Test]
	public void TgStorage_Message_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableMessageModel message = new();
			TestContext.WriteLine(message);
			Assert.Multiple(() =>
			{
				Assert.That(Equals((long)0, message.Id));
				Assert.That(Equals((long)0, message.SourceId));
				Assert.That(Equals(TgEnumMessageType.Message, message.Type));
				Assert.That(Equals((long)0, message.Size));
				Assert.That(Equals(string.Empty, message.Message));
			});
		});
	}

	[Test]
	public void TgStorage_Proxy_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableProxyModel proxy = new();
			TestContext.WriteLine(proxy);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(TgEnumProxyType.None, proxy.Type));
				Assert.That(Equals(string.Empty, proxy.HostName));
				Assert.That(Equals((ushort)0, proxy.Port));
				Assert.That(Equals(string.Empty, proxy.UserName));
				Assert.That(Equals(string.Empty, proxy.Password));
				Assert.That(Equals(string.Empty, proxy.Secret));
			});
		});
	}

	[Test]
	public void TgStorage_Source_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableSourceModel source = new();
			TestContext.WriteLine(source);
			Assert.Multiple(() =>
			{
				Assert.That(Equals((long)0, source.Id));
				Assert.That(Equals(string.Empty, source.UserName));
				Assert.That(Equals(string.Empty, source.Title));
				Assert.That(Equals(string.Empty, source.About));
				Assert.That(Equals(0, source.Count));
			});
		});
	}

	[Test]
	public void TgStorage_Version_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgSqlTableVersionModel version = new();
			TestContext.WriteLine(version);
			Assert.Multiple(() =>
			{
				Assert.That(Equals((short)0, version.Version));
				Assert.That(Equals(string.Empty, version.Description));
			});
		});
	}

	#endregion
}