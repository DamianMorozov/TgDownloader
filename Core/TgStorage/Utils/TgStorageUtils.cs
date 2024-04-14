﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;
using Microsoft.EntityFrameworkCore;

namespace TgStorage.Utils;

public static class TgStorageUtils
{
	#region Public and private fields, properties, constructor

	public static readonly ReaderWriterLockSlim ReaderWriterLockSlimItems = new();
	public static readonly SemaphoreSlim SemaphoreSlimItems = new(1, 1);
	public static readonly List<string> ListItems = new();

	#endregion

	#region Public and private methods - GetXpValid

	public static FluentValidation.Results.ValidationResult GetXpValid<T>(T item) where T : XPLiteObject, ITgDbEntity, new() =>
	    item switch
	    {
		    TgXpoAppEntity app => GetXpValid(app),
		    TgXpoDocumentEntity document => GetXpValid(document),
		    TgXpoFilterEntity filter => GetXpValid(filter),
		    TgXpoMessageEntity message => GetXpValid(message),
		    TgXpoSourceEntity source => GetXpValid(source),
		    TgXpoProxyEntity proxy => GetXpValid(proxy),
		    TgXpoVersionEntity version => GetXpValid(version),
		    _ => new FluentValidation.Results.ValidationResult { Errors = [new ValidationFailure(nameof(item), "Type error!")] }
	    };
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoAppEntity item) => new TgXpoAppValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoDocumentEntity item) => new TgXpoDocumentValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoFilterEntity item) => new TgXpoFilterValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoMessageEntity item) => new TgXpoMessageValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoSourceEntity item) => new TgXpoSourceValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoProxyEntity item) => new TgXpoProxyValidator().Validate(item);
    public static FluentValidation.Results.ValidationResult GetXpValid(TgXpoVersionEntity item) => new TgXpoVersionValidator().Validate(item);

	#endregion

	#region Public and private methods - GetEfValid

	public static FluentValidation.Results.ValidationResult GetEfValid<T>(T item) where T : TgEfEntityBase, ITgDbEntity, new() =>
	    item switch
	    {
		    TgEfAppEntity app => GetEfValid(app),
		    TgEfDocumentEntity document => GetEfValid(document),
		    TgEfFilterEntity filter => GetEfValid(filter),
		    TgEfMessageEntity message => GetEfValid(message),
		    TgEfSourceEntity source => GetEfValid(source),
		    TgEfProxyEntity proxy => GetEfValid(proxy),
		    TgEfVersionEntity version => GetEfValid(version),
		    _ => new FluentValidation.Results.ValidationResult { Errors = [new ValidationFailure(nameof(item), "Type error!")] }
	    };

	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfAppEntity item) => new TgEfAppValidator().Validate(item);
	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfDocumentEntity item) => new TgEfDocumentValidator().Validate(item);
	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfFilterEntity item) => new TgEfFilterValidator().Validate(item);
	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfMessageEntity item) => new TgEfMessageValidator().Validate(item);
	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfSourceEntity item) => new TgEfSourceValidator().Validate(item);
	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfProxyEntity item) => new TgEfProxyValidator().Validate(item);
	public static FluentValidation.Results.ValidationResult GetEfValid(TgEfVersionEntity item) => new TgEfVersionValidator().Validate(item);

	#endregion

	#region Public and private methods

	public static TgEfContext GetEfContextProd()
	{
		LoggerFactory factory = new();
		DbContextOptionsBuilder<TgEfContext> builderDbProd = new DbContextOptionsBuilder<TgEfContext>()
			.UseLoggerFactory(factory)
			.UseSqlite($"{TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}");
		return new(builderDbProd.Options);
	}

	public static string ToUpperString(this Guid uid) => uid.ToString().ToUpper();

	#endregion
}