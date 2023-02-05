
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Int2DArray
{
    private int[,] AltGridList;
    private int listsizeX;
    private int listsizeY;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    // Gridsize = number of points on any size of the square grid, say 20.
    // Will be indexed by 0->19 as any normal array.
    public Int2DArray(int inSizeX, int inSizeY)
    {
        listsizeX = inSizeX;
        listsizeY = inSizeY;
        AltGridList = new int[listsizeX, listsizeY];
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public void SetVal(int x, int y, int val)
    {
        if (x >= listsizeX) return;
        if (y >= listsizeY) return;

        y = (listsizeY - 1) - y; // invert the y axis, to place 0,0 at bottom left
        AltGridList[x, y] = val;
    }

    public int GetVal(int x, int y)
    {
        if (x >= listsizeX) return 0;
        if (y >= listsizeY) return 0;

        y = (listsizeY - 1) - y; // invert the y axis, to place 0,0 at bottom left
        return AltGridList[x, y];
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    // Quick* routines remove the checks, for when called extensively
    public void QuickSetVal(int x, int y, int val)
    {
        y = (listsizeY - 1) - y; // invert the y axis, to place 0,0 at bottom left
        AltGridList[x, y] = val;
    }

    public int QuickGetVal(int x, int y)
    {
        y = (listsizeY - 1) - y; // invert the y axis, to place 0,0 at bottom left
        return AltGridList[x, y];
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    // Assign a new set of values into a larger parent, defining a bottom right starting pos.
    public bool SetPatch(Int2DArray newPatch, int blX, int blY)
    {
        // Check the list will fit, or return false.
        int trX = blX + (newPatch.listsizeX - 1);
        int trY = blY + (newPatch.listsizeY - 1);
        if ((trX > listsizeX) || (trY > listsizeY))
            return false;

        int destX = 0;
        int destY = 0;
        for (int y = 0; y < newPatch.listsizeY; y++)
        {
            destY = blY + y;
            for (int x = 0; x < newPatch.listsizeX; x++)
            {
                destX = blX + x;
                AltGridList[destX, destY] = newPatch.AltGridList[x, y];
            }
        }
        return true;
    }

    public bool GetPatch(int blX, int blY, int patchWidth, int patchHeight, out Int2DArray outPatch)
    {
        // Create the required out value
        outPatch = new Int2DArray(patchWidth, patchHeight);

        // check the size will fit.
        int trX = blX + (patchWidth - 1);
        int trY = blY + (patchHeight - 1);
        if ((trX > listsizeX) || (trY > listsizeY))
            return false;

        for (int y = 0; y < patchHeight; y++)
        {
            int srcY = blY + y;
            for (int x = 0; x < patchWidth; x++)
            {
                int srcX = blX + x;
                outPatch.AltGridList[x, y] = AltGridList[srcX, srcY];
            }
        }
        return true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public void InitZero()
    {
        for (int y = 0; y < listsizeY; y++)
            for (int x = 0; x < listsizeX; x++)
                AltGridList[x, y] = 0;
    }

    public bool IsZero()
    {
        for (int y = 0; y < listsizeY; y++)
            for (int x = 0; x < listsizeX; x++)
                if (AltGridList[x, y] != 0) return false;
        return true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public void SetAllVals(int val)
    {
        for (int y = 0; y < listsizeY; y++)
            for (int x = 0; x < listsizeX; x++)
                AltGridList[x, y] = val;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public string GridToCSVString(bool flipYaxis = false)
    {
        string RetStr = "";
        int lastXIndex = listsizeX - 1;
        int lastYIndex = listsizeY - 1;

        for (int y = 0; y <= lastYIndex; y++)
        {
            int y2 = (flipYaxis) ? lastYIndex - y : y;
            string currline = "";

            for (int x = 0; x < lastXIndex; x++) // The < and <= used in these two loops is intentional
            {
                currline += AltGridList[x, y2];
                currline += ", ";
            }
            currline += AltGridList[lastXIndex, y2];
            currline += "\n";

            RetStr += currline;
        }
        return RetStr;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public bool PopulateGridFromCSVString(string InStr, int inSizeX, int inSizeY)
    {
        AltGridList = new int[inSizeX, inSizeY];

        string[] strLines = InStr.Split('\n');

        // Check we have enough lines of data to read from, not checking further than that.
        if (strLines.Length < inSizeY) return false;

        int currX = 0;
        int currY = 0;
        for (int currLineId = 0; currLineId < inSizeY; currLineId++)
        {
            // Split the line into strings for each value
            string currline = strLines[currLineId];
            string[] currLineValues = currline.Split(',');

            // Check we have the right number of values
            if (currLineValues.Length != inSizeX) return false;

            Int32 newVal = 0;
            foreach (string currVal in currLineValues)
            {
                newVal = Int32.Parse(currVal);
                AltGridList[currX, currY] = newVal;
                currX++;
            }
            currY++;
            currX = 0;
        }
        return true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public bool GridToBinary(string fileName)
    {
        using (var stream = File.Open(fileName, FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(listsizeX);
                writer.Write(listsizeY);

                for (int y = 0; y < listsizeY; y++)
                    for (int x = 0; x < listsizeX; x++)
                        writer.Write(AltGridList[x, y]);
            }
        }
        return true;
    }

    public bool GridFromBinary(string fileName)
    {
        if (File.Exists(fileName))
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    listsizeX = reader.ReadInt32();
                    listsizeY = reader.ReadInt32();

                    AltGridList = new int[listsizeX, listsizeY];

                    for (int y = 0; y < listsizeY; y++)
                        for (int x = 0; x < listsizeX; x++)
                            AltGridList[x, y] = reader.ReadInt32();
                }
            }
        }
        return true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public Int2DArray GetSampledGrid(int skipStep)
    {
        // contruct a list of the indicies we'll use (and avoid complexities of dividing random integer values).
        var xindicies = new List<int>();
        var yindicies = new List<int>();
        for (int x = 0; x < listsizeX; x += skipStep) xindicies.Add(x);
        for (int y = 0; y < listsizeY; y += skipStep) yindicies.Add(y);

        int sampledXsize = xindicies.Count;
        int sampledYsize = yindicies.Count;

        // Create the new object with the list sizes (so we know they'll match)
        Int2DArray sampledArr = new Int2DArray((int)sampledXsize, (int)sampledYsize);

        // Loop thruogh and populate the new 2D array.
        for (int x = 0; x < sampledXsize; x++)
        {
            int xindex = (int)xindicies[x];

            for (int y = 0; y < sampledYsize; y++)
            {
                int yindex = (int)yindicies[y];

                sampledArr.SetVal((int)x, (int)y, AltGridList[xindex, yindex]);
            }
        }

        return sampledArr;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    // upscale the array to a larger size by repeating the values.

    public Int2DArray GetScaledGrid(int repSteps)
    {
        int repXsize = listsizeX * repSteps;
        int repYsize = listsizeY * repSteps;

        // Create the new object with the list sizes (so we know they'll match)
        Int2DArray repArr = new Int2DArray((int)repXsize, (int)repYsize);

        // Loop thruogh and populate the new 2D array.
        for (int x = 0; x < repXsize; x++)
        {
            int xindex = (int)(x / repSteps);

            for (int y = 0; y < repYsize; y++)
            {
                int yindex = (int)(y / repSteps);

                repArr.SetVal((int)x, (int)y, AltGridList[xindex, yindex]);
            }
        }

        return repArr;
    }
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 