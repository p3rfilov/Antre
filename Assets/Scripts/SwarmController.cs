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
    public float time = 1f;
    public float amplitude = 1f;
    public float falloff;

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
                if (obj != null)
                {
                    List<SwarmObject>[] neighbours = objectPlacer.GetNeighbours(obj, size);
                    foreach (var list_item in neighbours)
                    {
                        foreach (SwarmObject item in list_item)
                        {
                            item.transform.Translate(Vector3.up * amplitude * Time.deltaTime);
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
