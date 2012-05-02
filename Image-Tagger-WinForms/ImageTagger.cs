using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageTaggerWinForms
{
	public class ImageTagger : Form
	{
		private System.ComponentModel.IContainer components = null;

		ImageBrowser imageBrowser;
		PictureBox mainImage = new PictureBox ();

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		public ImageTagger ()
		{
			// Window properties
			this.components = new System.ComponentModel.Container ();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Width = 1200;
			this.Height = 850;
			this.Text = "Image-Tagger Playground";
			
			this.KeyPreview = true;
			this.KeyPress += new KeyPressEventHandler(HandleKeyPress);
			
			// Create Menu
			CreateMenuStrip ();
			
			// left/right Splitcontainer 
			SplitContainer leftRightSplitter = new SplitContainer ();
			leftRightSplitter.Dock = DockStyle.Fill;
			this.Controls.Add (leftRightSplitter);
			
			// Panel 1
			leftRightSplitter.Panel1MinSize = 210;
			leftRightSplitter.SplitterDistance = 210;
			leftRightSplitter.Panel1.BackColor = Color.AntiqueWhite;
			
			// Init ImageBrowser
			imageBrowser = new ImageBrowser ();
			imageBrowser.Dock = DockStyle.Fill;
			leftRightSplitter.Panel1.Controls.Add (imageBrowser);
			imageBrowser.ImageClick += HandleImage1Click;
			
			// Panel 2
			mainImage.Dock = DockStyle.Fill;
			mainImage.SizeMode = PictureBoxSizeMode.Zoom;
			
			leftRightSplitter.Panel2.Controls.Add (mainImage);
		}

		private void HandleKeyPress (Object sender, KeyPressEventArgs e)
		{
			// The keypressed method uses the KeyChar property to check 
			// whether the ENTER key is pressed.  
			
			// If the ENTER key is pressed, the Handled property is set to true, 
			// to indicate the event is handled.
			if (e.KeyChar == 'w') {
				imageBrowser.PreviousImage();
				e.Handled = true;
			}
			else if (e.KeyChar == 's') {
				imageBrowser.NextImage();
				e.Handled = true;
			}
			else if (e.KeyChar == 'a') {
				imageBrowser.PreviousPage();
				e.Handled = true;
			}
			else if (e.KeyChar == 'd') {
				imageBrowser.NextPage();
				e.Handled = true;
			}
		}

		protected void CreateMenuStrip ()
		{
			// Main Menu
			MenuStrip MainMenu = new MenuStrip ();
			this.Controls.Add (MainMenu);
			
			// File Menu
			ToolStripMenuItem File = new ToolStripMenuItem ("File");
			MainMenu.Items.Add (File);
			
			ToolStripMenuItem OpenDirectory = new ToolStripMenuItem ("Open Directory");
			OpenDirectory.Click += new EventHandler (OpenDirectory_Click);
			File.DropDownItems.Add (OpenDirectory);
			
		}

		void OpenDirectory_Click (object sender, EventArgs e)
		{
			String path = "";
			FolderBrowserDialog dialog = new FolderBrowserDialog ();
			var dialogResult = dialog.ShowDialog ();
			
			if (dialogResult == DialogResult.OK) {
				path = dialog.SelectedPath;
			} else {
				this.Close ();
			}
			
			DirectoryInfo imageDir = new DirectoryInfo (path);
			var files = imageDir.GetFiles ("*", SearchOption.AllDirectories);
			
			imageBrowser.Clear ();
			foreach (var file in files) {
				if (!CheckExtension (file.Extension)) {
					continue;
				}
				
				imageBrowser.Add (file);
				Application.DoEvents ();
			}
		}

		public bool CheckExtension (String extension)
		{
			String lowerExtension = extension.ToLower ();
			
			if (lowerExtension == ".jpg" || lowerExtension == ".jpeg" || lowerExtension == ".png" || lowerExtension == ".tif" || lowerExtension == ".tiff") {
				return true;
			} else {
				return false;
			}
		}

		void HandleImage1Click (object sender, ImageClickEventArgs e)
		{
			if (e != null && e.Information != null && !String.IsNullOrEmpty (e.Information.Path)) {
				mainImage.Load (e.Information.Path);
			} else if (e != null && e.Thumbnail != null) {
				mainImage.Image = e.Thumbnail;
			}
		}
	}
}
