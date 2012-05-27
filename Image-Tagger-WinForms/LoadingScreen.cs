using System;
using System.Windows.Forms;
using System.Drawing;

namespace ImageTaggerWinForms
{
	public class LoadingScreen : Form
	{
		public LoadingScreen ()
		{
			this.Text = "Image-Tagger: Loading...";
			
			var label = new Label();
			label.Text = "Please wait while loading...";
			label.Dock = DockStyle.Fill;
			label.TextAlign = ContentAlignment.MiddleCenter;
			this.Controls.Add(label);
		}
	}
}

