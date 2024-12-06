    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSound : MonoBehaviour
{
    [SerializeField] private string sound;
    [SerializeField] private AudioSource source;
    private LevelMaster levelMaster;
    private float target = 0f;
    private float speed = 1f;
    SoundHolder.SoundOption option;
    private void Start()
    {
        source.Stop();
        source.volume = 0f;
        source.loop = true;
        if (LevelManager.Default.HasCurrent)
        {
            levelMaster = LevelManager.Default.CurrentLevel;
            levelMaster.OnFinish += OnFinish;
        }
        OnStart();
    }
    private void OnDestroy()
    {
        if(levelMaster != null)
        {
            levelMaster.OnStart -= OnStart;
            levelMaster.OnFinish -= OnFinish;
        }
    }
    private void OnStart()
    {
        source.Stop();
        var sp = SoundHolder.Default.GetSoundPack(sound);
        int count = sp.sounds.Count;
        int rand = Random.Range(0, count);
        option = sp.sounds[rand];
        source.time = 0.0f;
        source.clip = option.clip;
        target = option.volume;
        source.Play(); speed = 1f;
    }
    private void OnFinish()
    {
        speed = option.volume;
        target = 0f;
    }
    private void LateUpdate()
    {
        source.volume = Mathf.MoveTowards(source.volume, target, Time.deltaTime * speed);
    }
}
