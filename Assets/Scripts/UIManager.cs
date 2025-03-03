using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool expanded = false;
    public GameObject panel;

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }

    }
}

    
