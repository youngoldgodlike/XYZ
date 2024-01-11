using System;
using System.Collections;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Text _playerText;
        [SerializeField] private Text _alienText;
        [SerializeField] private GameObject _playerContainer;
        [SerializeField] private GameObject _alienContainer;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _alienSprite;
        [SerializeField] private Image _playerSprite;

        [Space] [SerializeField] private float _textSpeed = 0.09f;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private int _currentSentenceCount;
        private int _currentTurnCount;
        private bool _isDialog;

        private DialogData _data;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private Text _text;

        public bool IsDialog => _isDialog;

        private string _currentSentence => _data.DialogCharacterData[_currentTurnCount].Sentences[_currentSentenceCount];
        private EDialogCharacter _currentCharacter => _data.DialogCharacterData[_currentTurnCount].DialogCharacter;
        private int currentSestenceLength => _data.DialogCharacterData[_currentTurnCount].Sentences.Length;
        
        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }
        
        public void ShowDialog(DialogData data, Sprite alienSprite)
        {
            _isDialog = true;
            
            if (alienSprite != null)
                _alienSprite.overrideSprite = alienSprite;
            
            _data = data;
            _currentSentenceCount = 0;
            _currentTurnCount = 0;
            _playerText.text = String.Empty;
            _alienText.text = String.Empty;

            SetTurn(_currentCharacter);
            
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }

        public void OnSkip()
        {
            if (_typingRoutine == null) return;
            
            StopTypeAnimation();
            _text.text = _currentSentence;
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentenceCount++;
            
            var isDialogCompleted = _currentSentenceCount >= currentSestenceLength;
            if (isDialogCompleted)
            {
                if (_currentTurnCount + 1 >= _data.DialogCharacterData.Length )
                {
                    HideDialogBox();
                    _isDialog = false;
                }
                else
                {
                    _currentSentenceCount = 0;
                    _currentTurnCount++;
                    SetTurn(_currentCharacter);
                    OnStartDialogAnimation();
                }
            }
            else
            {
                OnStartDialogAnimation();
            }

        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);

            _typingRoutine = null;
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            ShowSprites(false);
            _sfxSource.PlayOneShot(_close);
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sestence = _currentSentence;
            
            foreach (var letter in sestence)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        private void OnStartDialogAnimation()
        {
            ShowSprites(true);
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private void OnCloseDialog()
        {
            _playerContainer.SetActive(false);
        }

        private void ShowSprites(bool isShow)
        {
            _alienSprite.gameObject.SetActive(isShow);
            _playerSprite.gameObject.SetActive(isShow);
        }

        private void SetTurn(EDialogCharacter character)
        {
            switch (character)
            {
                case EDialogCharacter.Player:
                    _playerContainer.SetActive(true);
                    _alienContainer.SetActive(false);
                    _text = _playerText;
                    break;
                case EDialogCharacter.Alien:
                    _alienContainer.SetActive(true);
                    _playerContainer.SetActive(false);
                    _text = _alienText;
                    break;
            }
        }
    }
}