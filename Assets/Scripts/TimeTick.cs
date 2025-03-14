using UnityEngine;
using System; 

public class TimeTick : MonoBehaviour
{
    public static event Action OnTick;
    public float tickInterval = 1;
    private float timeCount;
    public int tick;

    public static TimeTick Instance;
    public bool dayOrNight = true;

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

    private void Start()
    {
        
    }
    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount >= tickInterval)
        {
            tick++;
            timeCount = 0;
            DayOrNight();
            OnTick?.Invoke();
        }
    }

    public bool DayOrNight()
    {
        if (tick % 24 < 12)
        {
            return dayOrNight = true;
        }
        else
        {
            return dayOrNight = false;
        }
    }
}
