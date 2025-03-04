using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    
    public GameObject plantSelectPanel;
    public GameObject closePlantSelectButton;


    public Text tileTitleText;
    public TMP_InputField tileWeight;
    public TMP_InputField tileSunlight;
    public TMP_InputField tileMaturity;
    public TMP_InputField tileWater;

    public GameObject tilePanel;

    public TileManager tileManager;


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
        if (plantSelectPanel != null)
        {
            plantSelectPanel.SetActive(false);
        }
        closePlantSelectButton.SetActive(false);



        tilePanel.SetActive(false);
    }

    public void OpenTilePanel()
    {
        tilePanel.SetActive(true);
    }



    private float UpdateWeightTileUI()
    {
        tileManager.tileSOData.weight = (float)Convert.ToDouble(UIManager.Instance.tileWeight.text);
        return tileManager.tileSOData.weight;

    }
}

    
