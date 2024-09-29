using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class AudioPlayerPool : MonoBehaviour
{
    [SerializeField] private AudioPlayer audioSourcePrefab;
    [SerializeField] private int initialPoolSize = 60;

    private Queue<AudioPlayer> _audioPlayerPool = new Queue<AudioPlayer>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AudioPlayer audioPlayer = Instantiate(audioSourcePrefab, transform);
            audioPlayer.gameObject.SetActive(false);
            _audioPlayerPool.Enqueue(audioPlayer);
        }
    }
    
    public AudioPlayer GetAudioPlayerFromPool(Vector3 position) //Gets an AudioPlayer from teh pool or crates one.
    {
        AudioPlayer audioPlayer;
        
        if (_audioPlayerPool.Count > 0)
        {
            audioPlayer = _audioPlayerPool.Dequeue();
        }
        else
        {
            audioPlayer = Instantiate(audioSourcePrefab, transform);
        }

        audioPlayer.transform.position = position;
        audioPlayer.gameObject.SetActive(true);
        return audioPlayer;
    }
    
    public void ReturnAudioPlayerToPool(AudioPlayer audioPlayer) //Returns AudioPlayer to pool.
    {
        audioPlayer.Stop();
        audioPlayer.gameObject.SetActive(false);
        _audioPlayerPool.Enqueue(audioPlayer);
    }
}
