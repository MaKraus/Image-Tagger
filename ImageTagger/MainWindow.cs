using System;
using Gtk;
using GLib;
using Gdk;
using System.Diagnostics;
using System.IO;
using ImageTagger;

public partial class MainWindow : Gtk.Window
{
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		
		this.Title = "Image Tagger";
		
	}

	private void OpenFileDialog ()
	{
		String filename = null;
		FileChooserDialog dia = new FileChooserDialog ("Choose an image", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
		
		if (dia.Run () == (int)ResponseType.Accept) {
			filename = dia.Filename;
		}
		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		dia.Destroy ();
		
		if (filename != null) {
			Pixbuf buffer = new Pixbuf (filename);
			if (buffer.Width >= 0 && buffer.Height >= 0) {
				ImageHelper.ZoomToWindow (buffer, ref imageMain);
			} else {
				MessageDialog warningDia = new MessageDialog (this, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo, "Selected Image (" + filename + ") is invalid or not supported. Select another Image?");
				if (warningDia.Run () == (int)ResponseType.Yes) {
					OpenFileDialog ();
				}
				warningDia.Destroy ();
				
			}
		}
		
	}

	private void OpenDirectoryDialog ()
	{	
		String directoryName = null;
		FileChooserDialog dia = new FileChooserDialog ("Choose an directory", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
		
		if (dia.Run () == (int)ResponseType.Accept) {
			directoryName = dia.Filename;
		}
		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		dia.Destroy ();
		
		if (directoryName == null) {
			return;
		}
		DirectoryInfo directory = new DirectoryInfo (directoryName);
		if (!directory.Exists) {
			return;
		}
		
		FileInfo[] files = directory.GetFiles ("*", SearchOption.AllDirectories);
		
		foreach (var child in hboxThumbnails.Children) {
			hboxThumbnails.Remove (child);
		}
		
		foreach (var file in files) {
			ThumpnailButton thumpnail = new ThumpnailButton(file.FullName);
			thumpnail.Clicked += new EventHandler(OnThumpnailClicked);
			hboxThumbnails.Add (thumpnail);
		}
		hboxThumbnails.ShowAll ();
	}
	
	public void OnGTKException (UnhandledExceptionArgs args)
	{
		String message = "Unhandled Exception. ";
		if(args.ExceptionObject is Exception)
		{
			Exception exp = (Exception) args.ExceptionObject;
			message = message + exp.Message;
			
			if(exp.InnerException != null)
			{
				message = message + exp.InnerException.Message;
			}
		}		
		MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, message);
		errorDialog.Run();
		errorDialog.Destroy();
		
		//	ShowErrorDialog (args.ExceptionObject, args.IsTerminating);
		args.ExitApplication = true;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected virtual void OnThumpnailClicked(object sender, System.EventArgs e)
	{
		ThumpnailButton button = (ThumpnailButton) sender;
		ImageHelper.ZoomToWindow(button.FullSizeBuffer, ref imageMain);
	}
	
	protected virtual void OnQuitActionActivated (object sender, System.EventArgs e)
	{
		OnDeleteEvent (sender, new DeleteEventArgs ());
	}

	protected virtual void OnOpenActionActivated (object sender, System.EventArgs e)
	{
		OpenFileDialog ();
	}

	protected virtual void OnDirectoryActionActivated (object sender, System.EventArgs e)
	{
		OpenDirectoryDialog ();
	}
	
	
	
	
}

