using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAllPlants()
    {
        
    }
}
