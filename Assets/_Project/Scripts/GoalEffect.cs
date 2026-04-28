using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class GoalEffect : MonoBehaviour, IPoolable<IMemoryPool>
{
    private IMemoryPool _pool;

    [Header("Particle References")]
    [SerializeField] private ParticleSystem _particleSystem;

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;

        Debug.Log("Spawned");

        if(_particleSystem != null)
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            _particleSystem.Simulate(0, true, true);

            _particleSystem.Play(true);
        }

        AutoReturn().Forget();
    }
    private async UniTask AutoReturn()
    {
        float particalDuration = _particleSystem.main.duration;

        await UniTask.Delay(System.TimeSpan.FromSeconds(particalDuration));

        ReturnToPool();
    }
    private void ReturnToPool() => _pool?.Despawn(this);
    public void OnDespawned()
    {
        _particleSystem.Stop();
        _particleSystem.Clear();
    }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool,GoalEffect> { }
}
