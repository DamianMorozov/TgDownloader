// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Messages;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableMessages)]
public sealed class TgSqlTableMessageModel : TgSqlTableBase
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

    [DefaultValue("0001-01-01 00:00:00")]
    private DateTime _dtCreated;
    [Persistent("DT_CREATED")]
    public DateTime DtCreated { get => _dtCreated; set => SetPropertyValue(nameof(_dtCreated), ref _dtCreated, value); }

	private TgMessageType _type;
    [DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnType)]
	[Indexed]
	public TgMessageType Type { get => _type; set => SetPropertyValue(nameof(_type), ref _type, value); }

    private long _size;
    [DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnSize)]
	[Indexed]
	public long Size { get => _size; set => SetPropertyValue(nameof(_size), ref _size, value); }

    private string _message;
    [DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnMessage)]
	[Indexed]
	public string Message { get => _message; set => SetPropertyValue(nameof(_message), ref _message, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgSqlTableMessageModel() : base()
	{
        _sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
        _id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        _dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
		_type = this.GetPropertyDefaultValueAsGeneric<TgMessageType>(nameof(Type));
		_size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
        _message = this.GetPropertyDefaultValue(nameof(Message));
    }

	/// <summary>
	/// Default constructor with session.
	/// </summary>
	/// <param name="session"></param>
	public TgSqlTableMessageModel(Session session) : base(session)
	{
		_sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
		_type = this.GetPropertyDefaultValueAsGeneric<TgMessageType>(nameof(Type));
		_size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
		_message = this.GetPropertyDefaultValue(nameof(Message));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableMessageModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
		_sourceId = info.GetInt64(nameof(SourceId));
        _id = info.GetInt64(nameof(Id));
		_dtCreated = info.GetDateTime(nameof(DtCreated));
		object? type = info.GetValue(nameof(Type), typeof(TgMessageType));
		_type = type is TgMessageType messageType ? messageType : TgMessageType.Message;
		_size = info.GetInt64(nameof(Size));
		_message = info.GetString(nameof(Message)) ?? this.GetPropertyDefaultValue(nameof(Message));
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
        info.AddValue(nameof(DtCreated), DtCreated);
		info.AddValue(nameof(Type), Type);
        info.AddValue(nameof(Size), Size);
        info.AddValue(nameof(Message), Message);
    }

	#endregion

	#region Public and private methods

	public override string ToString() =>
		$"{nameof(SourceId)} = {SourceId} | " +
		$"{nameof(Id)} = {Id} | " +
	    $"{nameof(Type)} = {Type} | " +
		$"{nameof(Size)} = {Size}";

	#endregion
}