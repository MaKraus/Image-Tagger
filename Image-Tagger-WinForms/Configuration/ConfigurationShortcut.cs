using System;
using System.Windows.Forms;
namespace ImageTaggerWinForms
{
	public class ConfigurationShortcut
	{
		// File
		public Keys OpenDirectory { get; set; }
		
		// Navigation
		public Keys PreviousImage { get; set; }
		public Keys NextImage { get; set; }
		public Keys PreviousPage { get; set; }
		public Keys NextPage { get; set; }
		
		public ConfigurationShortcut ()
		{
		}
	}
}

