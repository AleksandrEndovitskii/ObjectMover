using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class ExplosionManager : BaseManager<ExplosionManager>
    {
        [SerializeField]
        private ParticleSystem _explosionPrefab;
        [SerializeField]
        private float _explosionDurationSeconds = 1f;
        [SerializeField]
        private AudioClip _audioClip;

        private AudioSource _audioSource;

        private ParticleSystem _explosionInstance;

        public override void Initialize()
        {
            _audioSource = this.gameObject.GetComponent<AudioSource>();
        }
        public override void UnInitialize()
        {
        }

        public override void Subscribe()
        {
        }
        public override void UnSubscribe()
        {
        }

        public void Explode(GameObject go)
        {
            _explosionInstance = Instantiate(_explosionPrefab, go.transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_audioClip);
            Destroy(go);
            var explosionStoppingCoroutine = StartCoroutine(InvokeActionAfterDelayCoroutine(() =>
                {
                    Destroy(_explosionInstance);
                },
                _explosionDurationSeconds));
        }

        private IEnumerator InvokeActionAfterDelayCoroutine(Action action, float delaySeconds = 1f)
        {
            yield return new WaitForSeconds(delaySeconds);

            action?.Invoke();
        }
    }
}
