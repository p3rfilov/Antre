﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    [Header("Randomize Transforms (+/-)")]
    public Vector3 randomPositionOffset;
    public Vector3 randomRotationOffset;
    public Vector3 randomScaleOffset;

    [Header("Place on Surface")]
    public Collider surfaceCollider;
    public Direction rayCastDirection;
    public float surfaceOffset;
    public bool discardOnMiss;

    public enum GridCellType { Triangle, Square };
    public enum Direction { Down, Up, Left, Right, Forward, Back };

    [HideInInspector] public SwarmObject[,] swarmObjectArray;


    private void Awake ()
    {
        PlaceObjects();
    }

    public void Clear ()
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; --i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }

        swarmObjectArray = new SwarmObject[xCount, yCount];
    }

    public void PlaceObjects ()
    {
        Clear();
        if (objects != null && objects.Length > 0)
        {
            Vector3 _pos;
            Vector3 _rot;
            Vector3 _scale;

            Vector3 _parentPosition = transform.position;
            float _halfSpacing = spacing / 2;
            float _triHeight = Mathf.Sqrt(spacing * spacing - _halfSpacing * _halfSpacing);

            for (int y = 0; y < yCount; y++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    _pos = _parentPosition;
                    if (gridCellType == GridCellType.Triangle)
                    {
                        float offset = 0;
                        if (y % 2 != 0) // shift every odd row halfway forward
                        {
                            offset = _halfSpacing;
                        }
                        _pos += new Vector3(x * spacing + offset, 0f, y * _triHeight);
                    }
                    else
                    {
                        _pos += new Vector3(x * spacing, 0f, y * spacing);
                    }

                    _pos = GetRayHitPositionOnSurface(_pos);
                    if (discardOnMiss && _pos == Vector3.zero)
                    {
                        continue;
                    }
                    _pos += GetRandomValue(randomPositionOffset);
                    _rot = initialRotation + GetRandomValue(randomRotationOffset);
                    _scale = initialScale + GetRandomValue(randomScaleOffset);

                    SwarmObject _obj = objects[Random.Range(0, objects.Length)];
                    if (_obj != null)
                    {
                        GameObject obj = Instantiate(_obj.gameObject, _pos, Quaternion.Euler(_rot), transform);
                        obj.transform.localScale = _scale;

                        var swarwObj = obj.GetComponent<SwarmObject>();
                        swarwObj.SetParent(this);
                        swarwObj.SetInitialPosition(_pos);
                        swarwObj.SetIndex(new Vector2Int(x, y));
                        swarmObjectArray[x, y] = swarwObj;
                    }
                }
            }
        }
    }

    public List<SwarmObject>[] GetNeighbours (SwarmObject obj, int count)
    {
        var _array = new List<SwarmObject>[count];

        for (int i = 0; i < count; i++)
        {
            _array[i] = new List<SwarmObject>();
            if (i == 0)
            {
                _array[i].Add(obj);
            }
            else
            {
                foreach (SwarmObject item in _array[i - 1])
                {
                    if (item != null)
                    {
                        Vector2Int[] nIndexes;
                        var i1 = new Vector2Int(item.Index.x + 1, item.Index.y);
                        var i2 = new Vector2Int(item.Index.x - 1, item.Index.y);
                        var i3 = new Vector2Int(item.Index.x, item.Index.y + 1);
                        var i4 = new Vector2Int(item.Index.x, item.Index.y - 1);

                        if (gridCellType == GridCellType.Square)
                        {
                            nIndexes = new Vector2Int[] { i1, i2, i3, i4 };
                        }
                        else // due to the grid being shifted halfway forward in y, we get 2 additional (odd/even row-dependent) neighbors at y + 1 and y + 1
                        {
                            var i5 = new Vector2Int(item.Index.x + (item.Index.y % 2 != 0 ? 1 : -1), item.Index.y + 1);
                            var i6 = new Vector2Int(item.Index.x + (item.Index.y % 2 != 0 ? 1 : -1), item.Index.y - 1);
                            nIndexes = new Vector2Int[] { i1, i2, i3, i4, i5, i6 };
                        }

                        foreach (var index in nIndexes)
                        {
                            if (_IsIndexInRange(index))
                            {
                                SwarmObject neighbour = swarmObjectArray[index.x, index.y];
                                if (!_IsObjectCollected(neighbour, _array, i))
                                {
                                    _array[i].Add(neighbour);
                                }
                            }
                        }
                    }
                }
            }
        }
        return _array;
    }

    public Vector3 GetDirection ()
    {
        if (rayCastDirection == Direction.Up)
            return Vector3.up;
        if (rayCastDirection == Direction.Left)
            return Vector3.left;
        if (rayCastDirection == Direction.Right)
            return Vector3.right;
        if (rayCastDirection == Direction.Forward)
            return Vector3.forward;
        if (rayCastDirection == Direction.Back)
            return Vector3.back;
        return Vector3.down;
    }

    private Vector3 GetRandomValue (Vector3 range)
    {
        var result = new Vector3(Random.Range(range.x, -range.x), Random.Range(range.y, -range.y), Random.Range(range.z, -range.z));
        return result;
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
            else if (discardOnMiss)
            {
                return Vector3.zero;
            }
        }
        return origin;
    }

    private bool _IsIndexInRange (Vector2Int index)
    {
        if (Enumerable.Range(0, xCount).ToList().Contains(index.x) && Enumerable.Range(0, yCount).ToList().Contains(index.y))
        {
            return true;
        }
        return false;
    }

    private bool _IsObjectCollected (SwarmObject obj, List<SwarmObject>[] array, int index, int searchDepth = 1)
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
