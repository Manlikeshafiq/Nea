using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    
    public GameObject plantSelectPanel;
    public GameObject closePlantSelectButton;


    public Text tileTitleText;
    public TMP_Text tilePlantTitleText0;
    public TMP_Text tilePlantTitleText1;
    public TMP_Text tilePlantTitleText2;
    public TMP_Text tilePlantTitleText3;
    public TMP_Text tilePlantTitleText4;

    public TMP_Text tilePlantBodyText0;
    public TMP_Text tilePlantBodyText1;
    public TMP_Text tilePlantBodyText2;
    public TMP_Text tilePlantBodyText3;
    public TMP_Text tilePlantBodyText4;

    public TMP_Text tileAnimalTitleText0;
    public TMP_Text tileAnimalTitleText1;
    public TMP_Text tileAnimalTitleText2;
    public TMP_Text tileAnimalTitleText3;
    public TMP_Text tileAnimalTitleText4;

    public TMP_Text tileAnimalBodyText0;
    public TMP_Text tileAnimalBodyText1;
    public TMP_Text tileAnimalBodyText2;
    public TMP_Text tileAnimalBodyText3;
    public TMP_Text tileAnimalBodyText4;
    public TMP_Text foxesAndRabbits;



    public TMP_Text speed;
    public TMP_Text days;
    public Button addSpeed;
    public Button pause;

    public Button decreaseSpeed;
    public Button newGame;

    public TMP_InputField tileWeight;
    public TMP_InputField tileBiome;

    public TMP_InputField tileSunlight;
    public TMP_InputField tileMaturity;
    public TMP_InputField tileWater;
    public TMP_InputField gridXSize;
    public TMP_InputField gridYSize;
    public TMP_InputField animalsInGame;
    public TMP_InputField inputFieldbiome;


    public TileSO tileso;
    public GameObject tilePanel;
    public TileMono selectedTile;
    bool panelIsOpen = false;
    int hours;
    int intdays;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
       
        plantSelectPanel.SetActive(false);
        closePlantSelectButton.SetActive(false);
        tilePanel.SetActive(false);

        TimeTick.OnTick += RefreshDaysAndHours;
    }

    public void RefreshDaysAndHours()
    {
        intdays = TimeTick.Instance.tick / 24;
        hours = TimeTick.Instance.tick % 24;

        days.text = ($"Days - {intdays} Hours - {hours}");
        foxesAndRabbits.text = ($"Foxes - {GridManager.Instance.foxes} Rabbits - {GridManager.Instance.rabbits}");

    }
    public void AddTileToRefreshInfo()
    {
        if (panelIsOpen)
        {
            RefreshInfo(selectedTile);
        }
       
    }
    
    public void RefreshInfo(TileMono tile)
    {
        TMP_Text[] plantTitleTexts = { tilePlantTitleText0, tilePlantTitleText1, tilePlantTitleText2, tilePlantTitleText3, tilePlantTitleText4 };
        TMP_Text[] plantBodyTexts = { tilePlantBodyText0, tilePlantBodyText1, tilePlantBodyText2, tilePlantBodyText3, tilePlantBodyText4 };


        TMP_Text[] animalTitleTexts = { tileAnimalTitleText0, tileAnimalTitleText1, tileAnimalTitleText2, tileAnimalTitleText3, tileAnimalTitleText4 };
        TMP_Text[] animalBodyTexts = { tileAnimalBodyText0, tileAnimalBodyText1, tileAnimalBodyText2, tileAnimalBodyText3, tileAnimalBodyText4 };

        if (tile.plantSlots.Count > 0)
        {

            
            for (int i = 0; i < tile.plantSlots.Count; i++)
            {
                 

                plantTitleTexts[i].text = tile.plantSlots[i].plantName;
                plantBodyTexts[i].text = $"HP - {tile.plantSlots[i].HP} Hunger - {Math.Round(tile.plantSlots[i].hungerLevel, 2)} Thirst - {Math.Round(tile.plantSlots[i].thirstLevel, 2)}\n" +
                                    $"Maturity - {tile.plantSlots[i].plantMaturityName}\n" +
                                    $"Growth to {tile.plantSlots[i].nextPlantMaturityName} - / {tile.plantSlots[i].totalGrowthTicks}\n";
            }

           
        }
        else if (tile.animalSlots.Count > 0)
        {


            for (int i = 0; i < tile.animalSlots.Count; i++)
            {
                animalTitleTexts[i].text = tile.animalSlots[i].animalName;
                animalBodyTexts[i].text = $"HP - {tile.animalSlots[i].HP} Hunger - {Math.Round(tile.animalSlots[i].hungerLevel, 2)} Thirst - {Math.Round(tile.animalSlots[i].thirstLevel, 2)}\n" +
                                          $"Maturity - {tile.animalSlots[i].animalMaturityName}\n" +
                                          $"Growth to {tile.animalSlots[i].nextAnimalMaturityName} - {tile.animalSlots[i].totalGrowthTicks}\n";
            }




        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                plantTitleTexts[i].text = ("N/a");
                plantBodyTexts[i].text = ("N/A)");
                animalTitleTexts[i].text = ("N/A)");
                animalBodyTexts[i].text = ("N/A)");
            }
        }


    }

   
    public void OpenTilePanel(TileMono tile)
    {
        selectedTile = tile;
        tilePanel.SetActive(true);
        panelIsOpen = true;
        //ALL INPUT FIELDS = DATA

        tileTitleText.text = tile.tileSOData.tileBiome;
        tileWeight.text = tile.tileSOData.weight.ToString();
        tileWater.text = tile.tileSOData.waterLeve.ToString();
        tileSunlight.text = tile.tileSOData.sunlightLevel.ToString();
        tileMaturity.text = tile.tileSOData.fertilityLevel.ToString();
        


        //TO DO: ITERATION
        /*
        if (tile.plantSlots != null)
        {
            tilePlantTitleText0.text = tile.plantSlots[0].plantName.ToString();
            tilePlantTitleText1.text = tile.plantSlots[1].plantName.ToString();
            tilePlantTitleText2.text = tile.plantSlots[2].plantName.ToString();
            tilePlantTitleText3.text = tile.plantSlots[3].plantName.ToString();
            tilePlantTitleText4.text = tile.plantSlots[4].plantName.ToString();
        }
        else
        {
            Debug.Log("no plants opn tile");
        }

        tilePlantBodyText0.text = ($"HP - {tile.plantSlots[0].HP} Hunger - {tile.plantSlots[0].hungerLevel} Thirst - {tile.plantSlots[0].thirstLevel}\r\n" +
            $"Maturity - {tile.plantSlots[0].plantMaturity}\r\n" +
            "Growth to [] - []/ []\r\n");

        tilePlantBodyText1.text = ($"HP - {tile.plantSlots[1].HP} Hunger - {tile.plantSlots[1].hungerLevel} Thirst - {tile.plantSlots[1].thirstLevel}\r\n" +
            $"Maturity - {tile.plantSlots[1].plantMaturity}\r\n" +
            "Growth to [] - []/ []\r\n");
        */


       

        TimeTick.OnTick += AddTileToRefreshInfo;
    }



    public void UpdateTileWeight(string inputValue)
    {
        float newWeight = (float)Convert.ToDouble(inputValue);
        selectedTile.tileSOData.weight = newWeight;

    }

    public void UpdateTileSunlight(string inputValue)
    {
        int newSunlight = Convert.ToInt32(inputValue);
        selectedTile.tileSOData.sunlightLevel = newSunlight;

    }

    public void UpdateTileMaturity(string inputValue)
    {
        int newMaturity = Convert.ToInt32(inputValue);
        selectedTile.tileSOData.fertilityLevel = newMaturity;
     

    }

    public void UpdateTileWater(string inputValue)
    {
        int newWater = Convert.ToInt32(inputValue);
        selectedTile.tileSOData.waterLeve = newWater;


    }

    public int animalsMin;

    public void UpdateAnimals(string inputValue)
    {

        int newAnimals = Convert.ToInt32(inputValue);
        GridManager.Instance.animalsMin = newAnimals;

        Debug.Log(GridManager.Instance.animalsMin );
        Debug.Log(newAnimals + "new");





    }
    public string biome;
    public void UpdateBiome(string inputValue)
    {
        biome = (inputValue);
       


    }



    public int X;

    public void UpdateX(string inputValue)
    {
        
          int newX = Convert.ToInt32(inputValue);
          X = newX;
          

    }

    public int Y;
    public void UpdateY(string inputValue)
    {
        int newY = Convert.ToInt32(inputValue);
        Y = newY;
    }










    public void SpeedTick()
    {
        TimeTick.Instance.tickInterval /= 1.5f;
    }

    public void SlowTick()
    {
        TimeTick.Instance.tickInterval *= 1.5f;
    }

    bool paused = false;
    public void Pause()
    {
        if (paused == false)
        {
            TimeTick.Instance.tickInterval *= 9999999;
            paused = true;

        }
        else
        {
            TimeTick.Instance.tickInterval = 1;

        }
    }





    public void PanelOpenFalse()
    {
        panelIsOpen = false;
        tilePanel.SetActive(false);

        TimeTick.OnTick -= AddTileToRefreshInfo;
    }

 
    public void NewGame()
    {

    }

}

    
