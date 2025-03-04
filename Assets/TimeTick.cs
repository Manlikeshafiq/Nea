using UnityEngine;
using System; 

public class TimeTick : MonoBehaviour
{
    public static event Action OnTick;
    private float tickInterval = 1;
    private float tickCountdown;
    public int tick { get; private set; }


    public static TimeTick Instance;

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
    void Update()
    {
        tickCountdown += Time.deltaTime;

        if (tickCountdown >= tickInterval)
        {
            tick++;
            tickCountdown = 0;

            
            OnTick?.Invoke();
        }
    }
}
