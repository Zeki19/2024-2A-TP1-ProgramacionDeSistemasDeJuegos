using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class AudioPlayerPool : MonoBehaviour
{
    public static AudioPlayerPool Instance { get; private set; }
    
    [SerializeField] private AudioPlayer audioSourcePrefab;
    [SerializeField] private int initialPoolSize;

    private Queue<AudioPlayer> _audioPlayerPool = new Queue<AudioPlayer>();

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
        
        for (int i = 0; i < initialPoolSize; i++)
        {
            AudioPlayer audioPlayer = Instantiate(audioSourcePrefab, transform);
            audioPlayer.gameObject.SetActive(false);
            _audioPlayerPool.Enqueue(audioPlayer);
        }
    }
    
    public AudioPlayer GetAudioPlayerFromPool() //Gets an AudioPlayer from teh pool or crates one.
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
