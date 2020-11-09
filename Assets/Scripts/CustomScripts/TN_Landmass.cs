using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_Landmass 
{
    private List<TN_Region> regions;
    private bool isContinent;
    private int elevationIndex;

    public TN_Landmass(TN_Region region, bool isContinent, int elevationIndex)
    {
        regions = new List<TN_Region>();
        this.elevationIndex = elevationIndex;
        this.isContinent = isContinent;
        AddRegion(region);
    }

    public bool AddRegion(TN_Region region)
    {
        if(region.Landmass != null || region.BordersLandmass)
        {
            return false;
        }
        regions.Add(region);
        region.Landmass = this;
        region.SetElevationIndex(elevationIndex);
        return true;
    }
}
