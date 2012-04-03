using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageTaggerWinForms
{
	public class ImageTagger : Form
	{
        private System.ComponentModel.IContainer components = null;

		PictureBox mainImage = new PictureBox();

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		public ImageTagger ()
		{
			// Window properties
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Width = 800;
			this.Height = 800;
			this.Text = "Image-Tagger Playground";
			
			// left/right Splitcontainer 
			SplitContainer leftRightSplitter = new SplitContainer();
            leftRightSplitter.Dock = DockStyle.Fill;
			this.Controls.Add(leftRightSplitter);
						
			// Panel 1
			leftRightSplitter.Panel1MinSize = 210;
            leftRightSplitter.SplitterDistance = 210;
			leftRightSplitter.Panel1.BackColor = Color.AntiqueWhite;
			FlowLayoutPanel imageList = new FlowLayoutPanel();
			imageList.FlowDirection = FlowDirection.LeftToRight;
						
			imageList.Dock = DockStyle.Fill;
			imageList.AutoScroll = true;

            String path = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            var dialogResult = dialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }
            else
            {
                this.Close();
            }

            DirectoryInfo imageDir = new DirectoryInfo(path);
			var files = imageDir.GetFiles("*", SearchOption.AllDirectories);
			
			foreach(var file in files)
			{
                if (!CheckExtension(file.Extension))
				{
					continue;
				}

                int width = 200;
                int height = 150;

                var thumbnail = CreateThumbnail(file.FullName, width, height);
                var pictureBox = CreatePictureBox(thumbnail, width, height);

				imageList.Controls.Add(pictureBox);

                Application.DoEvents();
 
			}
			leftRightSplitter.Panel1.Controls.Add(imageList);
			imageList.ResumeLayout();
			
			// Panel 2
			mainImage.Dock = DockStyle.Fill;
			mainImage.SizeMode = PictureBoxSizeMode.Zoom;
						
			leftRightSplitter.Panel2.Controls.Add(mainImage);
		}

        public bool CheckExtension(String extension)
        {
            String lowerExtension = extension.ToLower();

            if (lowerExtension == ".jpg"
                    || lowerExtension == ".jpeg"
                    || lowerExtension == ".png"
                    || lowerExtension == ".tif"
                    || lowerExtension == ".tiff")
            {
                return true;
            }
            else
            { 
                return false; 
            }
        }

        public PictureBox CreatePictureBox(Image image, int width, int height)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Width = width;
            pictureBox.Height = height;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Click += HandleImage1Click;
            pictureBox.Image = image;
            return pictureBox;
        }

        public Image CreateThumbnail(String path, int width, int height)
        {
            Image fullSizeImage = new Bitmap(path);
            Image scaledImage = new Bitmap(fullSizeImage, width, height);
            scaledImage.Tag = new ImageInformation()
            {
                Path = path
            }; 
            fullSizeImage.Dispose();
            return scaledImage;
        }

		void HandleImage1Click (object sender, EventArgs e)
		{
            if (sender is PictureBox)
            {
                PictureBox listImage = (PictureBox)sender;

                if (listImage.Image != null && listImage.Image.Tag is ImageInformation)
                {
                    mainImage.Load(((ImageInformation)listImage.Image.Tag).Path);
                }
                else
                {
                    mainImage.Image = listImage.Image;
                }
            }
		}
	}
}

