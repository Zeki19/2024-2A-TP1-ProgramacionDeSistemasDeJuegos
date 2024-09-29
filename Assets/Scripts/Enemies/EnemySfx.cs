using System;
using System.Collections;
using Audio;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemySfx : MonoBehaviour
    {
        [SerializeField] private AudioPlayerPool _audioPlayerPool;
        [SerializeField] private RandomContainer<AudioClipData> spawnClips;
        [SerializeField] private RandomContainer<AudioClipData> explosionClips;
        private Enemy _enemy;

        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            _audioPlayerPool = FindObjectOfType<AudioPlayerPool>();
            if (_audioPlayerPool == null) //If not found, turn it off.
            {
                Debug.LogError("AudioPlayerPool not in the scene!");
                enabled = false;
            }
        }

        private void FetchComponents()
        {
            // "a ??= b" is equivalent to "if(a == null) a = b" 
            _enemy ??= GetComponent<Enemy>();
        }
        
        private void OnEnable()
        {
            _enemy.OnSpawn += HandleSpawn;
            _enemy.OnDeath += HandleDeath;
        }
        
        private void OnDisable()
        {
            _enemy.OnSpawn -= HandleSpawn;
            _enemy.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            PlayRandomClip(explosionClips);
        }

        private void HandleSpawn()
        {
            PlayRandomClip(spawnClips);
        }

        private void PlayRandomClip(RandomContainer<AudioClipData> container)
        {
            if (!container.TryGetRandom(out var clipData)) return;

            AudioPlayer audioPlayer = _audioPlayerPool.GetAudioPlayerFromPool(transform.position);
            audioPlayer.Play(clipData);
            StartCoroutine(ReturnAudioPlayerAfterPlay(audioPlayer, clipData));
        }

        private IEnumerator ReturnAudioPlayerAfterPlay(AudioPlayer audioPlayer, AudioClipData clipData)
        {
            yield return new WaitForSeconds(clipData.Clip.length);
            _audioPlayerPool.ReturnAudioPlayerToPool(audioPlayer);
        }
    }
}
