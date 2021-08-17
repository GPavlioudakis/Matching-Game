using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReader
{

    public Dictionary<string, string> ReadDataFromFile(string path) 
    {
        Dictionary<string, string> animal_url_dict = new Dictionary<string, string>();

        //Read the text from the file
        StreamReader reader = new StreamReader(path);
        string line;
        //Read every line of the file
        while ((line = reader.ReadLine()) != null)
        {
            //Find every ":" and split the file in string arrays
            string[] stringArray = line.Split(char.Parse(":"));
            //Animal name is the first array
            string animal_name = stringArray[0];
            string animal_url;
            //The url starts with "https//:"
            animal_url = stringArray[1] + ":" + stringArray[2];
            animal_url_dict.Add(animal_name, animal_url);
            
            
        }
        reader.Close();

        //Empty file 
        if (animal_url_dict.Count == 0)
            return null;
        
        return animal_url_dict;
    }
}
