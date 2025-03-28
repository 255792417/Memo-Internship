using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Clip -> 对应音频，名字，播放轨道
    [System.Serializable]
    public class Clip
    {
        public AudioClip clip;
        public string name;
        public string targetMixerGroup;
    }
    public List<Clip> clipList;

    // MixerGroup -> 混音器轨道， 名字
    [System.Serializable]
    public class MixerGroup
    {
        public AudioMixerGroup mixer;
        public string name;
    }
    public List<MixerGroup> mixerList;

    // 混音器轨道字典
    Dictionary<string, AudioMixerGroup> mixerDict = new Dictionary<string, AudioMixerGroup>();
    // 音频片段字典
    Dictionary<string, Clip> clipDictionary = new Dictionary<string, Clip>();
    // 播放轨道列表
    List<AudioSource> audios = new List<AudioSource>();

    // 最大播放轨道数
    public int sourceNumber = 10;
    // 当前可使用的轨道编号
    public int currentSourceNumber = 0;

    private void Awake()
    {

        for (int i = 0; i < sourceNumber; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }

        // 初始化clip字典
        for(int i = 0; i < clipList.Count; i++)
        {
            clipDictionary.Add(clipList[i].name, clipList[i]);
        }

        // 初始化mixer字典
        foreach (var mixerGroup in mixerList)
        {
            mixerDict.Add(mixerGroup.name, mixerGroup.mixer);
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
            audio.clip = clip.clip;
            audio.loop = isLoop;
            audio.outputAudioMixerGroup = mixerDict[clip.targetMixerGroup];
            audio.Play();
        }
    }

    // 使用固定轨道播放
    public void PlayWithFixedSource(int index, string name, bool isLoop)
    {
        var clip = this.clipDictionary[name];
        if (clip != null && !audios[index].isPlaying)
        {
            var audio = audios[index];
            audio.clip = clip.clip;
            audio.loop = isLoop;
            audio.outputAudioMixerGroup= mixerDict[clip.targetMixerGroup];
            audio.Play();
        }
    }
}
