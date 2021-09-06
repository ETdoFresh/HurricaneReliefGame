using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialog
{
    public class DialogCharacter : ScriptableObject
    {
        [SerializeField] private List<DialogData> conversation = new List<DialogData>();
        [SerializeField] private GameObject dialogCanvasPrefab;
        private DialogCanvas _dialogCanvasInstance;

        public void DisplayDialog()
        {
            if (!_dialogCanvasInstance)
                _dialogCanvasInstance = Instantiate(dialogCanvasPrefab).GetComponent<DialogCanvas>();
            _dialogCanvasInstance.StartConversation(conversation);
        }
    }
}