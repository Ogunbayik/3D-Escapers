using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollectibleItem : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Item Data")]
    [SerializeField] private string _itemName;
    [SerializeField] private int _itemID;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerBase>())
        {
            _signalBus.Fire(new GameSignal.OnCollectableItemCollected(this));
        }
    }
}
