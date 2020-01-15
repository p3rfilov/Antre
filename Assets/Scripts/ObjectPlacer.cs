using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectPlacer : MonoBehaviour
{
    [Header("Object Settings")]
    public SwarmObject[] objects;
    public Vector3 initialRotation;
    public Vector3 initialScale = Vector3.one;

    [Header("Grid Settings")]
    public int xCount = 1;
    public int yCount = 1;
    public float spacing = 1f;
    public GridCellType gridCellType;

    [Header("Randomize Trasforms (+/-)")]
    public Vector3 randomPositionOffset;
    public Vector3 randomRotationOffset;
    public Vector3 randomScaleOffset;

    [Header("Place on Surface")]
    public Collider surfaceCollider;
    public Direction RayCastDirection;
    public float surfaceOffset;

    public enum GridCellType { Triangle, Square };
    public enum Direction { Down, Up, Left, Right, Forward, Back };

    [HideInInspector] public SwarmObject[] swarmObjectArray;

    private void Start ()
    {
        PlaceObjects();
    }

    private void Init ()
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; --i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }

        swarmObjectArray = new SwarmObject[xCount * yCount];
    }

    private Vector3 GetRayHitPositionOnSurface (Vector3 origin)
    {
        if (surfaceCollider != null)
        {
            Vector3 dir = GetDirection();
            var ray = new Ray(origin, dir);
            if (surfaceCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return hit.point + dir * -surfaceOffset;
            }
        }
        return origin;
    }

    private Vector3 GetDirection ()
    {
        if (RayCastDirection == Direction.Up)
            return Vector3.up;
        if (RayCastDirection == Direction.Left)
            return Vector3.left;
        if (RayCastDirection == Direction.Right)
            return Vector3.right;
        if (RayCastDirection == Direction.Forward)
            return Vector3.forward;
        if (RayCastDirection == Direction.Back)
            return Vector3.back;
        return Vector3.down;
    }

    public void PlaceObjects ()
    {
        Init();
        if (objects != null && objects.Length > 0)
        {
            Vector3 pos;
            Vector3 rot;
            Vector3 scale;

            Vector3 parentPosition = transform.position;
            float half_spacing = spacing / 2;
            float tri_height = Mathf.Sqrt(spacing * spacing - half_spacing * half_spacing);

            for (int i = 0, y = 0; y < yCount; y++)
            {
                for (int x = 0; x < xCount; x++, i++)
                {
                    pos = parentPosition;
                    if (gridCellType == GridCellType.Triangle)
                    {
                        float offset = 0;
                        if (y % 2 != 0)
                        {
                            offset = half_spacing;
                        }
                        pos += new Vector3(x * spacing + offset, 0f, y * tri_height);
                    }
                    else
                    {
                        pos += new Vector3(x * spacing, 0f, y * spacing);
                    }

                    pos = GetRayHitPositionOnSurface(pos);
                    pos += GetRandomValue(randomPositionOffset);
                    rot = initialRotation + GetRandomValue(randomRotationOffset);
                    scale = initialScale + GetRandomValue(randomScaleOffset);

                    SwarmObject _obj = objects[Random.Range(0, objects.Length)];
                    if (_obj != null)
                    {
                        GameObject obj = Instantiate(_obj.gameObject, pos, Quaternion.Euler(rot), transform);
                        obj.transform.localScale = scale;

                        var swarwObj = obj.GetComponent<SwarmObject>();
                        swarwObj.SetInitialPosition(pos);
                        swarwObj.SetIndex(i);
                        swarmObjectArray[i] = swarwObj;
                    }
                }
            }
        }
    }

    Vector3 GetRandomValue (Vector3 range)
    {
        var result = new Vector3(Random.Range(range.x, -range.x), Random.Range(range.y, -range.y), Random.Range(range.z, -range.z));
        return result;
    }
}
