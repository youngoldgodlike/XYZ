using System;
using UnityEngine;

namespace Assets.Scripts.Models.Data
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private DialogCharacterData[] _dialogCharacterData;

        public DialogCharacterData[] DialogCharacterData => _dialogCharacterData;
    }

    [Serializable]
    public struct DialogCharacterData
    {
        [SerializeField] private EDialogCharacter _dialogCharacter;
        [SerializeField] private string[] _sentences;

        public EDialogCharacter DialogCharacter => _dialogCharacter;
        public string[] Sentences => _sentences;
    }

    public enum EDialogCharacter
    {
        Player,
        Alien
    }
}