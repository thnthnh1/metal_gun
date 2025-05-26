using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	private static SoundManager _Instance_k__BackingField;

	public AudioSource audioMusic;

	public AudioSource audioSfx;

	public AudioClip sfxButtonClick;

	public AudioClip[] musicClips;

	public AudioClip[] sfxClips;

	public AudioClip multiplayerSound;

	public Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

	public Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();

	[Header("AUDIO MIXER")]
	public AudioMixer audioMixer;

	private string musicParameterName = "musicVolume";

	private string sfxParameterName = "sfxVolume";

	public static SoundManager Instance
	{
		get;
		private set;
	}

	private void Start()
	{
		if (SoundManager.Instance == null)
		{
			SoundManager.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.Init();
	}

	private void Init()
	{
		for (int i = 0; i < this.musicClips.Length; i++)
		{
			if (this.musicClips[i] != null)
			{
				this.musicDictionary.Add(this.musicClips[i].name, this.musicClips[i]);
			}
		}
		for (int j = 0; j < this.sfxClips.Length; j++)
		{
			if (this.sfxClips[j] != null)
			{
				this.sfxDictionary.Add(this.sfxClips[j].name, this.sfxClips[j]);
			}
		}
		this.AdjustMusicVolume(ProfileManager.UserProfile.musicVolume);
		this.AdjustSoundVolume(ProfileManager.UserProfile.soundVolume);
	}

	public void PlaySfxClick()
	{
		this.audioSfx.PlayOneShot(this.sfxButtonClick, this.audioSfx.volume);
	}

	public void PlaySfx(string sfxName, float decibel = 0f)
	{
		if (this.sfxDictionary.ContainsKey(sfxName))
		{
			AudioClip clip = this.sfxDictionary[sfxName];
			this.PlaySfx(clip, decibel);
		}
	}

	public void PlaySfx(AudioClip clip, float decibel = 0f)
	{
		if (clip)
		{
			this.audioMixer.SetFloat(this.sfxParameterName, decibel);
			this.audioSfx.PlayOneShot(clip, this.audioSfx.volume);
		}
	}

	public void StopSfx()
	{
		this.audioSfx.Stop();
	}

	public void StopMusic()
	{
		if (this.audioMusic.isPlaying)
		{
			this.audioMusic.Stop();
			this.audioMusic.clip = null;
		}
	}

	public void PlayMusic(string musicName, float decibel = 0f)
	{
		if (this.musicDictionary.ContainsKey(musicName))
		{
			if (this.IsPlayingMusic(musicName))
			{
				return;
			}
			this.audioMixer.SetFloat(this.musicParameterName, decibel);
			this.audioMusic.clip = this.musicDictionary[musicName];
			this.audioMusic.loop = true;
			this.audioMusic.Play();
		}
	}

	public void PlayMusicFromBeginning(string musicName, float decibel = 0f)
	{
		if (this.musicDictionary.ContainsKey(musicName))
		{
			this.audioMixer.SetFloat(this.musicParameterName, decibel);
			this.audioMusic.clip = this.musicDictionary[musicName];
			this.audioMusic.loop = true;
			this.audioMusic.Play();
		}
	}

	public bool IsPlayingMusic(string musicName)
	{
		return this.audioMusic.clip != null && string.Compare(this.audioMusic.clip.name, musicName) == 0;
	}

	public AudioClip GetAudioClip(string soundName)
	{
		if (this.sfxDictionary.ContainsKey(soundName))
		{
			return this.sfxDictionary[soundName];
		}
		return null;
	}

	public void AdjustSoundVolume(float vol)
	{
		this.audioSfx.volume = vol;
	}

	public void AdjustMusicVolume(float vol)
	{
		this.audioMusic.volume = vol;
	}

	public void SetMute(bool isMute)
	{
		this.audioMusic.mute = isMute;
		this.audioSfx.mute = isMute;
	}
	public void ChangeMultiplayerMenuMusic(int clip) 
	{
		if (clip == 0)
		{
			audioMusic.clip = multiplayerSound;
			audioMusic.Play();
		}
		else
		{
			audioMusic.clip = musicClips[3];
			audioMusic.Play();
		}
		
	}
}
