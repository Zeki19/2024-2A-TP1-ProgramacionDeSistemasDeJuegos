using System.Collections;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioClip))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private OnFinishAction finishAction;
        private AudioSource _source;

        private AudioSource Source
        {
            get
            {
                _source ??= GetComponent<AudioSource>();
                return _source;
            }
        }
        
        public enum OnFinishAction
        {
            None,
            Deactivate,
        }
        
        public void Play(AudioClipData data)
        {
            Source.loop = data.Loop;
            Source.clip = data.Clip;
            Source.outputAudioMixerGroup = data.Group;
            Source.Play();
            var clipLength = data.Clip.length;
            if (finishAction == OnFinishAction.Deactivate)
            {
                StartCoroutine(DeactivateIn(clipLength));
            }
        }
        
        public void Stop()
        {
            Source.Stop();
        }
        
        private IEnumerator DeactivateIn(float seconds)
        {
            yield return new WaitForSeconds(Mathf.Max(seconds, 0));
            gameObject.SetActive(false);
        }
    }
}
