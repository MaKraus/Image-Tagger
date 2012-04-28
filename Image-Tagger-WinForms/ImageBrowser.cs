using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
namespace ImageTaggerWinForms
{
	public class ImageBrowser : FlowLayoutPanel
	{
		// Delegate  
		public delegate void ImageClickHandler (object sender, ImageClickEventArgs data);
		// The event  
		public event ImageClickHandler ImageClick;

		public ImageBrowser ()
		{
		}

		// The method which fires the Event  
		protected void OnImageClick (object sender, EventArgs ignored)
		{
			var pictureBox = (PictureBox) sender;
			
			var data = new ImageClickEventArgs(pictureBox.Image, pictureBox.Image.Tag as ImageInformation);
			// Check if there are any Subscribers  
			if (ImageClick != null) {
				// Call the Event  
				ImageClick (this, data);
			}
		}

		public void Add (FileInfo file)
		{
			int width = 200;
			int height = 150;
			
			var thumbnail = CreateThumbnail (file.FullName, width, height);
			var pictureBox = CreatePictureBox (thumbnail, width, height);
			
			this.Controls.Add (pictureBox);
		}

		public void Clear ()
		{
			this.Controls.Clear ();
		}

		public PictureBox CreatePictureBox (Image image, int width, int height)
		{
			PictureBox pictureBox = new PictureBox ();
			pictureBox.Width = width;
			pictureBox.Height = height;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Click += OnImageClick;
			pictureBox.Image = image;
			return pictureBox;
		}

		public Image CreateThumbnail (String path, int width, int height)
		{
			Image fullSizeImage = new Bitmap (path);
			Image scaledImage = new Bitmap (fullSizeImage, width, height);
			scaledImage.Tag = new ImageInformation { Path = path };
			fullSizeImage.Dispose ();
			return scaledImage;
		}
		
	}

	public class ImageClickEventArgs : EventArgs
	{
		public Image Thumbnail { get; internal set; }
		public ImageInformation Information { get; internal set; }
		public ImageClickEventArgs (Image thumbnail, ImageInformation information)
		{
			this.Thumbnail = thumbnail;
			this.Information = information;
		}
	}	
}

