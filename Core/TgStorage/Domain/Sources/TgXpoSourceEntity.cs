///// <summary>
///// SQL table SOURCES.
///// Do not make base class!
///// </summary>
//public sealed class TgXpoSourceEntity : XPLiteObject, ITgDbEntity
//{
//	#region Public and private fields, properties, constructor

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
//		$"{TgStorageConstants.TableSources} | {Uid} | {Id} | {(IsAutoUpdate ? "a" : " ")} | {(FirstId == Count ? "v" : "x")} | {UserName} | " +
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