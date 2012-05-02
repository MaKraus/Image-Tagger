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
		protected TableLayoutPanel layoutPanel = new TableLayoutPanel();
		
		protected List<PictureBox> imageBoxes = new List<PictureBox>();
		public int imagesPerPage { get; protected set; }		
		public int currentPage { get; protected set; }
		public int maxPage 
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
			currentPage = 0;
		}

		// The method which fires the Event  
		protected void OnImageClick (object sender, EventArgs ignored)
		{
			var pictureBox = (PictureBox)sender;
			CurrentPictureBox = pictureBox;
			
			var data = new ImageClickEventArgs (pictureBox.Image, pictureBox.Image.Tag as ImageInformation);
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
			currentPage = 1;
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

		public void NextPage()
		{
			if(currentPage + 1 < maxPage)
			{
				return;	
			}
			
			int newPage = currentPage + 1;
			SwitchPage(newPage);			
		}
		
		public void PreviousPage()
		{
			if(currentPage - 1 <= 0)
			{
				return;	
			}
			
			int newPage = currentPage -1;
			SwitchPage(newPage);
		}
		
		public void SwitchPage(int page)
		{
			if(page <= 0 || page > maxPage)
			{
				return;
			}
			
			this.Controls.Clear();
			var newImages = GetImagesForPage(page).ToArray();
			this.Controls.AddRange(newImages);
			if(newImages.Length > 0)
			{
				OnImageClick (newImages[0], null);
			}
		}
		
		protected List<PictureBox> GetImagesForPage(int page)
		{
			int firstIndex = (page - 1) * 5;
			return imageBoxes.GetRange(firstIndex, imagesPerPage);
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

