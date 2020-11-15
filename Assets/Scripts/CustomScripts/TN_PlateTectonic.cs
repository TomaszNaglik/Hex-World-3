using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_PlateTectonic
{
    private static int plateCounter = 0;
    public int numRegions;
    public int cellsPerRegion;
    public int ID;
    private List<TN_Region> regions;
    List<HexCell> cells;
    List<HexCell> frontier;

    private int terrainIndex;
    private int elevationIndex;
    private HexDirection direction;


    public int TerrainIndex { get => terrainIndex; set => terrainIndex = value; }
    public int ElevationIndex { get => elevationIndex; set => elevationIndex = value; }
    public HexDirection Direction { get => direction; set => direction = value; }
    public List<TN_Region> Regions { get => regions; set => regions = value; }

    public TN_PlateTectonic(HexCell cell, int _cellsPerRegion)
    {
        ID = plateCounter;
        plateCounter++;
        cellsPerRegion = _cellsPerRegion;
        cells = new List<HexCell>();
        regions = new List<TN_Region>();
        frontier = new List<HexCell>();

        TerrainIndex = UnityEngine.Random.Range(0, 5);
        Direction = (HexDirection)UnityEngine.Random.Range(0, 5);
        ElevationIndex = 1;
        AddCell(cell);
       
        
    }

    private bool AddCell(HexCell cell)
    {
        if (cells.Contains(cell))
        {
            Debug.Log("Plate: " + ID + " cannot add cell as it already exists in the region");
            return false;

        }
        cell.TerrainTypeIndex = TerrainIndex;
        cell.Elevation = ElevationIndex;
        cell.Plate = this;
        //cell.SetLabel(ID.ToString());
        cells.Add(cell);
        return true;
    }

    public bool Expand()
    {
        PopulateFrontier();
        if (frontier.Count == 0)
        {
            return false;
        }


        return AddCell(frontier[UnityEngine.Random.Range(0, frontier.Count)]);


    }

    private void PopulateFrontier()
    {
        
        frontier.Clear();
        for (int i = 0; i < cells.Count; i++)
        {
            HexCell cell = cells[i];
            HexCell neighbour;
            for (int j = 0; j < 6; j++)
            {
                neighbour = cell.GetNeighbor((HexDirection)j);
                if (neighbour != null && neighbour.Plate == null)
                {

                    frontier.Add(neighbour);
                   
                    
                }
            }
        }
        
    }

    internal void FindRegionNeighbours()
    {
        foreach(TN_Region region in regions)
        {
            region.FindNeighbours();
        }
    }

    /*internal void GetMountains()
    {
        foreach (TN_Region region in regions)
        {
            region.GetMountains();
        }
    }*/

    public void GenerateRegionSeeds(float mountainProbability, float hillProbability)
    {
        regions = new List<TN_Region>();
        numRegions = cells.Count / cellsPerRegion;
        for (int i = 0; i < numRegions; i++)
        {
            TN_Region region = new TN_Region(cells[UnityEngine.Random.Range(0, cells.Count)], mountainProbability, hillProbability);
            regions.Add(region);


        }
    }
    public void ExpandRegions()
    {
        bool canExpand = true;
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
