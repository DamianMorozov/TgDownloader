// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// https://supportcenter.devexpress.com/ticket/details/t1179747/with-sqlite-save-the-guids-in-an-uppercase-format

using DevExpress.Xpo.DB;

namespace TgStorage.Common;

public sealed class TgXpoSqLiteLowerProvider : SQLiteConnectionProvider
{
	public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
	{
		IDbConnection dbConnection = CreateConnection(connectionString);
		objectsToDisposeOnDisconnect = new IDisposable[1] { dbConnection };
		return CreateProviderFromConnection(dbConnection, autoCreateOption);
	}

	public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption) => 
		new TgXpoSqLiteLowerProvider(connection, autoCreateOption);

	static TgXpoSqLiteLowerProvider()
	{
		RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
		RegisterFactory(new TgXpoSqLiteLowerProviderFactory());
	}

	public TgXpoSqLiteLowerProvider(IDbConnection connection, AutoCreateOption autoCreateOption) : base(connection, autoCreateOption)
	{
		//
	}

	public static string GetConnectionStringLower(string database) =>
		$"XpoProvider={XpoProviderTypeString};Data Source={EscapeConnectionStringArgument(database)}";

	public static string GetConnectionStringLower(string database, string password) => 
		$"XpoProvider={XpoProviderTypeString};Data Source={EscapeConnectionStringArgument(database)};Password={EscapeConnectionStringArgument(password)}";

	protected override object ConvertToDbParameter(object clientValue, TypeCode clientValueTypeCode)
	{
		// LowerCase Guid.
		if (clientValue is Guid uid && clientValueTypeCode == TypeCode.Object)
			return uid.ToString().ToLower();
		return base.ConvertToDbParameter(clientValue, clientValueTypeCode);
	}
}