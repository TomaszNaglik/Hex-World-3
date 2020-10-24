using System.Collections.Generic;
using UnityEngine;

public class TN_MapGenerator : MonoBehaviour
{
    public int numRegions;
    public List<TN_Region> regions;
    
    public HexGrid hexGrid;
    internal void GenerateMap(int x, int z, bool wrapping)
    {
        hexGrid.CreateMap(x, z, wrapping);
        GenerateLand();


    }

    void GenerateLand()
    {
        int cellCount = hexGrid.cellCountX * hexGrid.cellCountZ;
        bool canExpand = true;
        HexCell cell;
        for (int i = 0; i < cellCount; i++)
        {
            cell = hexGrid.GetCell(i);
            cell.TerrainTypeIndex = 1;
            cell.WaterLevel = 1;
            cell.Elevation = 1;
        }

        regions = new List<TN_Region>();
        for (int i = 0; i< numRegions; i++)
        {
            TN_Region region = new TN_Region(hexGrid.GetCell(Random.Range(0, cellCount - 1)));
            regions.Add(region);
            
            
        }

        while (canExpand)
        {
            canExpand = false;
            for (int i = 0; i < regions.Count; i++)
            {
                if (regions[i].Expand())
                {
                    canExpand = true;
                }
            }
        }
        


    }
}
