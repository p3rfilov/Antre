using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private void Update ()
    {
        if (objectPlacer != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                SwarmObject obj = GetObjectUnderMouse();
                if (obj != null && !obj.Active)
                {
                    List<SwarmObject>[] neighbours = objectPlacer.GetNeighbours(obj, size);
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        foreach (SwarmObject item in neighbours[i])
                        {
                            float _amp = amplitude - distanceFalloff * i;
                            item.Wobble(objectPlacer.GetDirection(), _amp, amplitudeCutoff, speed, time, delay * i);
                        }
                    }
                }
            }
        }
    }

    private SwarmObject GetObjectUnderMouse ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            var obj = hit.transform.GetComponent<SwarmObject>();
            if (obj != null)
            {
                return obj;
            }
        }
        return null;
    }
}
