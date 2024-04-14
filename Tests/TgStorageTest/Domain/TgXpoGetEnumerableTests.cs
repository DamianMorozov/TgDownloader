// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgXpoGetEnumerableTests : TgDbContextTestsBase
{
    #region Public and private methods

    private async Task GetEnumerableAsync<T>(ITgXpoRepository<T> repository, TgEnumTableTopRecords topRecords = TgEnumTableTopRecords.Top20) 
	    where T : XPLiteObject, ITgDbEntity, new()
    {
        IEnumerable<T> items = (await repository.GetEnumerableAsync(topRecords)).Items;
        foreach (T item in items)
        {
            TestContext.WriteLine(item.ToDebugString());
        }
    }

    [Test]
    public void Get_enumerable_of_apps() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.AppRepository));

    [Test]
    public void Get_enumerable_of_documents() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.DocumentRepository, TgEnumTableTopRecords.Top1));

    [Test]
    public void Get_enumerable_of_filters() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.FilterRepository));

    [Test]
    public void Get_enumerable_of_messages() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.MessageRepository, TgEnumTableTopRecords.Top1));

    [Test]
    public void Get_enumerable_of_proxies() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.ProxyRepository));

    [Test]
    public void Get_enumerable_of_sources() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.SourceRepository));

    [Test]
    public void Get_enumerable_of_versions() => Assert.DoesNotThrowAsync(() => GetEnumerableAsync(XpoProdContext.VersionRepository));

    [Test]
    public void Check_last_table_version_after_fill()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            XpoProdContext.FillTableVersions();
            TgXpoVersionEntity versionLast =
                !XpoProdContext.IsTableExists(TgStorageConstants.TableVersions)
                ? (await XpoProdContext.VersionRepository.GetNewAsync()).Item
                : (await XpoProdContext.VersionRepository.GetItemLastAsync()).Item;
            TestContext.WriteLine(versionLast);
            Assert.That(Equals(TgEfContext.LastVersion, versionLast.Version));
        });
    }

    #endregion
}