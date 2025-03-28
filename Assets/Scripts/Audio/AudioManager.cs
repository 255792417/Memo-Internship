using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Clip -> ��Ӧ��Ƶ�����֣����Ź��
    [System.Serializable]
    public class Clip
    {
        public AudioClip clip;
        public string name;
        public string targetMixerGroup;
    }
    public List<Clip> clipList;

    // MixerGroup -> ����������� ����
    [System.Serializable]
    public class MixerGroup
    {
        public AudioMixerGroup mixer;
        public string name;
    }
    public List<MixerGroup> mixerList;

    // ����������ֵ�
    Dictionary<string, AudioMixerGroup> mixerDict = new Dictionary<string, AudioMixerGroup>();
    // ��ƵƬ���ֵ�
    Dictionary<string, Clip> clipDictionary = new Dictionary<string, Clip>();
    // ���Ź���б�
    List<AudioSource> audios = new List<AudioSource>();

    // ��󲥷Ź����
    public int sourceNumber = 10;
    // ��ǰ��ʹ�õĹ�����
    public int currentSourceNumber = 0;

    private void Awake()
    {

        for (int i = 0; i < sourceNumber; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }

        // ��ʼ��clip�ֵ�
        for(int i = 0; i < clipList.Count; i++)
        {
            clipDictionary.Add(clipList[i].name, clipList[i]);
        }

        // ��ʼ��mixer�ֵ�
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
            // ����һ����BGM, һ����Fly, һ����Drill
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

    // ʹ�ù̶��������
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
