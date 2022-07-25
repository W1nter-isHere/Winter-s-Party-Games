using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.Audio;

namespace Menu
{
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool doShake = true;
        
        private TextMeshProUGUI _text;
        private RectTransform _textTransform;

        private Tween _shake;
        private bool _wasEnabled;
        private Button _button;
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _textTransform = _text.rectTransform;
            InteractableChanged(_button.interactable);
        }

        private void Update()
        {
            var newState = _button.IsInteractable();
            if (_wasEnabled != newState)
            {
                InteractableChanged(newState);
            }
            _wasEnabled = newState;
        }

        private void InteractableChanged(bool interactable)
        {
            _text.color = interactable ? Color.white : Color.gray;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            SoundManager.Play(GameConstants.Sounds.ButtonHover);
            _textTransform.DOScale(1.3f, 0.1f).SetEase(Ease.OutCubic);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            _textTransform.DOScale(1f, 0.1f).SetEase(Ease.InCubic);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            _textTransform.DOScale(1.5f, 0.1f).SetEase(Ease.OutCubic);
            _text.color = Color.gray;
            if (doShake)
            {
                _shake = _textTransform.DOLocalRotate(new Vector3(0, 0, 3f), 0.1f).SetLoops(-1, LoopType.Yoyo);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            if (Math.Abs(_textTransform.localScale.x - 1) > 0.1f)
            {
                _textTransform.DOScale(1.3f, 0.1f).SetEase(Ease.OutCubic);
            }

            _textTransform.DOLocalRotate(Vector3.zero, 0.1f);
            _text.color = Color.white;
            if (doShake)
            {
                _shake.Kill();
                _shake = null;
            }
        }
    }
}