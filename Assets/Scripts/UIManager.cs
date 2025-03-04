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

    public TMP_InputField tileWeight;
    public TMP_InputField tileSunlight;
    public TMP_InputField tileMaturity;
    public TMP_InputField tileWater;

    public GameObject tilePanel;
    public TileMono selectedTile;

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
    }

    
    public void OpenTilePanel(TileMono tile)
    {
        selectedTile = tile;
        tilePanel.SetActive(true);
        
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


        TMP_Text[] plantTitleTexts = { tilePlantTitleText0, tilePlantTitleText1, tilePlantTitleText2, tilePlantTitleText3, tilePlantTitleText4 };
        TMP_Text[] plantBodyTexts = { tilePlantBodyText0, tilePlantBodyText1, tilePlantBodyText2, tilePlantBodyText3, tilePlantBodyText4 };

        if (tile.plantSlots != null && tile.plantSlots.Count > 0)
        {
            

            for (int i = 0; i < tile.plantSlots.Count; i++)
            {

                Debug.Log("Aaaa");
                plantTitleTexts[i].text = tile.plantSlots[i].plantName;
                plantBodyTexts[i].text = $"HP - {tile.plantSlots[i].HP} Hunger - {tile.plantSlots[i].hungerLevel} Thirst - {tile.plantSlots[i].thirstLevel}\n" +
                                    $"Maturity - {tile.plantSlots[i].plantMaturity}\n" +
                                    "Growth to [] - []/ []\n";
            }
        }
        else
        {
           for (int i = 0;i < 5; i++)
            {
                Debug.Log("No plants");
                plantTitleTexts[i].text =("N/a");
                plantBodyTexts[i].text = ("N/A)");
            }
        }
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


}

    
