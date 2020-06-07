using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	public Sound[] sounds;

	void Awake() {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        InitAudio();
	}

	public void InitAudio() {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play (string name) {
		Sound s = System.Array.Find(sounds, sound => sound.name == name);
		s.source.volume = s.volume * PlayerPrefs.GetFloat(s.isSFX ? "SFXVol" : "MusicVol");
		s.source.Play();
		Debug.Log("AudioManager :: " + s.source.isPlaying + " playing " + s.name);
	}
	public void Pause (string name, bool pause) {
		Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.volume = s.volume * PlayerPrefs.GetFloat(s.isSFX ? "SFXVol" : "MusicVol");
        if (pause) s.source.Pause();
		else s.source.UnPause();
		Debug.Log("AudioManager :: " + s.source.isPlaying + " (un)pausing " + s.name);
	}
	public void Stop (string name) {
		Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.volume = s.volume * PlayerPrefs.GetFloat(s.isSFX ? "SFXVol" : "MusicVol");
        s.source.Stop();
		Debug.Log("AudioManager :: " + s.source.isPlaying + " stopping " + s.name);
	}
	public void Play (string[] names) {
		List<Sound> plays = new List<Sound>();
		foreach (string name in names) {
			plays.Add(System.Array.Find(sounds, sound => sound.name == name));
		}
		StartCoroutine(PlayClips(plays));
	}

    public void SetVolume(bool isSFX) {
        foreach (Sound s in sounds) {
            if(isSFX && s.isSFX) s.source.volume = s.volume * PlayerPrefs.GetFloat("SFXVol");
            else if(!isSFX && !s.isSFX) s.source.volume = s.volume * PlayerPrefs.GetFloat("MusicVol");
        }
    }

	IEnumerator PlayClips(List<Sound> plays) {
		foreach (Sound sound in plays) {
			Play(sound.name);
			yield return new WaitForSeconds(sound.clip.length);
		}
	}
}

[System.Serializable]
public class Sound {
	public string name;
    public AudioClip clip;
    public bool isSFX;
    [HideInInspector] public AudioSource source;
	[Range(0f, 1f)] public float volume;
    [Range(1f, 3f)] public float pitch;
	public bool loop;
}
