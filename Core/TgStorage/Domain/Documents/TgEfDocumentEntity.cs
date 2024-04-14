﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage.Domain.Documents;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgStorageConstants.TableDocuments)]
[Index(nameof(SourceId))]
[Index(nameof(Id))]
[Index(nameof(SourceId), nameof(Id), IsUnique = true)]
[Index(nameof(MessageId))]
[Index(nameof(FileName))]
[Index(nameof(FileSize))]
[Index(nameof(AccessHash))]
public sealed class TgEfDocumentEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnSourceId)]
    public long SourceId { get; set; }
	
    public TgEfSourceEntity? Source { get; set; }

	[DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnId)]
    public long Id { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnMessageId)]
    public long MessageId { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(256)]
    [Column(TgStorageConstants.ColumnFileName)]
    public string FileName { get; set; } = default!;

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnFileSize)]
    public long FileSize { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnAccessHash)]
    public long AccessHash { get; set; }

    public TgEfDocumentEntity() : base()
    {
	    Default();
	}

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{TgStorageConstants.TableDocuments} | {base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {SourceId} | {Id} | {MessageId} | {FileName} | {FileSize} | {AccessHash}";

    public override void Default()
    {
	    base.Default();
	    SourceId = this.GetDefaultPropertyLong(nameof(SourceId));
	    Id = this.GetDefaultPropertyLong(nameof(Id));
	    MessageId = this.GetDefaultPropertyLong(nameof(MessageId));
	    FileName = this.GetDefaultPropertyString(nameof(FileName));
	    FileSize = this.GetDefaultPropertyLong(nameof(FileSize));
	    AccessHash = this.GetDefaultPropertyLong(nameof(AccessHash));
	}

    public override void Fill(object item)
    {
	    if (item is not TgEfDocumentEntity document)
		    throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfDocumentEntity)}!");

		SourceId = document.SourceId;
		Id = document.Id;
		MessageId = document.MessageId;
		FileName = document.FileName;
		FileSize = document.FileSize;
		AccessHash = document.AccessHash;
	}

    #endregion
}