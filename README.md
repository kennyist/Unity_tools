Unity_tools
===========

A set of tools for use on Unity3D  
  
---  

##Current Tools:  
  
**Parsers:**  
  
iniParser 

---  

##iniParser:  

This script is used to easily create and manage .ini files for your game, .ini files are usually used to store application settings, like Graphics and controls, but can be used for many other things as it is just a variable storage device. This script allows you to easily add comments to each key to help anyone who may edit it as well as providing section support for key grouping.

---

##iniPareser Usage:  

This script can be called without being attached to a game object from another script like so:

    iniParser parser = new iniParser(); // Initialize without loading a file
    iniParser parser = new iniPareser(IniFiles.FILENAME); // Initialize with a file name
    
The IniFiles is an Enum to store all your file names so they can be accessed from other scripts easily, Without the chance for entering the wrong name and creating errors.
To start creating keys and values into the .ini file you use "Set", Set has two overloads:

    parser.Set("KEY SUBSECTION", "KEY NAME", "KEY VALUE");
    parser.Set("KEY SUBSECTION", "KEY NAME", "KEY VALUE", "KEY COMMENT");
    
Once you have all the keys created you have to save the changes with:

    parser.Save(Inifiles.FILENAME);
	
You can still Edit and Add keys without needing to load the file again. To load files you use:

    parser.Load(Inifiles.FILENAME);
    
To start Fetching key values you use "Get", But there are two options for this:

    parser.Get("KEY NAME"); // Will just fetch the first key it finds with this name
    parser.Get("SUB SECTION", "KEY NAME"); // This should be used if you have multiple keys with the same name but in different sections
    
You are also able to get everything associated with the key using:

    parser.GetLine("KEY NAME"); 
    
This returns a string array of 4 values: 0 = Subsection, 1 = Key name, 2 = key Value and 3 = Key comment.  
  
To remove keys you use:

    parser.Remove("KEY NAME");
	parser.Remove("SUB SECTION", "KEY NAME"); // Use this if you have keys with the same name in different sections
    
Because files may not even exist yet there is a bool method to test if the file exists:

    if(parser.DoesExist(Inifiles.FILENAME)){
        // does exist
    } else {
        // doesnt exist
    }
  
Lastly, You can now Clear the current instance of all keys and values so you can start a new file without calling iniParser again:

    parser.Clear();  
  
 ---  
 
##iniParser Example:  
 
If i wanted to create a settings file for example i would do:  
 
    iniParser parser = new iniParser();
    parser.Set("resolution","graphics","1920x1080");
    parser.Set("antiAlias","graphics","8","Can only be: 0, 1, 2, 4, 8");
    parser.Set("UncatagorizedKey1","","1","Keys can be created without sections and will be displayed that the top");
    parser.Set("UncatagorizedKey2","","2","Sections are sorted by name, Keys are not");
    parser.Set("mode","audio","sterio");
    parser.Set("masterLevel","audio","100","Can only be 0 - 100");
    parser.Set("musicLevel","audio","75");
    parser.Save(IniFiles.SETTINGS);
    
And the output file will look like:

    UncatagorizedKey1=1                                     ; Keys can be created without sections and will be displayed that the top
    UncatagorizedKey2=2                                     ; Sections are sorted by name, Keys are not
    
    [audio]
    
    mode=sterio
    masterLevel=100                                         ; Can only be 0 - 100
    musicLevel=75
    
    [graphics]
    
    resolution=1920x1080
    antiAlias=8                                             ; Can only be: 0, 1, 2, 4, 8
---
