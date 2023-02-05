using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePathManager : MonoBehaviour
{
    public static string MapRootDir = "C:/Utils/GlobeLibrary/Maps/";

    public static string[] mapSubpathPerLvl = { "Level1_45x40Degs/", "Level2_5x5Degs/", "Level3_1x1Degs/", "Level4_0p1x0p1Degs/"};

    public const string Filename_SeaTile_5deg_Csv   = "LandSea_5Deg.csv";
    public const string Filename_SeaTile_1deg_Csv   = "LandSea_1Deg.csv";
    public const string Filename_SeaTile_0p1deg_Csv = "LandSea_0p1Deg.csv";

    public const string Filename_SeaTile_5deg_Binary   = "LandSea_5Deg.bin";
    public const string Filename_SeaTile_1deg_Binary   = "LandSea_1Deg.bin";
    public const string Filename_SeaTile_0p1deg_Binary = "LandSea_0p1Deg.bin";



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
