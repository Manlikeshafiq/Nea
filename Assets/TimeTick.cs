using UnityEngine;
using System; 

public class TimeTick : MonoBehaviour
{
    public static event Action OnTick;
    private float tickInterval = 1;
    private float tickCountdown;
    public int tick { get; private set; }

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
