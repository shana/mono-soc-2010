using System;

namespace Stetic
{
	//Provides access to informations menaged by ide
	public interface IProjectDesignInfo
	{
		//Returns component source file for given component
		string GetComponentFile (string componentName);
		bool HasComponentFile (string componentFile);
		
		//Returns gtkx file name for given component file
		string GetGtkxFile (string componentFile);

		//Search for all components source file folders 
		string[] GetComponentFolders ();
		
	}
}
