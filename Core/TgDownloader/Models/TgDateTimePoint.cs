namespace TgDownloader.Models;

public class TgDateTimePoint(DateTime dt, long value)
{
	public DateTime Dt { get; set; } = dt;
	public long Value { get; set; } = value;
}