// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgXpoRepositoryGetTests : TgDbContextTestsBase
{
    #region Public and private methods

    private void RepositoryGet<T>(ITgXpoRepository<T> repository, int count = 10) where T : XPLiteObject, ITgDbEntity, new()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            List<T> items = (await repository.GetEnumerableAsync(TgEnumTableTopRecords.Top100)).Items.ToList();
            TestContext.WriteLine($"Found {items.Count} items.");
            int i = 0;
            foreach (T item in items)
            {
                if (i > count)
                    break;
                i++;
                T itemFind = (await repository.GetAsync(item)).Item;
                Assert.That(item, Is.EqualTo(itemFind));
                TestContext.WriteLine(item.ToDebugString());
            }
        });
    }

    [Test]
    public void TgStorage_get_apps() => RepositoryGet(XpoProdContext.AppRepository);

    [Test]
    public void TgStorage_get_documents() => RepositoryGet(XpoProdContext.DocumentRepository);

    [Test]
    public void TgStorage_get_filters() => RepositoryGet(XpoProdContext.FilterRepository);

    [Test]
    public void TgStorage_get_messages() => RepositoryGet(XpoProdContext.MessageRepository, 1);

    [Test]
    public void TgStorage_get_proxies() => RepositoryGet(XpoProdContext.ProxyRepository);

    [Test]
    public void TgStorage_get_sources() => RepositoryGet(XpoProdContext.SourceRepository);

    [Test]
    public void TgStorage_get_versions() => RepositoryGet(XpoProdContext.VersionRepository);

    #endregion
}