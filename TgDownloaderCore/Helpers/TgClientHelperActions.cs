// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Runtime.CompilerServices;

namespace TgDownloaderCore.Helpers;

public partial class TgClientHelper
{
    private void TryCatchAction(Action action, Action<string> refreshStatus = null,
        [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            string fileName = Path.GetFileName(filePath);
            if (refreshStatus is not null)
            {
                //Log.MarkupLineStamp($"Exception at | {nameof(fileName)}: {fileName} | {nameof(lineNumber)}: {lineNumber} | {nameof(memberName)}: {memberName}");
                refreshStatus($"Exception at | {nameof(fileName)}: {fileName} | {nameof(lineNumber)}: {lineNumber} | {nameof(memberName)}: {memberName}");
                //Log.MarkupLineStamp(ex.Message);
                refreshStatus(ex.Message);
                if (ex.InnerException is not null)
                {
                    //Log.MarkupLineStamp(ex.InnerException.Message);
                    refreshStatus(ex.InnerException.Message);
                }
                /*
                string messageId = messageBase is not null ? messageBase.ID.ToString() : string.Empty;
                Log.MarkupLineStamp($"Exception | {messageId} | {ex.Message}");
                refreshStatus($"Exception | {messageId} |");
                refreshStatus(ex.Message);
                if (ex.InnerException is not null)
                {
                    Log.MarkupLineStamp(ex.InnerException.Message);
                    refreshStatus(ex.InnerException.Message);
                }
                */
            }
        }
    }
}