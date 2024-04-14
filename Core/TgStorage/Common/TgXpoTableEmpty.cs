// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Common;

/// <summary>
/// SQL table object base.
/// Do not make base from this class like TgSqlTableBase!!!
/// </summary>
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public class TgXpoTableEmpty : XPLiteObject, ITgDbEntity
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

	public TgXpoTableEmpty()
	{
		Default();
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    public TgXpoTableEmpty(Session session) : base(session)
	{
		Default();
	}

	#endregion

	#region Public and private methods

	public void Default()
	{
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
	}

	public void Fill(object item)
	{
		if (item is not TgXpoTableEmpty)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoTableEmpty)}!");
	}

	public override string ToString() => $"{Uid}";

    public string ToDebugString() => ToString();

    #endregion
}