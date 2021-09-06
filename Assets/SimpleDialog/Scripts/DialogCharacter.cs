using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialog
{
    public class DialogCharacter : ScriptableObject
    {
        [SerializeField] private List<ConversationData> conversation = new List<ConversationData>();
        [SerializeField] private GameObject _dialogCanvasPrefab;
        private DialogCanvas _dialogCanvasInstance;

        public void DisplayDialog()
        {
            if (!_dialogCanvasInstance)
                _dialogCanvasInstance = Instantiate(_dialogCanvasPrefab).GetComponent<DialogCanvas>();
            _dialogCanvasInstance.StartConversation(conversation);
        }
    }
}