using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioMixerGroup audioMixerGroup;

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [Range(0f, 1f)]
    public float spatialBlend;

    [Range(1f,500f)]
    public float minDistance;

    [Range(1f,500f)]
    public float maxDistance;

    [HideInInspector]
    public AudioSource source;
}
