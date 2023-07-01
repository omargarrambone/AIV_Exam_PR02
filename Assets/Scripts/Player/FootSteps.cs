using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{

    [Header("Audio Clips")]
    public AudioClip[] GrassFootsteps;

    [Header("Audio Sources")]
    public int InitialSourceCount;
    [Range(0, 1)] public float FootstepsVolume;
    [Range(0, 0.5f)] public float RandomVolumeRange;
    [Range(0, 0.3f)] public float RandomPitchRange;

    private List<AudioSource> sources;


    // Start is called before the first frame update
    void Start()
    {
        sources = new List<AudioSource>(InitialSourceCount);

        for (int i = 0; i < InitialSourceCount; i++)
        {
            sources.Add(CreateNewSource());
        }
    }

    AudioSource CreateNewSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 1;
        source.dopplerLevel = 0;

        return source;
    }


    AudioSource GetEmptySource()
    {
        AudioSource emptySource = null;
        bool found = false;

        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying && !found)
            {
                emptySource = source;
                found = true;
            }
        }

        if (!found)
        {
            AudioSource newSource = CreateNewSource();
            sources.Add(newSource);
            emptySource = newSource;
        }

        return emptySource;
    }

    public void PlayFootstep()
    {
        AudioSource source = GetEmptySource();

        if (source != null)
        {
            source.clip = GrassFootsteps[Random.Range(0, GrassFootsteps.Length)];
            source.volume = Random.Range(FootstepsVolume - RandomVolumeRange, FootstepsVolume);
            source.pitch = Random.Range(1 - (RandomPitchRange * 0.33f), 1 + RandomPitchRange);
            source.Play();
        }
    }
}
