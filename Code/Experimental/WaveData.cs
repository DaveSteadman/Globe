using System;

using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

// Define the struct that will be used to transmit the data
public struct WaveUDPMsg
{
    public double freqMin;
    public double freqInt;

    public double[] freq;

    public int next;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

public class WaveData
{
    const int msgDataWidth = 100;
    const int dataWidth = 1000;
    const int dataTimeSlices = 100;

    double freqMin = 10;
    double freqInt = 1;

    public double[,] arrData;

    int currTimeIndex = 0;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public WaveData()
    {
        arrData = new double[dataWidth, dataTimeSlices];
        currTimeIndex = 0;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Utilities
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    double GetRandomNumber(double minimum, double maximum)
    {
        Random random = new Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    double BellcurveFraction(double inRatio)
    {
        double outRatio = 0.0;
        if (inRatio < 0.5)
        {
            outRatio = 2.0 * inRatio * inRatio;
        }
        else
        {
            outRatio = 1.0 - (2.0 * (1.0 - inRatio) * (1.0 - inRatio));
        }
        return outRatio;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
    // Timeslice management
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public int GetTimeSliceIndex()
    {
        return currTimeIndex;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public int IndexForFreq(double inFreq)
    {
        int outIndex = (int)((inFreq - freqMin) / freqInt);
        if (outIndex < 0)
            outIndex = 0;
        if (outIndex >= dataWidth)
            outIndex = dataWidth - 1;
        return outIndex;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public double FreqForIndex(int inIndex)
    {
        double outFreq = freqMin + (inIndex * freqInt);
        return outFreq;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public void MoveToNextTimeSlice()
    {
        currTimeIndex++;
        if (currTimeIndex >= dataTimeSlices)
            currTimeIndex = 0;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public int GetPreviousTimeSliceIndex()
    {
        int prevTimeIndex = currTimeIndex - 1;
        if (prevTimeIndex < 0)
            prevTimeIndex = dataTimeSlices - 1;
        return prevTimeIndex;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
    // Define data
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public void BlankAllData()
    {
        for (int i = 0; i < dataWidth; i++)
        {
            for (int j = 0; j < dataTimeSlices; j++)
            {
                arrData[i, j] = 0.0;
            }
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public void SetCurrTimeSliceToNoise()
    {
        for (int i = 0; i < dataWidth; i++)
        {
            arrData[i, currTimeIndex] = GetRandomNumber(0.0, 1.0);
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public void AddPeak(double freq, double amp, double width)
    {
        int FreqMidIndex = (int)(freq - freqMin) / (int)freqInt;
        int FreqHalfWidthIndex = (int)((width / freqInt) / 2);
        int FreqMinIndex = FreqMidIndex - FreqHalfWidthIndex;
        int FreqMaxIndex = FreqMidIndex + FreqHalfWidthIndex;

        if (FreqMinIndex < 0) FreqMinIndex = 0;
        if (FreqMaxIndex >= dataWidth) FreqMaxIndex = dataWidth - 1;

        // create bellcurve distribution of amplitude values across the min to max index
        double[] arrAmp = new double[FreqMaxIndex - FreqMinIndex + 1];
        for (int i = 0; i < arrAmp.Length; i++)
        {
            double fraction = BellcurveFraction((double)i / (double)arrAmp.Length);
            arrAmp[i] = fraction * amp;
        }

        // add the amplitude values to the current time slice
        for (int i = 0; i < arrAmp.Length; i++)
        {
            arrData[FreqMinIndex + i, currTimeIndex] += arrAmp[i];
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Extract Data
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    // Get values for current time slice
    public double[] CurrTimeSliceData()
    {
        double[] arr = new double[dataWidth];
        for (int i = 0; i < dataWidth; i++)
        {
            arr[i] = arrData[i, currTimeIndex];
        }
        return arr;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public WaveUDPMsg GetCurrTimeSliceDataAsWaveUDPMsg(double fMin)
    {
        int FreqMinIndex = (int)(fMin - freqMin) / (int)freqInt;
        if (FreqMinIndex < 0) FreqMinIndex = 0;

        double[] dataExtract = new double[msgDataWidth];

        for (int i = 0; i < msgDataWidth; i++)
            dataExtract[i] = arrData[FreqMinIndex + i, currTimeIndex];

        WaveUDPMsg msg = new WaveUDPMsg();
        msg.freqMin = fMin;
        msg.freqInt = freqInt;
        msg.freq = dataExtract;

        return msg;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
    // Input new data
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    public void AddWaveUDPMsg(WaveUDPMsg msg)
    {
        int FreqMinIndex = (int)(msg.freqMin - freqMin) / (int)freqInt;
        if (FreqMinIndex < 0) FreqMinIndex = 0;

        for (int i = 0; i < msgDataWidth; i++)
            arrData[FreqMinIndex + i, currTimeIndex] = msg.freq[i];

        if (msg.next > 0)
        {
            MoveToNextTimeSlice();
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

}
