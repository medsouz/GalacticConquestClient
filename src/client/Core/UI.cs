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
	}
}
