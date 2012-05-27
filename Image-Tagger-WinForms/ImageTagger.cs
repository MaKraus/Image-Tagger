using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace ImageTaggerWinForms
{
	public class ImageTagger : Form
	{
		private System.ComponentModel.IContainer components = null;

		Configuration config = new Configuration ();

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
			LoadingScreen loadingScreen = new LoadingScreen();
			loadingScreen.Show();
			
			// Configuration
			config = GetConfiguration ();
			
			// Window properties
			this.components = new System.ComponentModel.Container ();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "Image-Tagger Playground";

			if(config.WindowSize.Width != 0 && config.WindowSize.Height != 0)
			{
				this.Width = config.WindowSize.Width;
				this.Height = config.WindowSize.Height;
			}
			
			this.KeyPreview = true;
			this.KeyPress += new KeyPressEventHandler (HandleKeyPress);
			this.Resize += new EventHandler(Form_Resize);
			
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
			
			if(!String.IsNullOrEmpty(config.DefaultDirectory))
			{
				OpenDirectory(config.DefaultDirectory);	
			}
			
			// Panel 2
			mainImage.Dock = DockStyle.Fill;
			mainImage.SizeMode = PictureBoxSizeMode.Zoom;
			
			leftRightSplitter.Panel2.Controls.Add (mainImage);
			
			loadingScreen.Hide();
		}

		public Configuration GetConfiguration ()
		{
			try {
				return LoadConfiguration();
			} catch (IOException) {
				var config = new Configuration 
				{ 
					CheckExtensionsCaseSensitive = false, 
					ValidExtensions = new List<String> { ".jpg", ".jpeg", ".png", ".tiff", ".gif" }, 
					Navigation = new ConfigurationNavigation { PreviousImage = 'a', NextImage = 'd', PreviousPage = 'w', NextPage = 's' },
					WindowSize = new Size(1253, 810)			
				};
				return config;
			}					
		}

		protected void CreateMenuStrip ()
		{
			// Main Menu
			MenuStrip MainMenu = new MenuStrip ();
			this.Controls.Add (MainMenu);
			
			// File Menu
			ToolStripMenuItem file = new ToolStripMenuItem ("File");
			MainMenu.Items.Add (file);
			
			ToolStripMenuItem OpenDirectory = new ToolStripMenuItem ("Open Directory");
			OpenDirectory.Click += new EventHandler (OpenDirectory_Click);
			file.DropDownItems.Add (OpenDirectory);
			
			ToolStripMenuItem LoadConfiguration = new ToolStripMenuItem ("Load Configuration");
			LoadConfiguration.Click += new EventHandler (LoadConfiguration_Click);
			file.DropDownItems.Add (LoadConfiguration);
			
			ToolStripMenuItem SaveConfiguration = new ToolStripMenuItem ("Save Configuration");
			SaveConfiguration.Click += new EventHandler (SaveConfiguration_Click);
			file.DropDownItems.Add (SaveConfiguration);
			
			// Navigation Menu
			ToolStripMenuItem navigation = new ToolStripMenuItem ("Navigation");
			MainMenu.Items.Add (navigation);
			
			ToolStripMenuItem previousImage = new ToolStripMenuItem ("Previous Image");
			navigation.DropDownItems.Add (previousImage);
			
			ToolStripMenuItem nextImage = new ToolStripMenuItem ("Next Image");
			navigation.DropDownItems.Add (nextImage);
			
			ToolStripMenuItem previousPage = new ToolStripMenuItem ("Previous Page");
			navigation.DropDownItems.Add (previousPage);
			
			ToolStripMenuItem nextPage = new ToolStripMenuItem ("Next Page");
			navigation.DropDownItems.Add (nextPage);
			
			// About Menu
			ToolStripMenuItem about = new ToolStripMenuItem ("About");
			MainMenu.Items.Add (about);
			
			ToolStripMenuItem feedback = new ToolStripMenuItem ("Feedback / Bugreport");
			feedback.Click += new EventHandler (OpenFeedback_Click);
			about.DropDownItems.Add (feedback);
			
			ToolStripMenuItem source = new ToolStripMenuItem ("Source");
			source.Click += new EventHandler (OpenSource_Click);
			about.DropDownItems.Add (source);
			
			ToolStripMenuItem licence = new ToolStripMenuItem ("Licence");
			licence.Click += new EventHandler (OpenLicence_Click);
			about.DropDownItems.Add (licence);
		}

		#region Main Form Events
		
		#endregion
		
		void Form_Resize (object sender, EventArgs e)
		{
			config.WindowSize = new Size(this.Width, this.Height);
		}
		
		#region File menu handler

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
			
			OpenDirectory(path);
		}

		void SaveConfiguration_Click (object sender, EventArgs e)
		{
			try {
				SaveConfiguration ();
				MessageBox.Show ("Configuration saved");
			} catch (IOException) {
				MessageBox.Show ("Error saving configuration");
			}
		}

		void LoadConfiguration_Click (object sender, EventArgs e)
		{
			try {
				config = LoadConfiguration ();
				MessageBox.Show ("Configuration loaded");
			} catch (IOException) {
				MessageBox.Show ("Error loading configuration");
			}
		}

		#endregion

		#region About menu handler

		void OpenFeedback_Click (object sender, EventArgs e)
		{
			String feedbackText = @"I'm happy to get feedback about this product. 
Please use the issue managment at gitHub to fill in your feedback and bugreports.
https://github.com/MaKraus/Image-Tagger";
			
			MessageBox.Show (feedbackText, "Feedback / Bugreport");
		}

		void OpenSource_Click (object sender, EventArgs e)
		{
			String sourceText = @"Image-Tagger source code may be picked up at github. 
Please feel free to contribute.
https://github.com/MaKraus/Image-Tagger";
			
			MessageBox.Show (sourceText, "Image-Tagger source code (gitHub)");
		}

		void OpenLicence_Click (object sender, EventArgs e)
		{
			String licenceText = @"Copyright (c) 2012, MaKraus & Contributors
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met: 

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer. 
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 

 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 'AS IS' AND
 ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
";
			MessageBox.Show (licenceText, "Image-Tagger licence (BSD-2-Clause)");
		}

		#endregion

		#region Navigation handler

		private void HandleKeyPress (Object sender, KeyPressEventArgs e)
		{
			var keys = config.Navigation;
			
			// The keypressed method uses the KeyChar property to check 
			// whether the ENTER key is pressed.  
			
			// If the ENTER key is pressed, the Handled property is set to true, 
			// to indicate the event is handled.
			if (e.KeyChar == keys.PreviousImage) {
				imageBrowser.PreviousImage ();
				e.Handled = true;
			} else if (e.KeyChar == keys.NextImage) {
				imageBrowser.NextImage ();
				e.Handled = true;
			} else if (e.KeyChar == keys.PreviousPage) {
				imageBrowser.PreviousPage ();
				e.Handled = true;
			} else if (e.KeyChar == keys.NextPage) {
				imageBrowser.NextPage ();
				e.Handled = true;
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

		#endregion

		public bool CheckExtension (String extension)
		{
			String modifiedExtension;
			if (config.CheckExtensionsCaseSensitive) {
				modifiedExtension = extension;
			} else {
				modifiedExtension = extension.ToLower ();
			}
			
			if (config.ValidExtensions != null && config.ValidExtensions.Contains (modifiedExtension)) {
				return true;
			} else {
				return false;
			}
		}

		public void SaveConfiguration ()
		{
			String basePath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			String applicationName = Application.ProductName;
			String folder = Path.Combine (basePath, applicationName);
			
			if (!Directory.Exists (folder)) {
				Directory.CreateDirectory (folder);
			}
			
			String path = Path.Combine (folder, ".ImageTagger.config");
			this.config.Save (path);
		}

		public Configuration LoadConfiguration ()
		{
			String basePath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			String applicationName = Application.ProductName;
			String folder = Path.Combine (basePath, applicationName);
			
			String path = Path.Combine (folder, ".ImageTagger.config");
			return config = Configuration.Load (path);
		}
		
		public void OpenDirectory(String path)
		{
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
	}
}
