using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace GalacticConquest.Core
{
	public class SWBF2Version
	{
		public enum Release
		{
			STEAM,
			RETAIL,
			GOG,
			UNKNOWN
		}

		private static string[] Name =
		{
			"Steam",
			"Retail",
			"GOG",
			"Unknown"
		};

		private static string[] SHA1Sum =
		{
			"0cac443f5c3e6ffa3ba273964412cb027f47e3cf",
			"6302a8ed9de54de63755c03fdf740e353db6ee9a",
			"b3816fe3f8ed0b89e31afb59bd25b9a95d4547a2",
			""
		};

		public static SWBF2Version getVersion(string sha1sum)
		{
			Release release = Release.UNKNOWN;
			for(int s = 0; s < SHA1Sum.Length; s++)
			{
				if (SHA1Sum[s] == sha1sum)
					release = (Release) s;
			}

			return new SWBF2Version(release);
		}

		public string name { get; private set; }
		public Release release{ get; private set; }

		public SWBF2Version(Release release)
		{
			this.release = release;
			name = Name[(int)release];
		}
	}
}
