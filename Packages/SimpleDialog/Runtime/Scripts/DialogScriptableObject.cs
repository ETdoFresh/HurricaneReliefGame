using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialog
{
    public class DialogScriptableObject : ScriptableObject
    {
        public UnityEvent dialogStart;
        public UnityEvent dialogEnd;
        
        [SerializeField] private List<DialogDataStruct> conversation = new List<DialogDataStruct>();
        [SerializeField] private GameObject dialogCanvasPrefab;
        private DialogCanvas _dialogCanvasInstance;

        public IEnumerable<DialogDataStruct> Conversation => conversation;

        public void DisplayDialog()
        {
            if (!_dialogCanvasInstance)
                _dialogCanvasInstance = Instantiate(dialogCanvasPrefab).GetComponent<DialogCanvas>();
            _dialogCanvasInstance.StartConversation(this);
        }
    }
}