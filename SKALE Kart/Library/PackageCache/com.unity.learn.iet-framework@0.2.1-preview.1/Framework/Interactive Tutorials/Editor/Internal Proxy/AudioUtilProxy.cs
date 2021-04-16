using UnityEditor;
using UnityEngine;

public static class AudioUtilProxy
{
    public static void PlayClip(AudioClip audioClip)
    {
        AudioUtil.PlayClip(audioClip);
    }
}
