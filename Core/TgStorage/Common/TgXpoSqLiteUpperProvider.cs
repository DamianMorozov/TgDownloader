// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// https://supportcenter.devexpress.com/ticket/details/t1179747/with-sqlite-save-the-guids-in-an-uppercase-format

using DevExpress.Xpo.DB;

namespace TgStorage.Common;

public sealed class TgXpoSqLiteUpperProvider : SQLiteConnectionProvider
{
	public const string XpoProviderTypeStringUpper = "SQLiteUpperUid";

	public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
	{
		IDbConnection dbConnection = CreateConnection(connectionString);
		objectsToDisposeOnDisconnect = new IDisposable[1] { dbConnection };
		return CreateProviderFromConnection(dbConnection, autoCreateOption);
	}

	public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption) => 
		new TgXpoSqLiteUpperProvider(connection, autoCreateOption);

	static TgXpoSqLiteUpperProvider()
	{
		//RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
		RegisterDataStoreProvider(XpoProviderTypeStringUpper, CreateProviderFromString);
		RegisterFactory(new TgXpoSqLiteUpperProviderFactory());
	}

	public TgXpoSqLiteUpperProvider(IDbConnection connection, AutoCreateOption autoCreateOption) : base(connection, autoCreateOption)
	{
		//
	}

	public static string GetConnectionStringUpper(string database) =>
		$"XpoProvider={XpoProviderTypeStringUpper};Data Source={EscapeConnectionStringArgument(database)}";

	public static string GetConnectionStringUpper(string database, string password) =>
		$"XpoProvider={XpoProviderTypeStringUpper};Data Source={EscapeConnectionStringArgument(database)};Password={EscapeConnectionStringArgument(password)}";

	protected override object ConvertToDbParameter(object clientValue, TypeCode clientValueTypeCode)
	{
		// UpperCase Guid.
		if (clientValue is Guid uid && clientValueTypeCode == TypeCode.Object)
			return uid.ToString().ToUpper();
		return base.ConvertToDbParameter(clientValue, clientValueTypeCode);
	}
}