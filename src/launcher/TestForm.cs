using EasyHook;
using GalacticConquest.Core;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GalacticConquest.Launcher
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
		}

		Process game;

		private void btnStartGame_Click(object sender, EventArgs e)
		{
			game = createSWBF2Process();
		}

		private void btnForceload_Click(object sender, EventArgs e)
		{
			if (game == null || game.HasExited)
				game = createSWBF2Process();

			IntPtr process = MemoryUtils.OpenProcess(0x1F0FFF, false, game.Id);
			int bytesRead = 0;
			byte[] buff = new byte[1];
			while (buff[0] == 0)
				MemoryUtils.ReadProcessMemory((int)process, 0x8FE45C, buff, 1, ref bytesRead);

			int bytesWritten = 0;
			//Forceload cor1c_con
			byte[] buffer = { 0x48, 0x6E, 0xF4, 0xA4, 0x63, 0x6F, 0x72, 0x31, 0x63, 0x5F, 0x63, 0x6F, 0x6E };
			MemoryUtils.WriteProcessMemory((int)process, 0x8FF9AC, buffer, buffer.Length, ref bytesWritten);
			buffer = BitConverter.GetBytes(0x7FF034);
			MemoryUtils.WriteProcessMemory((int)process, 0x1D77E58, buffer, buffer.Length, ref bytesWritten);
			buffer = BitConverter.GetBytes((byte)1);
			MemoryUtils.WriteProcessMemory((int)process, 0x1D77E5C, buffer, buffer.Length, ref bytesWritten);
		}

		public Process createSWBF2Process()
		{
			try {
				//Setup EasyHook
				Config.Register("GalacticConquest", "GalacticConquest.dll");

				ProcessStartInfo SWBF2 = new ProcessStartInfo("BattlefrontII.exe", "/win /resolution 1920 1080 " + ((txtProfile.Text != "") ? "/name " + txtProfile.Text : ""));
				//TODO: Don't hardcode
				SWBF2.WorkingDirectory = "D:\\Steam\\steamapps\\common\\Star Wars Battlefront II\\GameData";
				Process proc = Process.Start(SWBF2);
				//Inject our DLL
				RemoteHooking.Inject(proc.Id, InjectionOptions.Default, "GalacticConquest.dll", "GalacticConquest.dll", "GalacticConquest");

				return proc;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			return null;
		}

		private void btnCEF_Click(object sender, EventArgs e)
		{
			//InitCEF();
			//MainAsync("cachePath1", 1.0);
			//Cef.Shutdown();
		}
	}
}
