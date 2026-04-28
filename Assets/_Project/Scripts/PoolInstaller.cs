using UnityEngine;
using Zenject;

public class PoolInstaller : MonoInstaller
{
    [Header("VFX References")]
    [SerializeField] private GoalEffect _goalVFX;
    [Header("Group References")]
    [SerializeField] private Transform _vfxGroup;
    public override void InstallBindings()
    {
        Container.BindMemoryPool<GoalEffect, GoalEffect.Pool>()
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_goalVFX)
            .UnderTransform(_vfxGroup);

        Container.Bind<VFXManager>().AsSingle();
    }
}