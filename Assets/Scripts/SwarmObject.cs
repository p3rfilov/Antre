using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class SwarmObject : MonoBehaviour
{
    public Vector3 InitialPosition { get { return _initialPosition; } }
    public Vector2Int Index { get { return _index; } }

    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private Vector2Int _index;

    public void SetInitialPosition (Vector3 pos)
    {
        _initialPosition = pos;
    }

    public void SetIndex (Vector2Int index)
    {
        _index = index;
    }
}
