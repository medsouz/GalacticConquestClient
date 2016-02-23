using EasyHook;
using System;
using System.Runtime.InteropServices;

namespace GalacticConquest.Core
{

	public class Hook<T> where T: class
	{
		public T original { get; private set; }
		public LocalHook localHook { get; private set; }

		public Hook(IntPtr targetAddress, Delegate func, object callback)
		{
			original = (T)(object)Marshal.GetDelegateForFunctionPointer(targetAddress, typeof(T));
			localHook = LocalHook.Create(targetAddress, func, callback);
			localHook.ThreadACL.SetExclusiveACL(new Int32[1]);
		}
	}
}
