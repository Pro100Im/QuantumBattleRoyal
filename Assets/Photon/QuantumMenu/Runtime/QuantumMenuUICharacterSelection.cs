using QuantumMenu.Runtime.CharacterSelection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum.Menu
{
    public class QuantumMenuUICharacterSelection : QuantumMenuUIScreen
    {
        [SerializeField] private CharacterModel[] characterModels;
        [SerializeField] private UI_SelectableCharacter uI_SelectableCharacter;
        [SerializeField] private Transform characterSelectionParent;
        [SerializeField] protected QuantumMenuUIController quantumMenuUIController;

        private Dictionary<CharacterModel, UI_SelectableCharacter> _selectableCharacters = new();
        private CharacterModel _currentSelectedCharacter;

        public override void Awake()
        {
            UI_SelectableCharacter.OnCharacterSelected += OnCharacterSelected;

            InitializeCharacterSelection();
        }

        private void OnCharacterSelected(CharacterModel model)
        {
            if(_currentSelectedCharacter == model)
                return;

            if(_currentSelectedCharacter != null)
            {
                var previousSelectableCharacter = _selectableCharacters[_currentSelectedCharacter];
                previousSelectableCharacter.SetSelected(false);
            }

            _selectableCharacters[model].SetSelected(true);
            _currentSelectedCharacter = model;

            quantumMenuUIController.ConnectArgs.RuntimePlayers[0].PlayerAvatar = model.CharacterPrototype;
        }  

        private void InitializeCharacterSelection()
        {
            for (int i = 0; i < characterModels.Length; i++)
            {
                var characterModel = characterModels[i];
                var characterSelectionInstance = Instantiate(uI_SelectableCharacter, characterSelectionParent);
                characterSelectionInstance.Initialize(characterModel);
                _selectableCharacters.Add(characterModel, characterSelectionInstance);
            }  
            
            OnCharacterSelected(characterModels[0]);
        }

        public virtual void OnBackButtonPressed()
        {
            Controller.Show<QuantumMenuUIMain>();
        }

        private void OnDestroy()
        {
            UI_SelectableCharacter.OnCharacterSelected -= OnCharacterSelected;
        }
    }
}
