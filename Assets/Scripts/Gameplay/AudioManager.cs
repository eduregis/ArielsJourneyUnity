using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
        public bool loop = false;
    }

    public Sound[] sounds;

    private Dictionary<string, AudioSource> soundSources;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        InitializeVolumes();
    }

    private void InitializeVolumes() {
        soundSources = new Dictionary<string, AudioSource>();

        soundSources = new Dictionary<string, AudioSource>();

        // Obter volumes salvos
        float ambienceVolume = PlayerPrefs.GetFloat(GameDataManager.AmbienceVolumeKey, 1.0f);
        float musicVolume = PlayerPrefs.GetFloat(GameDataManager.MusicVolumeKey, 1.0f);
        float effectVolume = PlayerPrefs.GetFloat(GameDataManager.EffectVolumeKey, 1.0f);

        foreach (Sound sound in sounds) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume; // Valor padrão no som
            source.loop = sound.loop;

            // Ajustar volume com base nos valores salvos
            if (sound.name.StartsWith("Ambience_", System.StringComparison.OrdinalIgnoreCase)) {
                source.volume = ambienceVolume;
            } else if (sound.name.StartsWith("Music_", System.StringComparison.OrdinalIgnoreCase)) {
                source.volume = musicVolume;
            } else if (sound.name.StartsWith("Effect_", System.StringComparison.OrdinalIgnoreCase)) {
                source.volume = effectVolume;
            }

            soundSources.Add(sound.name, source);
        }
    }

    public void SetVolume(string soundType, float volume) {
        Debug.Log($"Setting volume for {soundType} to {volume}");
        string adjustedSoundType = soundType.Replace("Volume", "").Trim();

        foreach (var sound in sounds) {
            if (sound.name.StartsWith($"{adjustedSoundType}_", System.StringComparison.OrdinalIgnoreCase)) {
                if (soundSources.TryGetValue(sound.name, out var source)) {
                    source.volume = volume;
                }
            }
        }
    }

    // Método para tocar um som
    public void Play(string soundName) {
    if (soundSources.ContainsKey(soundName)) {
        if (!soundSources[soundName].isPlaying) {
            soundSources[soundName].Play();
        }
    } else {
        Debug.LogWarning($"Sound {soundName} not found!");
    }
}

    // Método para tocar um som de forma única (sem interferir nos sons em execução)
    public void PlayOneShot(string soundName) {
        if (soundSources.ContainsKey(soundName)) {
            soundSources[soundName].PlayOneShot(soundSources[soundName].clip);
        } else {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }

    public void PlayRandomTypingSound() {
        string[] typingSounds = { "Effect_Type_0", "Effect_Type_1", "Effect_Type_2", "Effect_Type_3" };
        int randomIndex = Random.Range(0, typingSounds.Length);
        PlayOneShot(typingSounds[randomIndex]);
    }

    // Obter o AudioSource se você precisar ajustar o pitch ou volume diretamente
    public AudioSource GetAudioSource(string soundName) {
        if (soundSources.ContainsKey(soundName)) {
            return soundSources[soundName];
        } else {
            Debug.LogWarning($"Sound {soundName} not found!");
            return null;
        }
    }
}
