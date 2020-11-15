using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_Landmass 
{
    private List<TN_Region> regions;
    private List<TN_Region> neighbours;
    private bool isContinent;
    private int elevationIndex;
    private int mandatoryLandmassSeparation = 3;

    public TN_Landmass(TN_Region region, bool isContinent, int elevationIndex)
    {
        regions = new List<TN_Region>();
        neighbours = new List<TN_Region>();
        this.elevationIndex = elevationIndex;
        this.isContinent = isContinent;

        AddRegion(region);
    }

    public bool AddRegion(TN_Region region)
    {
        
        regions.Add(region);
        region.Landmass = this;
        region.SetElevationIndex(elevationIndex);
        Debug.Log("Adding region");
        return true;
    }

    public bool Expand()
    {
        neighbours.Clear();
        foreach(TN_Region region in regions)
        {
            foreach(TN_Region neighbour in region.neighbours)
            {
                if(neighbour != null && neighbour.Landmass == null && !neighbour.BordersLandmassOtherThan(this, mandatoryLandmassSeparation, neighbour) && !neighbour.IsPolar )
                {
                    neighbours.Add(neighbour);
                    
                }
            }
        }
        if(neighbours.Count == 0)
        {
            return false;
        }
        TN_Region region1 = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
        
        return AddRegion(region1);
    }

    internal void GetMountains()
    {
        foreach (TN_Region region in regions)
        {
            region.GetMountains();
        }
    }
}
