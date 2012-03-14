using System;
using Gdk;
namespace ImageTagger
{
	public class ImageHelper
	{
		public static Pixbuf ResizeBuffer (Pixbuf buffer, int destWidth, int destHeight)
		{
			if (buffer != null) {
				
				int srcWidth, srcHeight;
				int newWidth, newHeight;
				
				srcWidth = buffer.Width;
				srcHeight = buffer.Height;
				
				getNewDimensions (srcWidth, srcHeight, destWidth, destHeight, out newWidth, out newHeight);
				return buffer.ScaleSimple (newWidth, newHeight, Gdk.InterpType.Bilinear);
			} else {
				return buffer;
			}
		}

		public static void ZoomToWindow (Pixbuf sourcePixbuf, ref Gtk.Image destinationImageWidget)
		{
			if (sourcePixbuf != null) {
				
				int srcWidth, srcHeight;
				int destWidth, destHeight;
				int newWidth, newHeight;
				Gdk.Region visibleRegion = destinationImageWidget.GdkWindow.VisibleRegion;
				Gdk.Rectangle rectangle = visibleRegion.GetRectangles ()[0];
				
				srcWidth = sourcePixbuf.Width;
				srcHeight = sourcePixbuf.Height;
				destWidth = rectangle.Width;
				destHeight = rectangle.Height;
				
				getNewDimensions (srcWidth, srcHeight, destWidth, destHeight, out newWidth, out newHeight);
				destinationImageWidget.Pixbuf = sourcePixbuf.ScaleSimple (newWidth, newHeight, Gdk.InterpType.Bilinear);
			}
		}

		private static void getNewDimensions (int srcWidth, int srcHeight, int destWidth, int destHeight, out int newWidth, out int newHeight)
		{
			double scaleFactor;
			
			if (destWidth <= destHeight) {
				scaleFactor = (double)destWidth / (double)srcWidth;
				newWidth = destWidth;
				newHeight = Convert.ToInt32 (srcHeight * scaleFactor);
			} else {
				scaleFactor = (double)destHeight / (double)srcHeight;
				newWidth = Convert.ToInt32 (srcWidth * scaleFactor);
				newHeight = destHeight;
			}
		}
		
	}
}

