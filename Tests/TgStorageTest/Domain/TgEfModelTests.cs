// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgEfModelTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void TgStorage_App_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfAppEntity app = new();
			TestContext.WriteLine(app);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(Guid.Empty, app.ApiHash));
				Assert.That(Equals(0, app.ApiId));
				Assert.That(Equals("+00000000000", app.PhoneNumber));
				Assert.That(Equals(Guid.Empty, app.ProxyUid));
			});
		});
	}

	[Test]
	public void TgStorage_Document_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfDocumentEntity document = new();
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
			TgEfFilterEntity filter = new();
			TestContext.WriteLine(filter);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(true, filter.IsEnabled));
				Assert.That(Equals(TgEnumFilterType.SingleName, filter.FilterType));
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
			TgEfMessageEntity message = new();
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
			TgEfProxyEntity proxy = new();
			TestContext.WriteLine(proxy);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(TgEnumProxyType.None, proxy.Type));
				Assert.That(Equals("No proxy", proxy.HostName));
				Assert.That(Equals((ushort)404, proxy.Port));
				Assert.That(Equals("No user", proxy.UserName));
				Assert.That(Equals("No password", proxy.Password));
				Assert.That(Equals("", proxy.Secret));
			});
		});
	}

	[Test]
	public void TgStorage_Source_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfSourceEntity source = new();
			TestContext.WriteLine(source);
			Assert.Multiple(() =>
			{
				Assert.That(Equals((long)1, source.Id));
				Assert.That(Equals("UserName", source.UserName));
				Assert.That(Equals("Title", source.Title));
				Assert.That(Equals("About", source.About));
				Assert.That(Equals(1, source.Count));
			});
		});
	}

	[Test]
	public void TgStorage_Version_Constructor()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfVersionEntity version = new();
			TestContext.WriteLine(version);
			Assert.Multiple(() =>
			{
				Assert.That(Equals(short.MaxValue, version.Version));
				Assert.That(Equals("New version", version.Description));
			});
		});
	}

	#endregion
}