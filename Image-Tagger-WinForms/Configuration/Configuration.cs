using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
namespace ImageTaggerWinForms
{
	public class Configuration
	{
		public List<String> ValidExtensions { get; set; }
		public bool CheckExtensionsCaseSensitive { get; set; }
		public String DefaultDirectory { get; set; }
		public Size WindowSize { get; set; }
		
		public ConfigurationShortcut Shortcut { get; set; }
		
		public Configuration ()
		{
			Shortcut = new ConfigurationShortcut();			
		}
		
		public void Save(String path)
		{
		    var objStreamWriter = new StreamWriter(path);
		    var serializer = new XmlSerializer(this.GetType());
    		serializer.Serialize(objStreamWriter, this);
    		objStreamWriter.Close();
		}
		
		public static Configuration Load(String path)
		{
			StreamReader objStreamReader = null;
			Configuration config;
			
			try
			{
		    	objStreamReader = new StreamReader(path);
		    	var serializer = new XmlSerializer(typeof(Configuration));
    			config = (Configuration) serializer.Deserialize(objStreamReader);
			}
			catch(InvalidOperationException)
			{
				config = null;
			}
			catch(IOException)
			{
				config = null;
			}
			finally
			{
				if(objStreamReader != null)
				{
					objStreamReader.Close();
				}
			}
			return (Configuration) config;
		}
	}
}

