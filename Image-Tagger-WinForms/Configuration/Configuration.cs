using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
namespace ImageTaggerWinForms
{
	public class Configuration
	{
		public List<String> ValidExtensions { get; set; }
		public bool CheckExtensionsCaseSensitive { get; set; }
		public String DefaultDirectory { get; set; }
		public Size WindowSize { get; set; }
		
		public ConfigurationNavigation Navigation { get; set; }
		
		public Configuration ()
		{
			Navigation = new ConfigurationNavigation();			
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
		    var objStreamReader = new StreamReader(path);
		    var serializer = new XmlSerializer(typeof(Configuration));
    		var config = serializer.Deserialize(objStreamReader);
    		objStreamReader.Close();
			return (Configuration) config;
		}
	}
}

