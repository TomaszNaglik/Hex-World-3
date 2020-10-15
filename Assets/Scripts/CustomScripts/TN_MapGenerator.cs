using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_MapGenerator : MonoBehaviour
{
    public HexGrid hexGrid;
    internal void GenerateMap(int x, int z, bool wrapping)
    {
        hexGrid.CreateMap(x, z, wrapping);
        int cellCount = x * z;
        HexCell cell;
        for(int i = 0; i < cellCount; i++)
        {
            cell = hexGrid.GetCell(i);
            cell.TerrainTypeIndex = 1;
            cell.WaterLevel = 1;
            cell.Elevation = 0;
        }


    }
}
