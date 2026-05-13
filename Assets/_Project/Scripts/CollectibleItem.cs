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
    [SerializeField] private Vector3 _idleRotation;
    [Header("Collect Animation Setting")]
    [SerializeField] private float _riseDistance;
    [SerializeField] private float _riseDuration;
    [SerializeField] private Vector3 _riseRotation;
    [SerializeField] private float _scaleUpValue;
    [SerializeField] private float _scaleUpDuration;
    [SerializeField] private float _scaleDownValue;
    [SerializeField] private float _scaleDownDuration;

    private bool _isCollected = false;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;

        if (other.gameObject.CompareTag("Player"))
        {
            _isCollected = true;

            PlayCollectAnimation();
            _signalBus.Fire(new GameSignal.OnCollectableItemCollected(this));
        }
    }
    public void PlayIdleAnimation()
    {
        transform.DOKill();

        var idleSequence = DOTween.Sequence();

        idleSequence.Append(transform.DOMoveY(_idleMoveDistance, _idleCycleDuration).SetEase(Ease.InOutSine));
        idleSequence.Join(transform.DORotate(_idleRotation, _idleCycleDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));

        idleSequence.Append(transform.DOMoveY(transform.position.y, _idleCycleDuration).SetEase(Ease.InOutSine));
        idleSequence.Join(transform.DORotate(_idleRotation, _idleCycleDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));

        idleSequence.SetLoops(-1, LoopType.Restart);
    }
    public void PlayCollectAnimation()
    {
        transform.DOKill();

        var collectSequence = DOTween.Sequence();

        collectSequence.Append(transform.DOMoveY(0f, 1f));

        collectSequence.Append(transform.DOMoveY(transform.position.y + _riseDistance, _riseDuration).SetEase(Ease.InOutSine));
        collectSequence.Join(transform.DORotate(_riseRotation, _riseDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));

        collectSequence.Append(transform.DOScale(_scaleUpValue, _scaleUpDuration));
        collectSequence.Append(transform.DOScale(_scaleDownValue, _scaleDownDuration));

        collectSequence.OnComplete(() => gameObject.SetActive(false));
    }
}
