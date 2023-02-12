using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

public class GlobeConfig
{
    private string jsonFilePath;

    private Dictionary<string, string> configData = new Dictionary<string, string>();

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public bool LoadOrCreateJSONConfig(string newJsonFileName)
    {
        jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), newJsonFileName);
        
        if (File.Exists(jsonFilePath))
        {
            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        else
        {
            configData = new Dictionary<string, string>();
            return SaveJSONConfig();
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public bool SaveJSONConfig()
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(configData);
            File.WriteAllText(jsonFilePath, jsonString);
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public bool SetParam(string name, string value)
    {
        // Sanity checking. This class is for small config items.
        if (name.Length > 256) return false;
        if (value.Length > 1024) return false;

        configData[name] = value;
        return SaveJSONConfig();
    }

    public bool HasParam(string name)
    {
        return configData.ContainsKey(name);
    }

    public string GetParam(string name)
    {
        return configData[name];
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
}
