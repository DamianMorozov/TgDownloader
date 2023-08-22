// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgRepositoryGetTests
{
    #region Public and private methods

    private void RepositoryGet<T>(ITgSqlRepository<T> repository, int count = 10) where T : ITgSqlTable, new()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<T> items = repository.GetEnumerable(1_000).ToList();
            TestContext.WriteLine($"Found {items.Count()} items.");
            int i = 0;
            foreach (T item in items)
            {
                if (i > count)
                    break;
                i++;
                T itemFind = repository.Get(item);
                Assert.That(item, Is.EqualTo(itemFind));
                TestContext.WriteLine(item.ToDebugString());
            }
        });
    }

    [Test]
    public void TgStorage_get_apps() => RepositoryGet(TgSqlTableAppRepository.Instance);

    [Test]
    public void TgStorage_get_documents() => RepositoryGet(TgSqlTableDocumentRepository.Instance);

    [Test]
    public void TgStorage_get_filters() => RepositoryGet(TgSqlTableFilterRepository.Instance);

    [Test]
    public void TgStorage_get_messages() => RepositoryGet(TgSqlTableMessageRepository.Instance);

    [Test]
    public void TgStorage_get_proxies() => RepositoryGet(TgSqlTableProxyRepository.Instance);

    [Test]
    public void TgStorage_get_sources() => RepositoryGet(TgSqlTableSourceRepository.Instance);

    [Test]
    public void TgStorage_get_versions() => RepositoryGet(TgSqlTableVersionRepository.Instance);

    #endregion
}