using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public class AudioTrigger : MonoBehaviour
{
    public AudioSource masterSyncTrack;
    public BarFraction waitForBeat;
    public float fadeTime = 0.5f;

    public enum BarFraction { None, Bar = 1, Half=2, Quarter=4, Eighth=8, Sixteenth=16, Thirty_Second=32, Sixty_Fourth=64 };

    private AudioSource source;
    private int clipSamples;
    private int lastSampleCount;
    private bool stopped;
    private bool isCounterRunning;
    private float updateFrequency = 0.01f;

    private void Awake ()
    {
        source = transform.GetComponent<AudioSource>();
        if (source != null)
        {
            clipSamples = source.clip.samples;
        }
        ValidateSampleCount();
    }

    private void OnMouseDown ()
    {
        if (source != null && !source.isPlaying)
        {
            stopped = false;
            if (!isCounterRunning)
            {
                StartCoroutine(RunSampleCounter());
            }
            print("Playing");
        }
        else
        {
            stopped = true;
            print("Stopping");
        }
    }

    private IEnumerator RunSampleCounter ()
    {
        isCounterRunning = true;
        while (true)
        {
            if (masterSyncTrack != null)
            {
                source.timeSamples = masterSyncTrack.timeSamples;
            }

            int _step = clipSamples / (int)waitForBeat;
            int _sampleMod = source.timeSamples % _step;
            if (waitForBeat == BarFraction.None || _sampleMod < lastSampleCount)
            {
                if (stopped)
                {
                    StartCoroutine(ChangeVolume(0));
                }
                else if (!source.isPlaying)
                {
                    StartCoroutine(ChangeVolume(1f));
                }
            }
            lastSampleCount = _sampleMod;
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    private IEnumerator ChangeVolume (float targetVolume)
    {
        float _startVolume = source.volume;

        if (!source.isPlaying)
        {
            source.Play();
        }

        if (targetVolume > _startVolume)
        {
            print("Fading in");
            while (source.volume < targetVolume)
            {
                source.volume += targetVolume * Time.deltaTime / fadeTime;
                yield return new WaitForSeconds(updateFrequency);
            }
        }
        else
        {
            print("Fading out");
            while (source.volume > targetVolume)
            {
                source.volume -= _startVolume * Time.deltaTime / fadeTime;
                yield return new WaitForSeconds(updateFrequency);
            }
        }
        source.volume = targetVolume;

        if (source.volume == 0)
        {
            source.Stop();
        }
        print("Fading done");
    }

    private void ValidateSampleCount ()
    {
        if (masterSyncTrack != null)
        {
            if (masterSyncTrack.clip.samples != clipSamples)
            {
                Debug.LogWarning("The master track and source audio have different sample count. Audio may play out of sync.");
            }
        }
    }
}
