using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class SwarmObject : MonoBehaviour
{
    public Vector3 InitialPosition { get { return _initialPosition; } }
    public int Index { get { return _index; } }

    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private int _index;

    public void SetInitialPosition (Vector3 pos)
    {
        _initialPosition = pos;
    }

    public void SetIndex (int index)
    {
        _index = index;
    }
}
