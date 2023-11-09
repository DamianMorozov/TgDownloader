// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloader.Models;

[DebuggerDisplay("Type = {nameof(TgDownloadSmartSource)}")]
public class TgDownloadSmartSource
{
    #region Public and private fields, properties, constructor

    public bool IsEmpty { get; set; } = true;
    private ChatBase? _chatBase;
    public ChatBase? ChatBase
    {
        get => _chatBase;
        set
        {
            _chatBase = value;
            Type = _chatBase is not null ? TgEnumSourceType.ChatBase : TgEnumSourceType.Default;
        }
    }
    private Channel? _channel;
    public Channel? Channel
    {
        get => _channel;
        set
        {
            _channel = value;
            Type = _channel is not null ? TgEnumSourceType.Channel : TgEnumSourceType.Default;
        }
    }
    public TgEnumSourceType Type { get; set; }

    #endregion

    #region Public and private methods

    public string ToDebugString() => $"{(IsEmpty ? "Is empty" : "Not empty")}";

    #endregion
}
