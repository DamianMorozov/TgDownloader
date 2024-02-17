// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Documents;

/// <summary>
/// SQL table DOCUMENTS.
/// Do not make base class!
/// </summary>
[Persistent(TgSqlConstants.TableDocuments)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableDocumentModel : XPLiteObject, ITgSqlTable
{
	#region Public and private fields, properties, constructor

	private Guid _uid;
	[Key(true)]
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Persistent(TgSqlConstants.ColumnUid)]
	[Indexed]
	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
	public bool IsNotExists => Equals(Uid, Guid.Empty);
	public bool IsExists => !IsNotExists;

	private long _sourceId;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnSourceId)]
	[Indexed]
	public long SourceId { get => _sourceId; set => SetPropertyValue(nameof(_sourceId), ref _sourceId, value); }

	private long _id;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnId)]
	[Indexed]
	public long Id { get => _id; set => SetPropertyValue(nameof(_id), ref _id, value); }

	private long _messageId;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnMessageId)]
	[Indexed]
	public long MessageId { get => _messageId; set => SetPropertyValue(nameof(_messageId), ref _messageId, value); }

	private string _fileName;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnFileName)]
	[Indexed]
	public string FileName { get => _fileName; set => SetPropertyValue(nameof(_fileName), ref _fileName, value); }

	private long _fileSize;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnFileSize)]
	[Indexed]
	public long FileSize { get => _fileSize; set => SetPropertyValue(nameof(_fileSize), ref _fileSize, value); }

	private long _accessHash;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnAccessHash)]
	[Indexed]
	public long AccessHash { get => _accessHash; set => SetPropertyValue(nameof(_accessHash), ref _accessHash, value); }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public TgSqlTableDocumentModel()
	{
        _uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_messageId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(MessageId));
		_fileName = this.GetPropertyDefaultValue(nameof(FileName));
		_fileSize = this.GetPropertyDefaultValueAsGeneric<long>(nameof(FileSize));
		_accessHash = this.GetPropertyDefaultValueAsGeneric<long>(nameof(AccessHash));
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgSqlTableDocumentModel(Session session) : base(session)
	{
        _uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_messageId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(MessageId));
		_fileName = this.GetPropertyDefaultValue(nameof(FileName));
		_fileSize = this.GetPropertyDefaultValueAsGeneric<long>(nameof(FileSize));
		_accessHash = this.GetPropertyDefaultValueAsGeneric<long>(nameof(AccessHash));
	}

    public void Fill(TgSqlTableDocumentModel item, Guid? uid = null)
	{
		_uid = uid ?? this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        if (item is { } document)
        {
		    _sourceId = document.SourceId;
		    _id = document.Id;
		    _messageId = document.MessageId;
		    _fileName = document.FileName;
		    _fileSize = document.FileSize;
		    _accessHash = document.AccessHash;
        }
        else
        {
            _sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
            _id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
            _messageId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(MessageId));
            _fileName = this.GetPropertyDefaultValue(nameof(FileName));
            _fileSize = this.GetPropertyDefaultValueAsGeneric<long>(nameof(FileSize));
            _accessHash = this.GetPropertyDefaultValueAsGeneric<long>(nameof(AccessHash));
        }
    }

	#endregion

	#region Public and private methods

	public override string ToString() => $"{SourceId} | {Id} | {MessageId} | {FileName} | {FileSize} | {AccessHash}";

	public string ToDebugString() => $"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {SourceId} | {Id} | {MessageId} | {FileName} | {FileSize} | {AccessHash}";

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
        if (obj is not TgSqlTableDocumentModel item)
            return false;
        return Equals(Uid, item.Uid) && Equals(SourceId, item.SourceId) && Equals(Id, item.Id) &&
               Equals(MessageId, item.MessageId) && Equals(FileName, item.FileName) && 
               Equals(FileSize, item.FileSize) && Equals(AccessHash, item.AccessHash);
    }

    #endregion
}