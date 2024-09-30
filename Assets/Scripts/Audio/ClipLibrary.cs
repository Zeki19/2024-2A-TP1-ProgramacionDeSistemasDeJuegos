using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class ClipLibrary : MonoBehaviour
{
    [SerializeField] private List<AudioClipData> spawnClips;    
    [SerializeField] private List<AudioClipData> explosionClips;
    
    public AudioClipData GetClipData(string clipType)
    {
        switch (clipType)
        {
            case "Spawn":
                return GetRandomClip(spawnClips);
            case "Explosion":
                return GetRandomClip(explosionClips);
            default:
                return default;
        }
    }

    private AudioClipData GetRandomClip(List<AudioClipData> clipList)
    {
        if (clipList == null || clipList.Count == 0)
        {
            Debug.LogWarning("Clip list is empty or null!");
            return default;
        }

        int randomIndex = Random.Range(0, clipList.Count);
        return clipList[randomIndex];
    }
    
}
