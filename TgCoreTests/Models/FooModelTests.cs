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
            TestContext.WriteLine($"{nameof(foo.ValueUshort)}: {foo.ValueUshort}");
            Assert.That(Equals((ushort)123, foo.ValueUshort));
			TestContext.WriteLine($"{nameof(foo.ValueUint)}: {foo.ValueUint}");
			Assert.That(Equals((uint)324, foo.ValueUint));
        });
    }

    #endregion
}