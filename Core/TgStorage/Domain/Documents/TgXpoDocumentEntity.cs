// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Domain.Documents;

/// <summary>
/// SQL table DOCUMENTS.
/// Do not make base class!
/// </summary>
[Persistent(TgStorageConstants.TableDocuments)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgXpoDocumentEntity : XPLiteObject, ITgDbEntity
{
	#region Public and private fields, properties, constructor

	private Guid _uid;
	[DevExpress.Xpo.Key(true)]
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Persistent(TgStorageConstants.ColumnUid)]
	[Indexed]
	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
	[NonPersistent]
	public string UidString
	{
		get => Uid.ToString();
		set => Uid = Guid.TryParse(value, out Guid uid) ? uid : Guid.Empty;
	}
	[NonPersistent]
	public bool IsExist => !Equals(Uid, Guid.Empty);
	[NonPersistent]
	public bool NotExist => !IsExist;
	[NonPersistent]
	public TgEnumLetterCase LetterCase { get; set; }

	private long _sourceId;
	[DefaultValue(0)]
	[Persistent(TgStorageConstants.ColumnSourceId)]
	[Indexed]
	public long SourceId { get => _sourceId; set => SetPropertyValue(nameof(_sourceId), ref _sourceId, value); }

	private long _id;
	[DefaultValue(0)]
	[Persistent(TgStorageConstants.ColumnId)]
	[Indexed]
	public long Id { get => _id; set => SetPropertyValue(nameof(_id), ref _id, value); }

	private long _messageId;
	[DefaultValue("")]
	[Persistent(TgStorageConstants.ColumnMessageId)]
	[Indexed]
	public long MessageId { get => _messageId; set => SetPropertyValue(nameof(_messageId), ref _messageId, value); }

	private string _fileName = default!;
	[DefaultValue("")]
	[Persistent(TgStorageConstants.ColumnFileName)]
	[Indexed]
	public string FileName { get => _fileName; set => SetPropertyValue(nameof(_fileName), ref _fileName, value); }

	private long _fileSize;
	[DefaultValue(0)]
	[Persistent(TgStorageConstants.ColumnFileSize)]
	[Indexed]
	public long FileSize { get => _fileSize; set => SetPropertyValue(nameof(_fileSize), ref _fileSize, value); }

	private long _accessHash;
	[DefaultValue(0)]
	[Persistent(TgStorageConstants.ColumnAccessHash)]
	[Indexed]
	public long AccessHash { get => _accessHash; set => SetPropertyValue(nameof(_accessHash), ref _accessHash, value); }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public TgXpoDocumentEntity()
    {
	    Default();
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgXpoDocumentEntity(Session session) : base(session)
	{
		Default();
	}

	#endregion

	#region Public and private methods

	public void Default()
	{
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		SourceId = this.GetDefaultPropertyLong(nameof(SourceId));
		Id = this.GetDefaultPropertyLong(nameof(Id));
		MessageId = this.GetDefaultPropertyLong(nameof(MessageId));
		FileName = this.GetDefaultPropertyString(nameof(FileName));
		FileSize = this.GetDefaultPropertyLong(nameof(FileSize));
		AccessHash = this.GetDefaultPropertyLong(nameof(AccessHash));
	}

	public void Fill(object item)
	{
		if (item is not TgXpoDocumentEntity document)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoDocumentEntity)}!");
        
	    SourceId = document.SourceId;
	    Id = document.Id;
	    MessageId = document.MessageId;
	    FileName = document.FileName;
	    FileSize = document.FileSize;
	    AccessHash = document.AccessHash;
    }

	public override string ToString() => $"{SourceId} | {Id} | {MessageId} | {FileName} | {FileSize} | {AccessHash}";

	public string ToDebugString() => 
		$"{TgStorageConstants.TableDocuments} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {SourceId} | {Id} | {MessageId} | {FileName} | {FileSize} | {AccessHash}";

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Uid.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceId.GetHashCode();
            hashCode = (hashCode * 397) ^ Id.GetHashCode();
            hashCode = (hashCode * 397) ^ MessageId.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(FileName) ? 0 : FileName.GetHashCode());
            hashCode = (hashCode * 397) ^ FileSize.GetHashCode();
            hashCode = (hashCode * 397) ^ AccessHash.GetHashCode();
            return hashCode;
        }
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        if (obj is not TgXpoDocumentEntity item)
            return false;
        return Equals(Uid, item.Uid) && Equals(SourceId, item.SourceId) && Equals(Id, item.Id) &&
               Equals(MessageId, item.MessageId) && Equals(FileName, item.FileName) && 
               Equals(FileSize, item.FileSize) && Equals(AccessHash, item.AccessHash);
    }

    #endregion
}