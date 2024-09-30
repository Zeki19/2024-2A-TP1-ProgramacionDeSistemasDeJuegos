using System;
using System.Collections;
using Audio;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemySfx : MonoBehaviour
    {
        private ClipLibrary clipLibrary;
        private AudioPlayerPool audioPlayerPool;
        
        private Enemy _enemy;

        private void Awake()
        {
            // Fetch components dynamically to assign references
            clipLibrary = FindObjectOfType<ClipLibrary>();
            audioPlayerPool = AudioPlayerPool.Instance;

            if (clipLibrary == null)
            {
                Debug.LogError("ClipLibrary is not found in the scene! Please ensure it exists.");
            }

            if (audioPlayerPool == null)
            {
                Debug.LogError("AudioPlayerPool instance is not found! Make sure AudioPlayerPool is initialized in the scene.");
            }

            _enemy = GetComponent<Enemy>();

            if (_enemy != null)
            {
                _enemy.OnSpawn += HandleOnSpawn;
                _enemy.OnDeath += HandleOnDeath;
            }
        }

        private void OnDestroy()
        {
            if (_enemy != null)
            {
                _enemy.OnSpawn -= HandleOnSpawn;
                _enemy.OnDeath -= HandleOnDeath;
            }
        }

        private void HandleOnSpawn()
        {
            PlayClip("Spawn");
        }

        private void HandleOnDeath()
        {
            PlayClip("Explosion");
        }

        private void PlayClip(string clipType)
        {
            if (clipLibrary == null)
            {
                Debug.LogError("clipLibrary is not assigned in EnemySfx! Please check the prefab.");
                return;
            }
            
            AudioClipData clipData = clipLibrary.GetClipData(clipType);
            
            if (clipData.Clip == null)
            {
                Debug.LogWarning($"ClipData of type {clipType} not found in ClipLibrary!");
                return;
            }
            
            AudioPlayer audioPlayer = AudioPlayerPool.Instance.GetAudioPlayerFromPool();
            if (audioPlayer == null)
            {
                Debug.LogWarning("No available AudioPlayer in the pool!");
                return;
            }
            
            audioPlayer.Play(clipData);
            StartCoroutine(ReturnAudioPlayerToPoolAfterPlay(audioPlayer, clipData.Clip.length));
        }
        
        private IEnumerator ReturnAudioPlayerToPoolAfterPlay(AudioPlayer player, float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            AudioPlayerPool.Instance?.ReturnAudioPlayerToPool(player);
        }
    }
}
