//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Xpo;

//namespace TgStorage.Domain.Sources;

///// <summary>
///// SQL table SOURCES.
///// Do not make base class!
///// </summary>
//[DebuggerDisplay("{ToDebugString()}")]
//[Persistent(TgStorageConstants.TableSources)]
//[DoNotNotify]
//public sealed class TgXpoSourceEntity : XPLiteObject, ITgDbEntity
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

//	private long _id;
//	[DefaultValue(1)]
//	[Persistent(TgStorageConstants.ColumnId)]
//	[Indexed]
//	public long Id { get => _id; set => SetPropertyValue(nameof(_id), ref _id, value); }

//	private string _userName = default!;
//	[DefaultValue("UserName")]
//	[Persistent(TgStorageConstants.ColumnUserName)]
//    [Size(256)]
//    [Indexed]
//	public string UserName { get => _userName; set => SetPropertyValue(nameof(_userName), ref _userName, value); }

//	private string _title = default!;
//	[DefaultValue("Title")]
//	[Persistent(TgStorageConstants.ColumnTitle)]
//    [Size(1024)]
//    [Indexed]
//	public string Title { get => _title; set => SetPropertyValue(nameof(_title), ref _title, value); }

//	private string _about = default!;
//	[DefaultValue("About")]
//	[Persistent(TgStorageConstants.ColumnAbout)]
//	[Indexed]
//	public string About { get => _about; set => SetPropertyValue(nameof(_about), ref _about, value); }

//	private int _count;
//	[DefaultValue(1)]
//	[Persistent(TgStorageConstants.ColumnCount)]
//	[Indexed]
//	public int Count { get => _count; set => SetPropertyValue(nameof(_count), ref _count, value); }

//	private string _directory = default!;
//	[DefaultValue("")]
//	[Persistent(TgStorageConstants.ColumnDirectory)]
//    [Size(1024)]
//    [Indexed]
//	public string Directory { get => _directory; set => SetPropertyValue(nameof(_directory), ref _directory, value); }

//	private int _firstId;
//	[DefaultValue(1)]
//	[Persistent(TgStorageConstants.ColumnFirstId)]
//	[Indexed]
//	public int FirstId { get => _firstId; set => SetPropertyValue(nameof(_firstId), ref _firstId, value); }

//	private bool _isAutoUpdate;
//	[DefaultValue(false)]
//	[Persistent(TgStorageConstants.ColumnIsAutoUpdate)]
//	[Indexed]
//	public bool IsAutoUpdate { get => _isAutoUpdate; set => SetPropertyValue(nameof(_isAutoUpdate), ref _isAutoUpdate, value); }

//    private DateTime _dtChanged;
//    [DefaultValue("0001-01-01 00:00:00")]
//    [Persistent(TgStorageConstants.ColumnDtChanged)]
//    [Indexed]
//    public DateTime DtChanged { get => _dtChanged; set => SetPropertyValue(nameof(_dtChanged), ref _dtChanged, value); }

//	/// <summary>
//    /// Default constructor.
//    /// </summary>
//    public TgXpoSourceEntity()
//    {
//	    Default();
//	}

//    /// <summary>
//    /// Default constructor with session.
//    /// </summary>
//    /// <param name="session"></param>
//    public TgXpoSourceEntity(Session session) : base(session)
//	{
//		Default();
//	}

//	#endregion

//	#region Public and private methods

//	public void Default()
//	{
//		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
//		Id = this.GetDefaultPropertyLong(nameof(Id));
//		UserName = this.GetDefaultPropertyString(nameof(UserName));
//		Title = this.GetDefaultPropertyString(nameof(Title));
//		About = this.GetDefaultPropertyString(nameof(About));
//		Count = this.GetDefaultPropertyInt(nameof(Count));
//		Directory = this.GetDefaultPropertyString(nameof(Directory));
//		FirstId = this.GetDefaultPropertyInt(nameof(FirstId));
//		IsAutoUpdate = this.GetDefaultPropertyBool(nameof(IsAutoUpdate));
//		DtChanged = this.GetDefaultPropertyDateTime(nameof(DtChanged));
//	}

//	public void Fill(object item)
//	{
//		if (item is not TgXpoSourceEntity source)
//			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoSourceEntity)}!");
        
//		DtChanged = source.DtChanged > DateTime.MinValue ? source.DtChanged : DateTime.Now;
//        Id = source.Id;
//        FirstId = source.FirstId;
//        IsAutoUpdate = source.IsAutoUpdate;
//        UserName = string.IsNullOrEmpty(source.UserName) ? "" : source.UserName;
//        Title = source.Title;
//        About = source.About;
//        Count = source.Count;
//        Directory = source.Directory;
//    }

//	public void Backup(object item)
//	{
//		Fill(item);
//		Uid = (item as TgXpoSourceEntity)!.Uid;
//	}

//	public string ToConsoleStringShort() =>
//        $"{GetPercentCountString()} | {(IsAutoUpdate ? "a | " : "")} | {Id} | " +
//        $"{(string.IsNullOrEmpty(UserName) ? "" : TgDataFormatUtils.GetFormatString(UserName, 30))} | " +
//        $"{(string.IsNullOrEmpty(Title) ? "" : TgDataFormatUtils.GetFormatString(Title, 30))} | " +
//        $"{FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

//    public string ToConsoleString() =>
//        $"{GetPercentCountString()} | {(IsAutoUpdate ? "a" : " ")} | {Id} | " +
//        $"{TgDataFormatUtils.GetFormatString(UserName, 30)} | " +
//        $"{TgDataFormatUtils.GetFormatString(Title, 30)} | " +
//        $"{FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

//    public string GetPercentCountString()
//    {
//	    float percent = Count <= FirstId ? 100 : FirstId > 1 ? (float) FirstId * 100 / Count : 0;
//	    if (IsPercentCountAll())
//		    return "100.00 %";
//	    return percent > 9 ? $" {percent:00.00} %" : $"  {percent:0.00} %";
//    }

//    public bool IsPercentCountAll() => Count <= FirstId;

//    public string ToDebugString() => 
//		$"{TgStorageConstants.TableSources} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {Id} | {(IsAutoUpdate ? "a" : " ")} | {(FirstId == Count ? "v" : "x")} | {UserName} | " +
//	    $"{TgDataFormatUtils.TrimStringEnd(Title)} | {FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

//    public override int GetHashCode()
//    {
//        unchecked
//        {
//            int hashCode = Uid.GetHashCode();
//            hashCode = (hashCode * 397) ^ Id.GetHashCode();
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(UserName) ? 0 : UserName.GetHashCode());
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Title) ? 0 : Title.GetHashCode());
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(About) ? 0 : About.GetHashCode());
//            hashCode = (hashCode * 397) ^ Count.GetHashCode();
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Directory) ? 0 : Directory.GetHashCode());
//            hashCode = (hashCode * 397) ^ FirstId.GetHashCode();
//            hashCode = (hashCode * 397) ^ IsAutoUpdate.GetHashCode();
//            hashCode = (hashCode * 397) ^ DtChanged.GetHashCode();
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
//        if (obj is not TgXpoSourceEntity item)
//            return false;
//        return Equals(Uid, item.Uid) && Equals(Id, item.Id) && Equals(UserName, item.UserName) &&
//               Equals(Title, item.Title) && Equals(About, item.About) &&
//               Equals(Count, item.Count) && Equals(Directory, item.Directory) && Equals(FirstId, item.FirstId) &&
//               Equals(IsAutoUpdate, item.IsAutoUpdate) && Equals(DtChanged, item.DtChanged);
//    }

//    #endregion
//}