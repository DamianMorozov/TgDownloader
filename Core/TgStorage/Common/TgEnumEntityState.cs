// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

public enum TgEnumEntityState
{
	Unknown,
	IsExist,
	NotExist,
	IsSaved,
	NotSaved,
	IsDeleted,
	NotDeleted,
	IsExecuted,
	NotExecuted,
}