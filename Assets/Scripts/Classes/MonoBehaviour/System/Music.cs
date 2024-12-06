using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    private AudioSource source;
    private SoundHolder.SoundPack musics;
    private int current = -1;
    [SerializeField] private float percentToFade = 0.8f;
    [SerializeField] private float lerpScale = 5.0f;
    private float timeToFade;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        musics = SoundHolder.Default.GetSoundPack("Music");
        Play();
    }

    public void Play()
    {
        source.Stop();
        int count = musics.sounds.Count;
        int rand = Random.Range(0, count);
        if(current == rand)
        {
            current = rand + 1; ;
            if (current >= count)
                current = 0;
        }
        else
        {
            current = rand;
        }
        var option = musics.sounds[current];
        source.time = 0.0f;
        source.clip = option.clip;
        source.volume = option.volume;
        timeToFade = option.clip.length * percentToFade;
        source.Play();
    }
    private void LateUpdate()
    {
        if (source.time >= timeToFade)
        {
            source.volume = Mathf.Lerp(source.volume, 0.0f, Time.deltaTime * lerpScale);
            if (source.volume <= 0.01f || !source.isPlaying)
            {
                Play();
            }
        }
    }
}
