using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageTaggerWinForms
{
	public class ImageTagger : Form
	{
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
			leftRightSplitter.Panel1MinSize = 310;
			leftRightSplitter.Panel1.BackColor = Color.AntiqueWhite;
			FlowLayoutPanel imageList = new FlowLayoutPanel();
			imageList.FlowDirection = FlowDirection.TopDown;
			
			imageList.SuspendLayout();
			imageList.Dock = DockStyle.Fill;
			imageList.AutoScroll = true;
			PictureBox image1 = new PictureBox();
			image1.Width = 300;
			image1.Height = 300;
				
			image1.Load("/storage/bilder/Digicam Nachbearbeitet/NachDemRegen.JPG");
			imageList.Controls.Add(image1);
			
			leftRightSplitter.Panel1.Controls.Add(imageList);
			imageList.ResumeLayout();
			
			// Panel 2
			TextBox hello = new TextBox();
			hello.Text = "Hello World!";
			leftRightSplitter.Panel2.Controls.Add(hello);
						
			leftRightSplitter.ResumeLayout(false);
			this.ResumeLayout(false);
		}
	}
}

