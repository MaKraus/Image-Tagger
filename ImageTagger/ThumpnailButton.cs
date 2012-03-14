using System;
using Gtk;
using Gdk;
using System.IO;

namespace ImageTagger
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ThumpnailButton : Gtk.Button
	{
		public Pixbuf FullSizeBuffer {get; private set;}
		String filePath;
		
		public ThumpnailButton ()
		{
			this.Build ();
		}
		
		public ThumpnailButton (String filePath)
		{
			FileInfo file = new FileInfo(filePath);
			if(file.Exists && file.Extension.ToLower() == ".jpg") {
				FullSizeBuffer = new Pixbuf (filePath);
				if (FullSizeBuffer.Width >= 0 && FullSizeBuffer.Height >= 0) {
					Pixbuf resizedBuffer = ImageHelper.ResizeBuffer (FullSizeBuffer, 300, 200);
					Gtk.Image img = new Gtk.Image (resizedBuffer);
					base.Image = img;
				}
				else
				{
					base.Label = "Invalid Image";	
				}
			}	
			else
			{
				base.Label = "Invalid Image";	
			}
			this.filePath = filePath;			
			
			this.Build ();
		}
	}
}

