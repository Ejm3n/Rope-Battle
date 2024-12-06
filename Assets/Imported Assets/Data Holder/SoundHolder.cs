using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Scriptables/SoundHolder")]
public class SoundHolder : DataHolder
{
    #region Singletone
    private static SoundHolder _default;
    public static SoundHolder Default => _default;
    #endregion

    [Serializable]
    public class SoundOption
    {
        public AudioClip clip;
        public float volume = 1f;
    }
    [Serializable]
    public class SoundPack
    {
        public string key = "";
        public List<SoundOption> sounds;
    }

    public List<SoundPack> soundPacks;


    public override void Init()
    {
        _default = this;
    }

    /// <summary>
    /// Спавнит GameObject c AudioSource, помещает в него рандомный клип из набора с соответствующим названем, задает настройки громкости.
    /// Объект автоматически удаляется после проигрывания клипа.
    /// </summary>
    public SoundPack GetSoundPack(string packName)
    {
        return soundPacks.Find((p) => p.key == packName);
    }
    public AudioSource PlayFromSoundPack(string packName, bool isLoop = false)
    {
        SoundPack pack = soundPacks.Find((p) => p.key == packName);

        if (pack == null || pack.sounds.Count <= 0) return null;
        SoundOption sound = pack.sounds[UnityEngine.Random.Range(0, pack.sounds.Count)];
        if (!sound.clip) return null;

        return SpawnSoundSource(sound.clip, sound.volume, isLoop);
    }
    public AudioSource PlayFromSoundPack(string packName, int id, bool isLoop = false)
    {
        SoundPack pack = soundPacks.Find((p) => p.key == packName);

        if (pack == null || pack.sounds.Count <= 0) return null;
        SoundOption sound = pack.sounds[id];
        if (!sound.clip) return null;

        return SpawnSoundSource(sound.clip, sound.volume, isLoop);
    }
    /// <summary>
    /// Спавнит GameObject c AudioSource, помещает в него рандомный клип.
    /// Объект автоматически удаляется после проигрывания клипа.
    /// </summary>
    public void PlayClip(AudioClip clip)
    {
        SpawnSoundSource(clip);
    }

    /// <summary>
    /// Возвращает рандомный звук с настройками громкости из соответствующего набора.
    /// </summary>
    public SoundOption GetRandomSound(string key)
    {
        SoundPack pack = soundPacks.Find((p) => p.key == key);
        if (pack == null || pack.sounds.Count <= 0) return null;
        return pack.sounds[UnityEngine.Random.Range(0, pack.sounds.Count)];
    }

    private AudioSource SpawnSoundSource(AudioClip clip, float volume = 1f, bool isLoop = false)
    {
        AudioSource source = new GameObject("AudioPlayer").AddComponent<AudioSource>();
        source.transform.SetParent(Camera.main.transform);
        source.transform.localPosition = Vector3.zero;
        source.clip = clip;
        source.pitch += UnityEngine.Random.Range(-0.1f, 0.1f);
        source.volume = volume;
        source.loop = isLoop;
        source.Play();
        if (!isLoop) Destroy(source.gameObject, clip.length);
        return source;
    }
}
