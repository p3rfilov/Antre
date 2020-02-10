using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class SwarmObject : MonoBehaviour, IClickable
{
    public string Parent { get { return parentName; } }
    public Vector3 InitialPosition { get { return initialPosition; } }
    public Vector2Int Index { get { return index; } }
    public bool Active { get; set; }

    [SerializeField] private string parentName;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector2Int index;

    public void SetParentName (string parentName)
    {
        this.parentName = parentName;
    }

    public void SetInitialPosition (Vector3 pos)
    {
        initialPosition = pos;
    }

    public void SetIndex (Vector2Int index)
    {
        this.index = index;
    }

    public void Click ()
    {
        print("Clicked: " + this.GetType().Name);
    }

    public void Wobble (Vector3 direction, float amplitude, float amplitudeCutoff, float speed, float time, float delay)
    {
        if (!Active)
        {
            StartCoroutine(_Wobble(direction, amplitude, amplitudeCutoff, speed, time, delay));
        }
    }

    private IEnumerator _Wobble (Vector3 direction, float amplitude, float amplitudeCutoff, float speed, float time, float delay)
    {
        Active = true;
        float _Elapsedtime = 0f;
        float _velocity = 0f;

        yield return new WaitForSeconds(delay);
        var _ampl = amplitude;
        while (_ampl > amplitudeCutoff)
        {
            _Elapsedtime += Time.deltaTime;
            transform.position = InitialPosition + direction * _ampl * Mathf.Sin(speed * _Elapsedtime + delay * 20f);
            _ampl = Mathf.SmoothDamp(_ampl, 0f, ref _velocity, time);
            yield return new WaitForFixedUpdate();
        }
        Active = false;
    }
}
