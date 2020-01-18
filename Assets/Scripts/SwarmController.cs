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
                    List<SwarmObject>[] neighbours = GetNeighbours(obj);
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

    private List<SwarmObject>[] GetNeighbours (SwarmObject obj)
    {
        var array = new List<SwarmObject>[size];
        int grid_x = objectPlacer.xCount;

        if (behaviour == OnClickBehaviour.Ripple)
        {
            for (int i = 0; i < size; i++)
            {
                array[i] = new List<SwarmObject>();
                if (i == 0)
                {
                    array[i].Add(obj);
                }
                else
                {
                    foreach (SwarmObject item in array[i-1])
                    {
                        var i1 = new Vector2Int(item.Index.x + 1, item.Index.y);
                        var i2 = new Vector2Int(item.Index.x - 1, item.Index.y);
                        var i3 = new Vector2Int(item.Index.x, item.Index.y + 1);
                        var i4 = new Vector2Int(item.Index.x, item.Index.y - 1);
                        foreach (var index in new Vector2Int[] { i1, i2, i3, i4 })
                        {
                            if (_IsIndexInRange(index))
                            {
                                SwarmObject neighbour = objectPlacer.swarmObjectArray[index.x, index.y];
                                if (!_IsObjectCollected(neighbour, array, i))
                                {
                                    array[i].Add(neighbour);
                                }
                            }
                        }
                    }
                }
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

    private bool _IsObjectCollected (SwarmObject obj, List<SwarmObject>[] array, int index, int searchDepth=1)
    {
        for (int i = index + 1, j = 0; i-- > 0; j++)
        {
            if (array[i].Contains(obj))
            {
                return true;
            }
            if (j > searchDepth)
            {
                break;
            }
        }
        return false;
    }
}
