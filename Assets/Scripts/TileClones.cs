using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TileClones : TileManager
{
    public TileSO tileSOData ;  

    private SpriteRenderer spriteRenderer;


    private List<PlantSO> plantSlots = new List<PlantSO>();
    public int maxPlantSlots = 5;
    public int maxAnimalSlots = 3;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TileSO data)
    {
        //ALL DATA COMES FROM CLONE
        tileSOData = CloneSOData(data) ;
        name = $"Tile ({tileSOData.tileBiome})";


        //ADD X,Y COORDS HERE?

        spriteRenderer.color = tileSOData.tileColor;


    }

  


    private void OnMouseDown()  //Temporary Solution (?)
    {
        if (PlantManager.Instance != null && PlantManager.Instance.plantingMode)
        {
            PlantManager.Instance.TryPlantOnTile(this);
        }
        else if (PlantManager.Instance.plantingMode != true)
        {
            TileUI();
        }
    }

    private void TileUI()
    {
        UIManager.Instance.OpenTilePanel();
        UIManager.Instance.tileTitleText.text = tileSOData.tileBiome ;
        UIManager.Instance.tileWeight.text = tileSOData.weight.ToString() ;
        UIManager.Instance.tileWater.text = tileSOData.waterLevel.ToString() ;
        UIManager.Instance.tileSunlight.text = tileSOData.sunlightLevel.ToString() ;
        UIManager.Instance.tileMaturity.text = tileSOData.fertilityLevel.ToString() ;

    }

   



  
}

        
