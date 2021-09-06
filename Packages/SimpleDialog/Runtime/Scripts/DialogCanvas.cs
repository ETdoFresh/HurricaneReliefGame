using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static CodeExtensions.QueueT;

namespace SimpleDialog
{
    public class DialogCanvas : MonoBehaviour
    {
        [SerializeField] private Text textUI;
        [SerializeField] private Image leftImage;
        [SerializeField] private Image centerImage;
        [SerializeField] private Image rightImage;
        [SerializeField] private InputActionAsset inputAction;
        [SerializeField] private GameObject eventSystemPrefab;
        private readonly Queue<DialogDataStruct> _conversationQueue = new Queue<DialogDataStruct>();
        private bool _proceedToNextConversation;
        private Coroutine _coroutine;
        private bool _skipText;

        private void OnEnable()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (!eventSystem) Instantiate(eventSystemPrefab);
            inputAction.Enable();
            var simpleDialogActionMap = inputAction.FindActionMap("SimpleDialog");
            var simpleDialogNextAction = simpleDialogActionMap.FindAction("Next");
            simpleDialogNextAction.performed += OnNextAction;
        }

        private void OnDisable()
        {
            inputAction.Disable();
            var simpleDialogActionMap = inputAction.FindActionMap("SimpleDialog");
            var simpleDialogNextAction = simpleDialogActionMap.FindAction("Next");
            simpleDialogNextAction.performed -= OnNextAction;
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void OnNextAction(InputAction.CallbackContext obj)
        {
            if (_skipText)
                _proceedToNextConversation = true;
            else
                _skipText = true;
        }

        public void StartConversation(DialogScriptableObject dialogScriptableObject)
        {
            _coroutine = StartCoroutine(ShowConversationForDuration(dialogScriptableObject));
        }

        private IEnumerator ShowConversationForDuration(DialogScriptableObject dialogScriptableObject)
        {
            _conversationQueue.Clear();
            _conversationQueue.EnqueueRange(dialogScriptableObject.Conversation);
            dialogScriptableObject.dialogStart.Invoke();
            while (_conversationQueue.IsNotEmpty())
            {
                var conversation = _conversationQueue.Dequeue();
                ShowImage(conversation);
                textUI.text = "";
                var timeBank = 0f;
                while (textUI.text.Length < conversation.text.Length)
                {
                    yield return null;
                    if (conversation.charactersPerSecond == 0) continue;
                    timeBank += Time.unscaledDeltaTime;
                    var secondsPerCharacter = Mathf.Abs(1f / conversation.charactersPerSecond);
                    while (timeBank >= secondsPerCharacter && textUI.text.Length < conversation.text.Length &&
                           textUI.text.Length >= 0)
                    {
                        timeBank -= secondsPerCharacter;
                        if (conversation.charactersPerSecond > 0)
                            textUI.text += conversation.text[textUI.text.Length];
                        else
                            textUI.text = textUI.text.Remove(0, textUI.text.Length - 1);
                    }

                    if (!_skipText) continue;
                    textUI.text = conversation.text;
                    break;
                }

                _skipText = true;
                yield return new WaitUntil(() => _proceedToNextConversation);
                _skipText = false;
                _proceedToNextConversation = false;
            }
            dialogScriptableObject.dialogEnd.Invoke();
            Destroy(gameObject);
        }

        private void ShowImage(DialogDataStruct dialogDataStruct)
        {
            leftImage.sprite = dialogDataStruct.sprite;
            centerImage.sprite = dialogDataStruct.sprite;
            rightImage.sprite = dialogDataStruct.sprite;
            leftImage.enabled = dialogDataStruct.sprite && dialogDataStruct.side == DialogPlacementEnum.Left;
            centerImage.enabled = dialogDataStruct.sprite && dialogDataStruct.side == DialogPlacementEnum.Center;
            rightImage.enabled = dialogDataStruct.sprite && dialogDataStruct.side == DialogPlacementEnum.Right;
        }
    }
}