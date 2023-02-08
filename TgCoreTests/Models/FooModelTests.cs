// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Models;

namespace TgCoreTests.Models;

[TestFixture]
internal class FooModelTests
{
    #region Public and private methods

    [Test]
    public void FooModel_Ushort_AreEqual()
    {
        Assert.DoesNotThrow(() =>
        {
            FooModel foo = new();
            Assert.AreEqual(123, foo.ValueUshort);
            Assert.AreEqual(324, foo.ValueUint);
        });
    }

    #endregion
}