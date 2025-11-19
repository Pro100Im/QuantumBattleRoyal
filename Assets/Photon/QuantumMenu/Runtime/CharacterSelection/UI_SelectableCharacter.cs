using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuantumMenu.Runtime.CharacterSelection
{
    public class UI_SelectableCharacter : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private GameObject characterSelected;

        private CharacterModel _characterModel;
             
        public static event Action<CharacterModel> OnCharacterSelected;

        public void Initialize(CharacterModel characterModel)
        {
            characterImage.sprite = characterModel.CharacterImage;
            characterName.text = characterModel.CharacterName;

            _characterModel = characterModel;
        }

        public void CharacterSelected()
        {
            OnCharacterSelected?.Invoke(_characterModel);
        }

        public void SetSelected(bool isSelected)
        {
            characterSelected.SetActive(isSelected);
        }
    }
}