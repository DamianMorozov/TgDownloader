// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

public sealed class TgEfSourceDto : ITgDbEntity, ITgDbFillEntity<TgEfSourceDto>
{
	public Guid Uid { get; set; }
	public DateTime DtChanged { get; set; }
	public long Id { get; set; }
	public bool IsActive { get; set; }
	public string? UserName { get; set; }
	public string? Title { get; set; }
	public int Count { get; set; }
	public string? Directory { get; set; }
	public int FirstId { get; set; }
	public bool IsAutoUpdate { get; set; }

	public void Default()
	{
		throw new NotImplementedException();
	}

	public TgEfSourceDto Fill(TgEfSourceDto item, bool isUidCopy)
	{
		throw new NotImplementedException();
	}

	public string ToDebugString()
	{
		throw new NotImplementedException();
	}
}
