//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Xpo;

//namespace TgStorage.Domain.Messages;

///// <summary>
///// SQL table MESSAGES.
///// Do not make base class!
///// </summary>
//[DebuggerDisplay("{ToDebugString()}")]
//[Persistent(TgStorageConstants.TableMessages)]
//[DoNotNotify]
//public sealed class TgXpoMessageEntity : XPLiteObject, ITgDbEntity
//{
//	#region Public and private fields, properties, constructor

//	private Guid _uid;
//	[DevExpress.Xpo.Key(true)]
//	[DefaultValue("00000000-0000-0000-0000-000000000000")]
//	[Persistent(TgStorageConstants.ColumnUid)]
//	[Indexed]
//	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
//	[NonPersistent]
//	public string UidString
//	{
//		get => Uid.ToString();
//		set => Uid = Guid.TryParse(value, out Guid uid) ? uid : Guid.Empty;
//	}
//	[NonPersistent]
//	public bool IsExist => !Equals(Uid, Guid.Empty);
//	[NonPersistent]
//	public bool NotExist => !IsExist;
//	[NonPersistent]
//	public TgEnumLetterCase LetterCase { get; set; }

//	private long _sourceId;
//	[DefaultValue(0)]
//	[Persistent(TgStorageConstants.ColumnSourceId)]
//	[Indexed]
//	public long SourceId { get => _sourceId; set => SetPropertyValue(nameof(_sourceId), ref _sourceId, value); }

//	private long _id;
//	[DefaultValue(0)]
//	[Persistent(TgStorageConstants.ColumnId)]
//	[Indexed]
//	public long Id { get => _id; set => SetPropertyValue(nameof(_id), ref _id, value); }

//	[DefaultValue("0001-01-01 00:00:00")]
//	private DateTime _dtCreated;
//	[Persistent("DT_CREATED")]
//	public DateTime DtCreated { get => _dtCreated; set => SetPropertyValue(nameof(_dtCreated), ref _dtCreated, value); }

//	private TgEnumMessageType _type;
//	[DefaultValue(TgEnumMessageType.Message)]
//	[Persistent(TgStorageConstants.ColumnType)]
//	[Indexed]
//	public TgEnumMessageType Type { get => _type; set => SetPropertyValue(nameof(_type), ref _type, value); }

//	private long _size;
//	[DefaultValue(0)]
//	[Persistent(TgStorageConstants.ColumnSize)]
//	[Indexed]
//	public long Size { get => _size; set => SetPropertyValue(nameof(_size), ref _size, value); }

//	private string _message = default!;
//	[DefaultValue("")]
//	[Persistent(TgStorageConstants.ColumnMessage)]
//	[Indexed]
//	public string Message { get => _message; set => SetPropertyValue(nameof(_message), ref _message, value); }

//	/// <summary>
//	/// Default constructor.
//	/// </summary>
//	public TgXpoMessageEntity()
//	{
//		Default();
//	}

//    /// <summary>
//    /// Default constructor with session.
//    /// </summary>
//    /// <param name="session"></param>
//    public TgXpoMessageEntity(Session session) : base(session)
//	{
//		Default();
//	}

//	#endregion

//	#region Public and private methods

//	public void Default()
//	{
//		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
//		SourceId = this.GetDefaultPropertyLong(nameof(SourceId));
//		Id = this.GetDefaultPropertyLong(nameof(Id));
//		DtCreated = this.GetDefaultPropertyDateTime(nameof(_dtCreated));
//		Type = this.GetDefaultPropertyGeneric<TgEnumMessageType>(nameof(Type));
//		Size = this.GetDefaultPropertyLong(nameof(Size));
//		Message = this.GetDefaultPropertyString(nameof(Message));
//	}

//	public void Fill(object item)
//	{
//		if (item is not TgXpoMessageEntity message)
//			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoMessageEntity)}!");

//		SourceId = message.SourceId;
//		Id = message.Id;
//		DtCreated = message.DtCreated > DateTime.MinValue ? message.DtCreated : DateTime.Now;
//		Type = message.Type;
//		Size = message.Size;
//		Message = message.Message;
//    }

//	public void Backup(object item)
//	{
//		Fill(item);
//		Uid = (item as TgXpoMessageEntity)!.Uid;
//	}

//	public override string ToString() => $"{SourceId} | {Id} | {Type} | {Size}";

//	public string ToDebugString() => 
//		$"{TgStorageConstants.TableMessages} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {SourceId} | {Id} | {Type} | {Size} | {Message}";

//    public override int GetHashCode()
//    {
//        unchecked
//        {
//            int hashCode = Uid.GetHashCode();
//            hashCode = (hashCode * 397) ^ SourceId.GetHashCode();
//            hashCode = (hashCode * 397) ^ Id.GetHashCode();
//            hashCode = (hashCode * 397) ^ DtCreated.GetHashCode();
//            hashCode = (hashCode * 397) ^ Type.GetHashCode();
//            hashCode = (hashCode * 397) ^ Size.GetHashCode();
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Message) ? 0 : Message.GetHashCode());
//            return hashCode;
//        }
//    }

//    public override bool Equals(object? obj)
//    {
//        if (ReferenceEquals(null, obj))
//            return false;
//        if (ReferenceEquals(this, obj))
//            return true;
//        if (obj.GetType() != GetType())
//            return false;
//        if (obj is not TgXpoMessageEntity item)
//            return false;
//        return Equals(Uid, item.Uid) && Equals(SourceId, item.SourceId) && Equals(Id, item.Id) &&
//               Equals(DtCreated, item.DtCreated) && Equals(Type, item.Type) &&
//               Equals(Size, item.Size) && Equals(Message, item.Message);
//    }

//    #endregion
//}