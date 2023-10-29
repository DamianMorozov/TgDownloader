namespace TgStorage.Common;

public interface ITgViewModelBase : ITgCommon
{
    bool IsLoad { get; set; }
    bool IsNotLoad => !IsLoad;
}