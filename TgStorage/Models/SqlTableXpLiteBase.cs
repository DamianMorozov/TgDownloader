// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Models;

public class SqlTableXpLiteBase : XPLiteObject, ISqlTable
{
    #region Public and private fields, properties, constructor

    private Guid _uid;
    [Key(true)]
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [Persistent("UID")]
    public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }

    [DefaultValue("0001-01-01 00:00:00")]
    private DateTime _dtCreated;
    [Persistent("DT_CREATED")]
    public DateTime DtCreated
    {
        get => _dtCreated;
        set => SetPropertyValue(nameof(_dtCreated), ref _dtCreated, value);
    }

    [DefaultValue("0001-01-01 00:00:00")]
    private DateTime _dtChanged;
    [Persistent("DT_CHANGED")]
    public DateTime DtChanged
    {
        get => _dtChanged;
        set => SetPropertyValue(nameof(_dtChanged), ref _dtChanged, value);
    }

    public bool IsNotExists => Equals(Uid, Guid.Empty);
    public bool IsExists => !IsNotExists;
    public bool IsNew => IsNotExists;
    public bool IsNotNew => IsExists;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlTableXpLiteBase()
    {
        _uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(_uid));
        _dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
        _dtChanged = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtChanged));
    }

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    public SqlTableXpLiteBase(Session session) : base(session)
    {
        _uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(_uid));
        _dtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtCreated));
        _dtChanged = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(_dtChanged));
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableXpLiteBase(SerializationInfo info, StreamingContext context)
    {
        _uid = info.GetValue(nameof(_uid), typeof(Guid)) is Guid uid ? uid : Guid.Empty;
        _dtCreated = info.GetDateTime(nameof(_dtCreated));
        _dtChanged = info.GetDateTime(nameof(_dtChanged));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_uid), _uid);
        info.AddValue(nameof(_dtCreated), _dtCreated);
        info.AddValue(nameof(_dtChanged), _dtChanged);
    }

    #endregion

    #region Public and private methods

    public override string ToString() =>
        $"{nameof(Uid)} = {Uid} | " +
        $"{nameof(DtCreated)} = {DtCreated} | " +
        $"{nameof(DtChanged)} = {DtChanged}";

    #endregion
}