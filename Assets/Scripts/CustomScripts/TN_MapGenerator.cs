using System;
using System.Collections.Generic;
using UnityEngine;

public class TN_MapGenerator : MonoBehaviour
{
    private int maxNumAttempts = 100;
    public int numPlates;
    public int cellsPerRegion;
    private List<TN_PlateTectonic> plates;
    private List<TN_Region> regions;
    private List<TN_Landmass> landmasses;

    public int numContinents;
    public int numIslands;

    public float landmassPercentage;
    private int regionLandBudget;
    
    public HexGrid hexGrid;
    internal void GenerateMap(int x, int z, bool wrapping)
    {
        hexGrid.CreateMap(x, z, wrapping);
        regions = new List<TN_Region>();
        landmasses = new List<TN_Landmass>();
        plates = new List<TN_PlateTectonic>();
        GenerateLand();


    }

    void GenerateLand()
    {
        int cellCount = hexGrid.cellCountX * hexGrid.cellCountZ;
        GenerateCells(cellCount);
        GeneratePlateTectonicSeeds(cellCount);
        ExpandPlateTectonics();
        GenerateRegionSeeds();
        ExpandRegions();
        FindRegionNeighbours();
        //GetMountains();

        
        CreateLandmasses();
        ExpandLandmasses();
        
        

        

       

        

        


    }

    private void CreateLandmasses()
    {
        foreach (TN_PlateTectonic plate in plates)
        {
            foreach (TN_Region region0 in plate.Regions)
            {
                regions.Add(region0);
            }
        }
        
        TN_Region region;
        TN_Landmass continent;
        int numAttempts = 0;
        Debug.Log("Regions num:" +regions.Count);
        while (landmasses.Count < numContinents && numAttempts < maxNumAttempts)
        {
            numAttempts++;
            region = regions[UnityEngine.Random.Range(0, regions.Count-1)];
            if (!region.IsPolar && region.Landmass == null && !region.BordersLandmass())
            {
                continent = new TN_Landmass(region, true, 2);
                landmasses.Add(continent);
            }

        }
        Debug.Log("Attempts: " + numAttempts);
        Debug.Log("Generated continents: "+landmasses.Count);
    }

    private void ExpandLandmasses()
    {
        int numAttempts = 0;
        regionLandBudget = (int)(regions.Count * landmassPercentage);
        Debug.Log("LandBudget: " + regionLandBudget);
        while (regionLandBudget > 0 && numAttempts < maxNumAttempts)
        {
            numAttempts++;
            Debug.Log("Attempt: " + numAttempts);
            foreach (TN_Landmass continent in landmasses)
            {
                if (continent.Expand())
                {
                    regionLandBudget--;
                    Debug.Log("LandBudget: " + regionLandBudget);
                }
            }
        }
    }

    private void GeneratePlateTectonicSeeds(int cellCount)
    {
       
        for (int i = 0; i < numPlates; i++)
        {
            TN_PlateTectonic plate = new TN_PlateTectonic(hexGrid.GetCell(UnityEngine.Random.Range(0, cellCount - 1)), cellsPerRegion);
            plates.Add(plate);


        }
    }

    private void ExpandPlateTectonics()
    {
        bool canExpand = true;
        while (canExpand)
        {
            canExpand = false;
            for (int i = 0; i < plates.Count; i++)
            {
                if (plates[i].Expand())
                {
                    canExpand = true;
                }
            }
        }
    }

    

    private void GenerateCells(int cellCount)
    {
        HexCell cell;
        for (int i = 0; i<cellCount; i++)
        {
            cell = hexGrid.GetCell(i);
            cell.TerrainTypeIndex = 1;
            cell.WaterLevel = 2;
            cell.Elevation = 1;
        }
    }

    
    private void GenerateRegionSeeds()
    {
        foreach(TN_PlateTectonic plate in plates)
        {
            plate.GenerateRegionSeeds();
            
        }
    }
    private void ExpandRegions()
    {
        foreach (TN_PlateTectonic plate in plates)
        {
            plate.ExpandRegions();
            
        }
        
    }

    private void FindRegionNeighbours()
    {
        foreach (TN_PlateTectonic plate in plates)
        {
            plate.FindRegionNeighbours();
        }

    }

   /* private void GetMountains()
    {
        foreach (TN_PlateTectonic plate in plates)
        {
            plate.GetMountains();
        }
    }*/
}
