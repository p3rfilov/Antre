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
    public float time;
    public float amplitude;
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
                    print(obj.Index);
                    print(obj.InitialPosition);
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

    private List<SwarmObject>[] GetNeighbours (SwarmObject obj)
    {
        var array = new List<SwarmObject>[size];
        int grid_x = objectPlacer.xCount;

        if (behaviour == OnClickBehaviour.Ripple)
        {
            foreach (var list_item in array)
            {
                //var n1 =
                //var n2 =
                //var n3 =
                //var n4 =
            }
        }
        return array;
    }

    private bool _IsIndexInRange (Vector2Int index)
    {
        if (Enumerable.Range(0, objectPlacer.xCount).ToList().Contains(index.x) && Enumerable.Range(0, objectPlacer.yCount).ToList().Contains(index.y))
        {
            return true;
        }
        return false;
    }

    private bool _IsObjectCollected (SwarmObject obj, List<SwarmObject>[] array)
    {
        foreach (var list_item in array)
        {
            if (list_item.Contains(obj))
            {
                return true;
            }
        }
        return false;
    }
}
