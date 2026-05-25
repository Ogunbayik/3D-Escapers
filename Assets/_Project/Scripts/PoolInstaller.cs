using UnityEngine;
using Zenject;

public class PoolInstaller : MonoInstaller
{
    [Header("VFX References")]
    [SerializeField] private GoalEffect _goalVFX;
    [Header("Prefab References")]
    [SerializeField] private GridCellView _gridCell;
    [Header("Pool Settings")]
    [SerializeField] private int _poolCount;
    [Header("Group References")]
    [SerializeField] private Transform _vfxGroup;
    [SerializeField] private Transform _gridGroup;
    public override void InstallBindings()
    {
        Container.BindMemoryPool<GoalEffect, GoalEffect.Pool>()
            .WithInitialSize(_poolCount)
            .FromComponentInNewPrefab(_goalVFX)
            .UnderTransform(_vfxGroup);

        Container.BindMemoryPool<GridCellView, GridCellView.Pool>()
            .FromComponentInNewPrefab(_gridCell)
            .UnderTransform(_gridGroup);

        Container.Bind<VFXManager>().AsSingle();
    }
}