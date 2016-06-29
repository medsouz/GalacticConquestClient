using System;
using System.Runtime.InteropServices;

namespace GalacticConquest.Core
{
	public class UI
	{
		public static int GetWidth()
		{
			return MemoryUtils.ReadInt32(0x1F43880);
		}

		public static int GetHeight()
		{
			return MemoryUtils.ReadInt32(0x1F43884);
		}

		[DllImport("user32.dll")]
		static extern IntPtr LoadCursorA(IntPtr hInstance, IntPtr lpCursorName);

		[DllImport("user32.dll")]
		static extern IntPtr SetCursor(IntPtr hCursor);
	}
}
