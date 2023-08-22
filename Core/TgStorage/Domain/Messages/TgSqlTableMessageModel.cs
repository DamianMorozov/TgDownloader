// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Utils.Filtering.Internal;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TgStorage.Domain.Messages;

/// <summary>
/// SQL table MESSAGES.
/// Do not make base class!
/// </summary>
[Persistent(TgSqlConstants.TableMessages)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableMessageModel : XPLiteObject, ITgSqlTable
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

	[DefaultValue("0001-01-01 00:00:00")]
	private DateTime _dtCreated;
	[Persistent("DT_CREATED")]
	public DateTime DtCreated { get => _dtCreated; set => SetPropertyValue(nameof(_dtCreated), ref _dtCreated, value); }

	private TgEnumMessageType _type;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnType)]
	[Indexed]
	public TgEnumMessageType Type { get => _type; set => SetPropertyValue(nameof(_type), ref _type, value); }

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
	public TgSqlTableMessageModel()
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
		_type = this.GetPropertyDefaultValueAsGeneric<TgEnumMessageType>(nameof(Type));
		_size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
		_message = this.GetPropertyDefaultValue(nameof(Message));
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgSqlTableMessageModel(Session session) : base(session)
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
		_type = this.GetPropertyDefaultValueAsGeneric<TgEnumMessageType>(nameof(Type));
		_size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
		_message = this.GetPropertyDefaultValue(nameof(Message));
	}

    public void Fill(TgSqlTableMessageModel item, Guid? uid = null)
	{
		_uid = uid ?? this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        if (item is { } message)
        {
		    _sourceId = message.SourceId;
		    _id = message.Id;
		    _dtCreated = message.DtCreated;
		    _type = message.Type;
		    _size = message.Size;
		    _message = message.Message;
        }
        else
        {
            _sourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
            _id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
            _dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
            _type = this.GetPropertyDefaultValueAsGeneric<TgEnumMessageType>(nameof(Type));
            _size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
            _message = this.GetPropertyDefaultValue(nameof(Message));
        }
    }

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableMessageModel(SerializationInfo info, StreamingContext context)
	{
		_uid = info.GetValue(nameof(Uid), typeof(Guid)) is Guid uid ? uid : Guid.Empty;
		_sourceId = info.GetInt64(nameof(SourceId));
		_id = info.GetInt64(nameof(Id));
		_dtCreated = info.GetDateTime(nameof(DtCreated));
		object? type = info.GetValue(nameof(Type), typeof(TgEnumMessageType));
		_type = type is TgEnumMessageType messageType ? messageType : TgEnumMessageType.Message;
		_size = info.GetInt64(nameof(Size));
		_message = info.GetString(nameof(Message)) ?? this.GetPropertyDefaultValue(nameof(Message));
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(Uid), Uid);
		info.AddValue(nameof(SourceId), SourceId);
		info.AddValue(nameof(Id), Id);
		info.AddValue(nameof(DtCreated), DtCreated);
		info.AddValue(nameof(Type), Type);
		info.AddValue(nameof(Size), Size);
		info.AddValue(nameof(Message), Message);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{SourceId} | {Id} | {Type} | {Size}";

	public string ToDebugString() => $"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {SourceId} | {Id} | {Type} | {Size} | {Message}";

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Uid.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceId.GetHashCode();
            hashCode = (hashCode * 397) ^ Id.GetHashCode();
            hashCode = (hashCode * 397) ^ DtCreated.GetHashCode();
            hashCode = (hashCode * 397) ^ Type.GetHashCode();
            hashCode = (hashCode * 397) ^ Size.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Message) ? 0 : Message.GetHashCode());
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
        if (obj is not TgSqlTableMessageModel item)
            return false;
        return Equals(Uid, item.Uid) && Equals(SourceId, item.SourceId) && Equals(Id, item.Id) &&
               Equals(DtCreated, item.DtCreated) && Equals(Type, item.Type) &&
               Equals(Size, item.Size) && Equals(Message, item.Message);
    }

    #endregion
}