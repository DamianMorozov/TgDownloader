// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloader.Models;

public class TgDateTimePoint(DateTime dt, long value)
{
	public DateTime Dt { get; set; } = dt;
	public long Value { get; set; } = value;
}