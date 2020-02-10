using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class AudioTrigger : MonoBehaviour, IClickable
{
    public AudioSource masterSyncTrack;
    public BarFraction waitForBeat;

    public enum BarFraction { None, Bar = 1, Half=2, Quarter=4, Eighth=8, Sixteenth=16, Thirty_Second=32, Sixty_Fourth=64 };

    private Collider col;

    private void Awake ()
    {
        col = transform.GetComponent<Collider>();
    }

    public void Click ()
    {
        print("Clicked: " + this.GetType().Name);
    }
}
