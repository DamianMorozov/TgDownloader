// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Models;

public class FooModel : IBase
{
    #region Public and private fields, properties, constructor

    private ushort _valueUshort;
    [DefaultValue((ushort)123)]
    public ushort ValueUshort { get => _valueUshort; set => _valueUshort = value; }
    
    private uint _valueUint;
    [DefaultValue((uint)324)]
    public uint ValueUint { get => _valueUint; set => _valueUint = value; }
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public FooModel()
    {
        _valueUshort = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(ValueUshort));
        _valueUint = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(ValueUint));
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected FooModel(SerializationInfo info, StreamingContext context)
    {
        _valueUshort = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(ValueUshort));
        _valueUint = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(ValueUint));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_valueUshort), _valueUshort);
        info.AddValue(nameof(_valueUint), _valueUint);
    }

    #endregion
}