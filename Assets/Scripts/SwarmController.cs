using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPlacer))]
public class SwarmController : MonoBehaviour
{
    [Header("Primary Effect")]
    public OnClickBehaviour behaviour;
    public int size;
    public float distanceFalloff;
    public float delay;
    public float speed;
    public float time;
    public float amplitude;
    public float amplitudeCutoff;

    public enum OnClickBehaviour { Ripple, WaveX, WaveY };

    private ObjectPlacer objectPlacer;

    private void Awake ()
    {
        objectPlacer = transform.GetComponent<ObjectPlacer>();
    }

    public void Activate (SwarmObject obj)
    {
        if (objectPlacer != null && obj != null)
        {
            if (behaviour == OnClickBehaviour.Ripple)
            {
                List<SwarmObject>[] neighbours = objectPlacer.GetNeighbours(obj, size);
                for (int i = 0; i < neighbours.Length; i++)
                {
                    foreach (SwarmObject item in neighbours[i])
                    {
                        if (item != null && objectPlacer == item.Parent)
                        {
                            float _amp = amplitude - distanceFalloff * i;
                            item.Wobble(objectPlacer.GetDirection(), _amp, amplitudeCutoff, speed, time, delay * i);
                        }
                    }
                }
            }
            else
            {
                print("Behavior not defined");
            }
        }
    }
}
