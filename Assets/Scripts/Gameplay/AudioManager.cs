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

        soundSources = new Dictionary<string, AudioSource>();

        foreach (Sound sound in sounds) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = sound.loop;
            soundSources.Add(sound.name, source);
        }
    }

    public void SetVolume(string soundType, float volume) {
         string adjustedSoundType = soundType.Replace("Volume", "").Trim();
        foreach (var sound in sounds) {
            if (sound.name.StartsWith(adjustedSoundType, System.StringComparison.OrdinalIgnoreCase)) {
                if (soundSources.TryGetValue(sound.name, out var source)) {
                    source.volume = volume;
                }
            }
        }
    }

    // Método para tocar um som
    public void Play(string soundName) {
        if (soundSources.ContainsKey(soundName)) {
            soundSources[soundName].Play();
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
