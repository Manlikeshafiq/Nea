using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;
    public List<AnimalSO> animalList = new List<AnimalSO>(); // All available animals
    private Dictionary<AnimalSO, int> animalCounts = new Dictionary<AnimalSO, int>();
    private AnimalSO animalData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Initialize(AnimalSO data)
    {
       
        Debug.Log("w");
    }

}
