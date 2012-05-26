using System;
using System.Windows.Forms;
namespace ImageTaggerWinForms
{
	public class LoadingScreen : Form
	{
		public LoadingScreen ()
		{
			this.Text = "Image-Tagger: Loading...";
			
			TextBox box = new TextBox();
			box.Text = "Please wait while loading...";
			box.Dock = DockStyle.Fill;
			this.Controls.Add(box);
		}
	}
}

