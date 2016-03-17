using GalacticConquest.Core;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GalacticConquest.Patch
{
	public class PatchRenderer
	{
		Hook<D3DDevice_EndSceneDelegate> D3DDevice_EndSceneHook = null;

		public PatchRenderer()
		{
			Device device = new Device(new Direct3D(), 0, DeviceType.NullReference, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1 });
			//EndScene's offset is 42
			IntPtr D3DDevice_EndScene_Address = (IntPtr)Marshal.ReadInt32((IntPtr)Marshal.ReadInt32(device.NativePointer) + 0xA8);
			D3DDevice_EndSceneHook = new Hook<D3DDevice_EndSceneDelegate>(D3DDevice_EndScene_Address, new D3DDevice_EndSceneDelegate(D3DDevice_EndScene), this);
		}


		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		delegate int D3DDevice_EndSceneDelegate(IntPtr devicePtr);

		int D3DDevice_EndScene(IntPtr devicePtr)
		{
			try {
				Device device = CppObject.FromPointer<Device>(devicePtr);
				Sprite s = new Sprite(device);

				s.Begin(SpriteFlags.AlphaBlend);

				int mouseX = MemoryUtils.ReadInt32(0x85CAB4);
				int mouseY = MemoryUtils.ReadInt32(0x85CAB8);

				GalacticConquest.ModuleCEF.SetMousePosition(mouseX, mouseY);

				Texture cefTexture = GalacticConquest.ModuleCEF.GetTexture(device);
				if (cefTexture != null)
					s.Draw(cefTexture, new SharpDX.Mathematics.Interop.RawColorBGRA(255, 255, 255, 255));
				s.End();

				Utilities.Dispose(ref s);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "Galactic Conquest Rendering Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return D3DDevice_EndSceneHook.original(devicePtr);
        }
    }
}
