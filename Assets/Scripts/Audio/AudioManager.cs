using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Clip
    {
        public AudioClip clip;
        public string name;
    }
    public List<Clip> clipList;

    public AudioMixer audioMixer;

    Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    List<AudioSource> audios = new List<AudioSource>();
    public int sourceNumber = 10;
    public int currentSourceNumber = 0;

    private void Awake()
    {
        for (int i = 0; i < sourceNumber; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }

        for(int i = 0; i < clipList.Count; i++)
        {
            clipDictionary.Add(clipList[i].name, clipList[i].clip);
        }

        PlayWithFixedSource(0,"Bgm", true);
    }

    public void Play(string name, bool isLoop)
    {
        if(currentSourceNumber == sourceNumber)
        {
            // 留出一个给BGM, 一个给Fly, 一个给Drill
            currentSourceNumber = 3;
        }
        var clip = this.clipDictionary[name];
        if(clip != null)
        {
            var audio = audios[currentSourceNumber++];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }

    public void PlayWithFixedSource(int index, string name, bool isLoop)
    {
        var clip = this.clipDictionary[name];
        if (clip != null && !audios[index].isPlaying)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }
}
