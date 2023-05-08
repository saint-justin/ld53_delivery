using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[SerializeField]
	private AudioSource _audioSourceMusic;

	[SerializeField]
	private AudioSource _audioSourceSound;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}


	public void PlaySound(AudioClip sound)
	{
		if (sound != null)
		{
			_audioSourceSound.PlayOneShot(sound);
		}
	}


	public void PlayMusic(AudioClip music, bool loop)
	{
		if (music != null)
		{
			_audioSourceMusic.clip = music;
			_audioSourceMusic.Play();
			_audioSourceMusic.loop = loop;
		}
	}


	public void PlaySoundSequence(AudioClip[] sounds)
	{
		StartCoroutine(PlaySounds(sounds));
	}


	private IEnumerator PlaySounds(AudioClip[] sounds)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i] != null)
			{
				_audioSourceSound.PlayOneShot(sounds[i]);
			}

			yield return new WaitForSeconds(sounds[i].length);
		}
	}
}
