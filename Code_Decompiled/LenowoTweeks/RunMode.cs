using System;

namespace LenowoTweeks;

[Flags]
public enum RunMode
{
	None = 0,
	Always = 1,
	ElementAllocating = 2,
	SlotAllocating = 4,
	AllowNonAllocating = 0x10,
	AllowRemoved = 0x20
}
