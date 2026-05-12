using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollectibleItemFactory
{
    private readonly DiContainer _container;
    public CollectibleItemFactory(DiContainer container) => _container = container;
    public CollectibleItem Create(CollectibleItem prefab, Vector3 position)
    {
        return _container.InstantiatePrefabForComponent<CollectibleItem>(
            prefab,
            position,
            Quaternion.identity,
            null);
    }
}
