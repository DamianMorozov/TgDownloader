// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Documents;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableDocuments)]
[DoNotNotify]
public sealed class TgSqlTableDocumentModel : TgSqlTableBase
{
	#region Public and private fields, properties, constructor

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

	public TgSqlTableDocumentModel() : base()
	{
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
		_sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_messageId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(MessageId));
		_fileName = this.GetPropertyDefaultValue(nameof(FileName));
		_fileSize = this.GetPropertyDefaultValueAsGeneric<long>(nameof(FileSize));
		_accessHash = this.GetPropertyDefaultValueAsGeneric<long>(nameof(AccessHash));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableDocumentModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
		_sourceId = info.GetInt64(nameof(SourceId));
		_id = info.GetInt64(nameof(Id));
		_messageId = info.GetInt64(nameof(MessageId));
		_fileName = info.GetString(nameof(FileName)) ?? this.GetPropertyDefaultValue(nameof(FileName));
		_fileSize = info.GetInt64(nameof(FileSize));
		_accessHash = info.GetInt64(nameof(AccessHash));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(SourceId), SourceId);
        info.AddValue(nameof(Id), Id);
        info.AddValue(nameof(MessageId), MessageId);
        info.AddValue(nameof(FileName), FileName);
        info.AddValue(nameof(FileSize), FileSize);
        info.AddValue(nameof(AccessHash), AccessHash);
    }

	#endregion

	#region Public and private methods

	public override string ToString() =>
		$"{nameof(SourceId)} = {SourceId} | " +
		$"{nameof(Id)} = {Id} | " +
		$"{nameof(MessageId)} = {MessageId} | " +
        $"{nameof(FileName)} = {FileName} | " +
		$"{nameof(FileSize)} = {FileSize} | " +
		$"{nameof(AccessHash)} = {AccessHash}";

	#endregion
}