using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public class AudioTrigger : MonoBehaviour, IClickable
{
    public AudioSource masterSyncTrack;
    public BarFraction waitForBeat;

    public enum BarFraction { None, Bar = 1, Half=2, Quarter=4, Eighth=8, Sixteenth=16, Thirty_Second=32, Sixty_Fourth=64 };

    private AudioSource source;
    private int clipSamples;
    private int lastSampleCount;
    private bool stopped;
    private float updateFrequency = 0.01f;

    private void Awake ()
    {
        source = transform.GetComponent<AudioSource>();
        clipSamples = source.clip.samples;
        ValidateSampleCount();
    }

    public void Click ()
    {
        if (source != null && !source.isPlaying)
        {
            stopped = false;
            StartCoroutine(TogglePlayback());
            print("Playing");
        }
        else
        {
            stopped = true;
            print("Stopping");
        }
    }

    private IEnumerator TogglePlayback ()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateFrequency);
            if (masterSyncTrack != null)
            {
                source.timeSamples = masterSyncTrack.timeSamples;
            }
            if (waitForBeat == BarFraction.None)
            {
                if (stopped)
                {
                    source.Stop();
                    yield break;
                }
                else if (!source.isPlaying)
                {
                    source.Play();
                }
            }
            else
            {
                int _step = clipSamples / (int)waitForBeat;
                int _sampleMod = source.timeSamples % _step;
                if (_sampleMod < lastSampleCount)
                {
                    if (stopped)
                    {
                        source.Stop();
                        yield break;
                    }
                    else if (!source.isPlaying)
                    {
                        source.Play();
                    }
                }
                lastSampleCount = _sampleMod;
            }
        }
    }

    private void ValidateSampleCount ()
    {
        if (masterSyncTrack != null && source != null)
        {
            if (masterSyncTrack.clip.samples != source.clip.samples)
            {
                Debug.LogWarning("The master track and source audio have different sample count. Audio may play out of sync.");
            }
        }
    }
}
