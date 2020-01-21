using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class SwarmObject : MonoBehaviour
{
    public Vector3 InitialPosition { get { return _initialPosition; } }
    public Vector2Int Index { get { return _index; } }
    public bool Active { get; set; }

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

    public void Wobble (Vector3 direction, float amplitude, float speed, float falloff, float delay)
    {
        if (!Active)
        {
            StartCoroutine(_Wobble(direction, amplitude, speed, falloff, delay));
        }
    }

    private IEnumerator _Wobble (Vector3 direction, float amplitude, float speed, float falloff, float delay)
    {
        Active = true;
        yield return new WaitForSeconds(delay);
        transform.position += direction * amplitude;
        while (transform.position != InitialPosition)
        {
            //print("Wobble");
            transform.position = InitialPosition + direction * amplitude * Mathf.Sin(speed * Time.time);
        }
        Active = false;
    }
}
