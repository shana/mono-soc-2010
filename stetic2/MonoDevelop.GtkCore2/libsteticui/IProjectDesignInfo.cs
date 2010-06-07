using System;

namespace Stetic
{
	//Provides access to informations menaged by ide
	public interface IProjectDesignInfo
	{
		//Returns component source file folder
		string GetComponentFolder(string componentName);
		
		//Search for all components source file folders 
		string[] GetComponentFolders ();
		
	}
}

