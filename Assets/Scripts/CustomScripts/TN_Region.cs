using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TN_Region 
{
    List<HexCell> cells;
    List<HexCell> frontier;
    public List<TN_Region> neighbours;

    private TN_PlateTectonic plate;
    private static int regionCounter = 0;
    public Color color;
    private int terrainIndex;
    private int elevationIndex;
    private int ID;
    private float mountainProbability;
    private float hillProbability;
    

    private TN_Landmass landmass;
    public int TerrainIndex { get => terrainIndex; set => terrainIndex = value; }

    public int GetElevationIndex()
    {
        return elevationIndex;
    }

    public void SetElevationIndex(int value)
    {
        elevationIndex = value;
        foreach (HexCell cell in cells)
        {
            cell.Elevation = elevationIndex;
        }
    }
    public TN_PlateTectonic Plate { get => plate; set => plate = value; }
    public TN_Landmass Landmass { get => landmass;  set => landmass = value; }
    public bool BordersLandmassOtherThan(TN_Landmass landmass)
    {
        
        foreach(TN_Region neighbour in neighbours)
        {
            if(neighbour!= null && neighbour.Landmass != null && neighbour.Landmass != landmass)
            {
                return true;
            }
        }
        return false;
        
    }
    public bool BordersLandmass()
    {

        foreach (TN_Region neighbour in neighbours)
        {
            if (neighbour != null && neighbour.Landmass != null)
            {
                return true;
            }
        }
        return false;

    }

    public bool IsPolar { get => isPolar; set => isPolar = value; }

    private bool isPolar;
    public TN_Region(HexCell cell, float _mountainProbability, float _hillProbability)
    {
        ID = regionCounter;
        regionCounter++;

        mountainProbability = _mountainProbability;
        hillProbability = _hillProbability;

        color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        cells = new List<HexCell>();
        frontier = new List<HexCell>();
        neighbours = new List<TN_Region>();

        TerrainIndex = 1;// UnityEngine.Random.Range(0, 5);
        SetElevationIndex(2);
        if(UnityEngine.Random.value > 1.0f)
        {
            SetElevationIndex(1);
        }
        else
        {
            SetElevationIndex(0);
        }
       
        plate = cell.Plate;
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
        cell.Elevation = GetElevationIndex();
        //cell.EnableHighlight(this.color);
        if (cell.IsPolar)
        {
            this.isPolar = true;
        }
        return true;
    }

    public bool Expand()
    {
        PopulateFrontier();
        if(frontier.Count == 0)
        {
            return false;
        }

        return AddCell(frontier[UnityEngine.Random.Range(0, frontier.Count)]);
        
       
    }

    

    private void PopulateFrontier()
    {
        frontier.Clear();
        for (int i = 0; i< cells.Count; i++)
        {
            HexCell cell = cells[i];
            HexCell neighbour;
            for(int j=0; j< 6; j++)
            {
                neighbour = cell.GetNeighbor((HexDirection)j);
                
                if (neighbour != null && neighbour.Plate.ID == this.Plate.ID && neighbour.Region == null)
                {
                   frontier.Add(neighbour);
                  
                }
            }
        }
        
    }

    internal void FindNeighbours()
    {
        HexCell cell;
        HexCell neighbour;
        for (int i = 0; i < cells.Count; i++)
        {
            cell = cells[i];
            for (int j = 0; j < 6; j++)
            {
                neighbour = cell.GetNeighbor((HexDirection)j);

                if (neighbour != null && neighbour.Region != cell.Region)
                {
                    cell.IsRegionBorder = true;
                    cell.neighbourRegion = neighbour.Region;
                    if (!neighbours.Contains(neighbour.Region))
                    {
                        
                            neighbours.Add(neighbour.Region);

                        
                    }

                    if (neighbour.Plate != cell.Plate)
                    {
                        cell.IsPlateBorder = true;
                        cell.neighbourPlate = neighbour.Plate;
                    }

                }
                    
                
            }
        }
    }

    internal void GetMountains()
    {
       
        foreach(HexCell cell in cells)
        {
            if (cell.IsPlateBorder && cell.Plate.Direction == HexDirectionExtensions.Opposite(cell.neighbourPlate.Direction))
            {
                if (UnityEngine.Random.value > (1-mountainProbability))
                {
                    cell.Elevation += 2;
                    cell.TerrainTypeIndex = 5;
                }
               
            }
            else if (cell.IsPlateBorder && cell.Plate.Direction == HexDirectionExtensions.Next(HexDirectionExtensions.Opposite(cell.neighbourPlate.Direction))
                || cell.IsPlateBorder && cell.Plate.Direction == HexDirectionExtensions.Previous(HexDirectionExtensions.Opposite(cell.neighbourPlate.Direction)))
            {
                if (UnityEngine.Random.value > (1- hillProbability))
                {
                    cell.Elevation += 1;
                    cell.TerrainTypeIndex = 3;
                }
            }
            
        }

        foreach (HexCell cell in cells)
        {
            if (cell.HasMountainNeighbour())
            {
                if (UnityEngine.Random.value > (1 - hillProbability))
                {
                    cell.Elevation += 1;
                    cell.TerrainTypeIndex = 3;
                }
            }
        }


    }

   

}

