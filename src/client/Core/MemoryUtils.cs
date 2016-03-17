using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GalacticConquest.Core
{
	public class MemoryUtils
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

		[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

		public static int ReadInt32(int address)
		{
			byte[] buffer = new byte[4];
			int bytesRead = 0;
			ReadProcessMemory((int)OpenProcess(0x1F0FFF, false, Process.GetCurrentProcess().Id), address, buffer, 4, ref bytesRead);
			return BitConverter.ToInt32(buffer, 0);
		}
	}
}
