using DG.Tweening;
using UnityEngine;
using Zenject;

public class CollectibleItem : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Item Data")]
    [SerializeField] private string _itemName;
    [SerializeField] private int _itemID;
    [Header("Idle Animation Settings")]
    [SerializeField] private float _idleMoveDistance;
    [SerializeField] private float _idleCycleDuration;
    [SerializeField] private float _waitDuration;
    [Header("Collect Animation Setting")]
    [SerializeField] private float _riseDistance;
    [SerializeField] private float _riseDuration;
    [SerializeField] private Vector3 _riseRotation;
    [SerializeField] private float _rotationDuration;
    [SerializeField] private float _scaleUpValue;
    [SerializeField] private float _scaleUpDuration;
    [SerializeField] private float _scaleDownValue;
    [SerializeField] private float _scaleDownDuration;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayCollectAnimation();
            _signalBus.Fire(new GameSignal.OnCollectableItemCollected(this));
            var collider = GetComponent<BoxCollider>();
            collider.enabled = false;
        }
    }
    public void PlayIdleAnimation()
    {
        transform.DOKill();

        var idleSequence = DOTween.Sequence();

        idleSequence.Append(transform.DOMoveY(_idleMoveDistance, _idleCycleDuration).SetEase(Ease.InOutSine));
        idleSequence.Append(transform.DORotate(new Vector3(0f, 360f, 0f), _idleCycleDuration).SetEase(Ease.Linear));

        idleSequence.AppendInterval(_waitDuration);

        idleSequence.Append(transform.DOMoveY(transform.position.y, _idleCycleDuration).SetEase(Ease.InOutSine));
        idleSequence.Append(transform.DORotate(new Vector3(0f, 360f, 0f), _idleCycleDuration).SetEase(Ease.Linear));

        idleSequence.SetLoops(-1, LoopType.Restart);
    }
    public void PlayCollectAnimation()
    {
        transform.DOKill();

        var collectSequence = DOTween.Sequence();

        collectSequence.Append(transform.DOMoveY(_riseDistance, _riseDuration).SetEase(Ease.InOutSine));
        collectSequence.Join(transform.DORotate(_riseRotation, _rotationDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutCubic));

        collectSequence.Append(transform.DOScale(_scaleUpValue, _scaleUpDuration));
        collectSequence.Append(transform.DOScale(_scaleDownValue, _scaleDownDuration));

        collectSequence.OnComplete(() => gameObject.SetActive(false));
    }
}
