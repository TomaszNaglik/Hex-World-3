using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TN_Region 
{
    List<HexCell> cells;
    List<HexCell> frontier;
    private static int regionCounter = 0;
    public Color color;
    private int terrainIndex;
    private int elevationIndex;
    private int ID;
    public int TerrainIndex { get => terrainIndex; set => terrainIndex = value; }
    public int ElevationIndex { get => elevationIndex; set => elevationIndex = value; }

    

    public TN_Region(HexCell cell)
    {
        ID = regionCounter;
        regionCounter++;
        
        TerrainIndex = Random.Range(0, 5);
        ElevationIndex = 2;
        if(Random.value > 0.8f)
        {
            ElevationIndex = 1;
        }
        else
        {
            ElevationIndex = 0;
        }
        color = new Color(Random.value, Random.value, Random.value);
        cells = new List<HexCell>();
        frontier = new List<HexCell>();
        AddCell(cell);
    }
    private bool AddCell (HexCell cell)
    {
        if (cells.Contains(cell))
        {
            Debug.Log("Region: " + ID + " cannot add cell as it already exists in the region");
            return false;
            
        }
        cell.Region = this;
        cells.Add(cell);
        cell.TerrainTypeIndex = TerrainIndex;
        cell.Elevation = ElevationIndex;
        cell.EnableHighlight(Color.green);
        return true;
    }

    public bool Expand()
    {
        PopulateFrontier();
        if(frontier.Count == 0)
        {
            return false;
        }

        bool result = false;
        HexCell cell = frontier[Random.Range(0, frontier.Count - 1)];
        if ( AddCell(cell))
        {
            Debug.Log("Region: " + ID + " added " + cell.coordinates);
            cell.EnableHighlight(this.color);
            result = true;
        }

        return result;
        
       
    }

    private void PopulateFrontier()
    {
       /* Debug.Log("Before adding frontier to Region: " + ID + " Region as these cells: ");
        for (int i = 0; i < cells.Count; i++)
        {
            Debug.Log("Cell "+i+": "+cells[i].coordinates);
        }*/
            frontier.Clear();
        for (int i = 0; i< cells.Count; i++)
        {
            HexCell cell = cells[i];
            HexCell neighbour;
            for(int j=0; j< 6; j++)
            {
                neighbour = cell.GetNeighbor((HexDirection)j);
                if (neighbour != null && neighbour.Region == null)
                {

                    frontier.Add(neighbour);
                    neighbour.SetLabel(neighbour.coordinates.ToString());
                    //Debug.Log("Region: " + ID + " added " +neighbour.coordinates+" to FrontierList");
                }
            }
        }
        /*Debug.Log("After adding frontier to Region: " + ID + " Region as these cells in the frontier: ");
        for (int i = 0; i < frontier.Count; i++)
        {
            Debug.Log("Cell " + i + ": " + frontier[i].coordinates);
        }*/
    }
}
