using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageTaggerWinForms
{
	public class ImageTagger : Form
	{
		PictureBox mainImage = new PictureBox();
		
		public static void Main ()
		{
			Application.Run (new ImageTagger ());
		}

		public ImageTagger ()
		{
			this.SuspendLayout();
			
			// Window properties
			this.Width = 800;
			this.Height = 800;
			this.Text = "Image-Tagger Playground";
			
			// left/right Splitcontainer 
			SplitContainer leftRightSplitter = new SplitContainer();
			Controls.Add(leftRightSplitter);
			leftRightSplitter.SuspendLayout();
			leftRightSplitter.Dock = DockStyle.Fill;
			leftRightSplitter.IsSplitterFixed = true;
						
			// Panel 1
			leftRightSplitter.Panel1MinSize = 210;
			leftRightSplitter.Panel1.BackColor = Color.AntiqueWhite;
			FlowLayoutPanel imageList = new FlowLayoutPanel();
			imageList.FlowDirection = FlowDirection.LeftToRight;
			
			
			imageList.SuspendLayout();
			imageList.Dock = DockStyle.Fill;
			imageList.AutoScroll = true;
			
			DirectoryInfo imageDir = new DirectoryInfo("/storage/bilder/Digicam Nachbearbeitet/");
			var files = imageDir.GetFiles("*", SearchOption.AllDirectories);
			
			foreach(var file in files)
			{
				if(file.Extension.ToLower() != ".jpg")
				{
					continue;
				}
				
				PictureBox image1 = new PictureBox();
				image1.Width = 200;
				image1.Height = 150;
				image1.SizeMode = PictureBoxSizeMode.Zoom;
				image1.Click += HandleImage1Click;
				
				image1.Load(file.FullName);
				imageList.Controls.Add(image1);
			}
			leftRightSplitter.Panel1.Controls.Add(imageList);
			imageList.ResumeLayout();
			
			// Panel 2
			mainImage.Dock = DockStyle.Fill;
			mainImage.SizeMode = PictureBoxSizeMode.Zoom;
						
			leftRightSplitter.Panel2.Controls.Add(mainImage);
						
			leftRightSplitter.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		void HandleImage1Click (object sender, EventArgs e)
		{
			if(sender is PictureBox)
			{
				PictureBox listImage = (PictureBox) sender;	
				mainImage.Load(listImage.ImageLocation);				
			}
		}
	}
}

