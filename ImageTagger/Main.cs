using System;
using Gtk;
using GLib;

namespace ImageTagger
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			
			MainWindow win = new MainWindow ();
			win.Show ();
			
			UnhandledExceptionHandler h = new UnhandledExceptionHandler (win.OnGTKException);
			ExceptionManager.UnhandledException += h;
			Application.Run ();
		}


	}
	
	
}

