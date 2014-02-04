/*	
	.Ini file Parser
	Author: Tristan 'Kennyist' Cunningham - www.tristanjc.com
	Date: 13/01/2014
	License: Creative Commons ShareAlike 3.0 - https://creativecommons.org/licenses/by-sa/3.0/
	Gihub Contributors: xenonsin, nitz
*/

using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Names of all Ini files, Requires editing for your desired setup.
/// </summary>
public enum IniFiles{
	// Enum is being used so its easy to get the right names from other scripts, Less chance for errors
	// If you wish to not use an Enum, change "(IniFiles file)" to "(String file)" on the methods below 
	// (being: DoesExist, Save and load)
	SETTINGS,
	USERPREFS
}

/// <summary>
/// An .ini file parser that Creates and edits .ini files, With functions to fetch and delete values.
/// </summary>
public class iniParser {

	private List<string> keys = new List<string>();
	private List<string> vals = new List<string>();
	private List<string> comments = new List<string>();
	private List<string> subSections = new List<string>();

	private int commentMargin = 60; // How many characters from the line start should the comment show

	/// <summary>
	/// Initializes a new instance of the <see cref="iniParser"/> class without loading a file.
	/// </summary>
	public iniParser(){}

	/// <summary>
	/// Initializes a new instance of the <see cref="iniParser"/> class with loading a file.
	/// </summary>
	/// <param name="file">Name of the file you want to load.</param>
	public iniParser(IniFiles file){
		Load(file); 
	}

	/// <summary>
	/// Returns true if the file exists, or false if it doesnt.
	/// </summary>
	/// <param name="file">The selected file.</param>
	public bool DoesExist(IniFiles file){
		return File.Exists(Application.dataPath+"/"+file+".ini") ? true : false;
	}

	/// <summary>
	/// Set the variable and value if they dont exist. Updates the variables value if does exist.
	/// </summary>
	/// <param name="subSection">The Section this key belongs to</param>
	/// <param name="key">The variable name</param
	/// <param name="val">The value of the variable</param>
	public void Set(string subSection, string key, string value){
		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key)){
				vals[i] = value;
				subSections[i] = subSection;
				return;
			}
		}

		subSections.Add(subSection);
		keys.Add(key);
		vals.Add(value);
		comments.Add("");
	}

	/// <summary>
	/// Set the variable and value if they dont exist including a comment. Updates the variables value and comment if does exist.
	/// </summary>
	/// <param name="subSection">The Section this key belongs to</param>
	/// <param name="key">The variable name</param>
	/// <param name="val">The value of the variable</param>
	/// <param name="comment">The comment of the variable</param>
	public void Set(string subSection, string key, string value, string comment){
		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key)){
				vals[i] = value;
				subSections[i] = subSection;
				comments[i] = comment;
				return;
			}
		}

		subSections.Add(subSection);
		keys.Add(key);
		vals.Add(value);
		comments.Add(comment);
	}

	/// <summary>
	/// Returns the value for the input variable.
	/// </summary>
	/// <param name="key">The variable name.</param>
	public string Get(string key){
		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key)){
				return vals[i];
			}
		}
		return "";
	}

	/// <summary>
	/// Get the specified key from a subSection. For use if you need to use the same name in differant sections.
	/// </summary>
	/// <param name="subSection">Sub section.</param>
	/// <param name="key">Key.</param>
	public string Get(string subSection, string key){
		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key) && subSections[i].Equals(subSection)){
				return vals[i];
			}
		} 
		return "";
	}

	/// <summary>
	/// Returns the Key, Value, subsection and comment of the choosen variable.
	/// </summary>
	/// <returns>String array containing the 3 values. 0 = subsection, 1 = key, 2 = value, 3 = comment</returns>
	/// <param name="key">The variable name.</param>
	public string[] GetLine(string key){
		string[] list = new string[4];

		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key)){
				list[0] = subSections[i];
				list[1] = keys[i];
				list[2] = vals[i];
				list[3] = comments[i];
				return list;
			}
		}

		return list;
	}

	/// <summary>
	/// Removes the selected Variable including its value and comment.
	/// </summary>
	/// <param name="key">The variable name.</param>
	public void Remove(string key){
		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key)){
				subSections.RemoveAt(i);
				keys.RemoveAt(i);
				vals.RemoveAt(i);
				comments.RemoveAt(i);
				return;
			}
		}
		Debug.LogError("Key not found");
	}

	/// <summary>
	/// Remove the specified key from the specified subSection.
	/// </summary>
	/// <param name="subSection">Sub section name.</param>
	/// <param name="key">The key name</param>
	public void Remove(string subSection, string key){
		for(int i = 0; i < keys.Count; i++){
			if(keys[i].Equals(key) && subSections[i].Equals(subSection)){
				subSections.RemoveAt(i);
				keys.RemoveAt(i);
				vals.RemoveAt(i);
				comments.RemoveAt(i);
				return;
			}
		}
		Debug.LogError("Key not found");
	}

	/// <summary>
	/// Save the specified file.
	/// </summary>
	/// <param name="file">The file name.</param>
	public void Save(IniFiles file){
        using (StreamWriter wr = new StreamWriter(Application.dataPath + "/" + file + ".ini")){
            List<string> noDup = new List<string>();
            for (int i = 0; i < subSections.Count; i++)
            {
                if (!noDup.Contains(subSections[i]))
                {
                    noDup.Add(subSections[i]);
                }
            }
            noDup.Sort();
            List<string> keysC = keys;
            List<string> valsC = vals;
            List<string> comsC = comments;
            List<string> subsC = subSections;
            for (int i = 0; i < noDup.Count; i++)
            {
                int cur = 0;
                while (subsC.Contains(noDup[i]))
                {
                    int pos = subsC.IndexOf(noDup[i]);
                    if (cur == 0)
                    {
                        if (!noDup[i].Equals(""))
                        {
                            wr.WriteLine("\n[" + noDup[i] + "]\n");
                        }
                    }
                    if (!comsC[pos].Equals(""))
                    {
                        string p1 = keysC[pos] + "=" + valsC[pos];
                        int tabs = (commentMargin - p1.Length) / 4;
                        wr.WriteLine(p1 + new string('\t', tabs) + "; " + comsC[pos]);
                    }
                    else
                    {
                        wr.WriteLine(keysC[pos] + "=" + valsC[pos]);
                    }
                    subsC.RemoveAt(pos);
                    keysC.RemoveAt(pos);
                    comsC.RemoveAt(pos);
                    valsC.RemoveAt(pos);
                    cur++;
                }
            }
        }
		Debug.Log(file+".ini Saved");
	}

	/// <summary>
	/// Load the specified file.
	/// </summary>
	/// <param name="file">The file name.</param>
	public void Load(IniFiles file){
		Clear();

		string line = "", dir = Application.dataPath +"/"+ file +".ini", catagory = "";
		int offset = 0, comment = 0, subcat = 0;

		try{
			using(StreamReader sr = new StreamReader(dir)){
				while((line = sr.ReadLine()) != null){
					offset = line.IndexOf("=");
					comment = line.IndexOf(";");
					subcat = line.IndexOf("[");
					if(subcat == 0){
						catagory = line.Substring(1,line.Length -2);
					}
					if(offset > 0){
						if(comment != -1){
							string val = line.Substring(offset+1,(comment - (offset+1)));
							val = val.Replace("\t","");
                            Set(catagory, line.Substring(0, offset), val, line.Substring(comment + 1).TrimStart(' ')); 
						} else {
                            Set(catagory, line.Substring(0, offset), line.Substring(offset + 1));
						}
					}
				}
				Debug.Log(file + " Loaded");
			}
		} catch(IOException e){
			Debug.Log("Error opening "+file+".ini");
			Debug.LogWarning(e);
		}
	}

	/// <summary>
	/// Clear this instance.
	/// </summary>
	public void Clear(){
		keys = new List<string>();
		vals = new List<string>();
		comments = new List<string>();
		subSections = new List<string>();
	}

	/// <summary>
	/// How many keys are stored.
	/// </summary>
	public int Count(){
		return keys.Count;
	}
}
