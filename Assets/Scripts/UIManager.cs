using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}

    
