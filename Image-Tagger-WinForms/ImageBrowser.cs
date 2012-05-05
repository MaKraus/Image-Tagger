using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
namespace ImageTaggerWinForms
{
	public class ImageBrowser : Control
	{
		// Delegate  
		public delegate void ImageClickHandler (object sender, ImageClickEventArgs data);
		// The event  
		public event ImageClickHandler ImageClick;

		protected PictureBox CurrentPictureBox { get; set; }
		protected FlowLayoutPanel layoutPanel = new FlowLayoutPanel();
		
		protected List<PictureBox> imageBoxes = new List<PictureBox>();
		public int imagesPerPage { get; protected set; }		
		public int MaxPage 
		{ 
			get
			{
				int imageCount = imageBoxes.Count;
				
				if(imageCount <= imagesPerPage)
				{
					return 1;	
				}
				
				return ((imageCount -1) / imagesPerPage) + 1;
			}
		}
			
		public ImageBrowser ()
		{
			layoutPanel.Dock = DockStyle.Fill;
			this.Controls.Add(layoutPanel);
			
			//Default Values
			imagesPerPage = 5;
		}

		// The method which fires the Event  
		protected void OnImageClick (object sender, EventArgs ignored)
		{
			var pictureBox = (PictureBox)sender;
			CurrentPictureBox = pictureBox;
			
			updateOverview();
			
			var data = new ImageClickEventArgs (pictureBox.Image, pictureBox.Image.Tag as ImageInformation);
			// Check if there are any Subscribers  
			if (ImageClick != null) {
				// Call the Event  
				ImageClick (this, data);
			}
		}

		protected void updateOverview()
		{
			layoutPanel.SuspendLayout();
			
			var currentIndex = imageBoxes.IndexOf (CurrentPictureBox);
			int lastValidIndex = imageBoxes.Count -1;
			int previewImageCount = (int) (imagesPerPage / 2);
			int firstImageIndex = currentIndex - previewImageCount;
			int lastImageIndex = currentIndex + previewImageCount;
			
			layoutPanel.Controls.Clear();
			for(int i = firstImageIndex; i <= lastImageIndex; i++)
			{
				if( i < 0 || i > lastValidIndex)
				{
					if(lastImageIndex + 1 <= lastValidIndex)
					{
						// Make sure that as much thumbnails are displayed as possible
						lastImageIndex++;
					}
					continue;
				}
				
				layoutPanel.Controls.Add(imageBoxes[i]);
			}			
			
			layoutPanel.ResumeLayout();
		}
		
		public void Add (FileInfo file)
		{
			int width = 200;
			int height = 150;
			
			var thumbnail = CreateThumbnail (file.FullName, width, height);
			var pictureBox = CreatePictureBox (thumbnail, width, height);
			
			imageBoxes.Add (pictureBox);
			
			if(layoutPanel.Controls.Count < imagesPerPage)
			{
				layoutPanel.Controls.Add(pictureBox);	
			}
		}

		public void Clear ()
		{
			imageBoxes.Clear ();
			layoutPanel.Controls.Clear();
			CurrentPictureBox = null;
		}

		public void NextImage ()
		{
			var pictureBox = CurrentPictureBox;
			var currentIndex = imageBoxes.IndexOf (pictureBox);
			
			if (currentIndex != -1 && currentIndex < imageBoxes.Count - 1) {
				var newPictureBox = imageBoxes[currentIndex + 1];
				OnImageClick (newPictureBox, null);
			}
			else if(currentIndex == -1 && imageBoxes.Count > 0)
			{
				var newPictureBox = imageBoxes[0];
				OnImageClick (newPictureBox, null);
			}
		}

		public void PreviousImage ()
		{
			var pictureBox = CurrentPictureBox;
			var currentIndex = imageBoxes.IndexOf (pictureBox);
			
			if (currentIndex != -1 && currentIndex > 0) {
				var newPictureBox = imageBoxes[currentIndex - 1];
				OnImageClick (newPictureBox, null);
			}
			else if(currentIndex == -1 && imageBoxes.Count > 0)
			{
				var newPictureBox = imageBoxes[0];
				OnImageClick (newPictureBox, null);
			}
		}

		public void PreviousPage()
		{
			var currentIndex = imageBoxes.IndexOf(CurrentPictureBox);
			
			if(currentIndex  - imagesPerPage >= 0)
			{
				CurrentPictureBox = imageBoxes[currentIndex - imagesPerPage];
			}
			else
			{
				CurrentPictureBox = imageBoxes[0];	
			}
			
			OnImageClick (CurrentPictureBox, null);
		}
		
		public void NextPage()
		{
			var currentIndex = imageBoxes.IndexOf (CurrentPictureBox);
			
			if(currentIndex  + imagesPerPage < imageBoxes.Count -1)
			{
				CurrentPictureBox = imageBoxes[currentIndex + imagesPerPage];
			}
			else
			{
				CurrentPictureBox = imageBoxes[imageBoxes.Count-1];	
			}
			
			OnImageClick (CurrentPictureBox, null);
		}
		
		protected PictureBox CreatePictureBox (Image image, int width, int height)
		{
			PictureBox pictureBox = new PictureBox ();
			pictureBox.Width = width;
			pictureBox.Height = height;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Click += OnImageClick;
			pictureBox.Image = image;
			return pictureBox;
		}

		protected Image CreateThumbnail (String path, int width, int height)
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

