// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;
using TgAssertCoreTests.Helpers;

namespace TgStorageTest.Utils;

[TestFixture]
internal static class TgStorageTestsUtils
{
    #region Public and private fields, properties, constructor

    internal static DataCoreHelper DataCore => DataCoreHelper.Instance;

    #endregion
}