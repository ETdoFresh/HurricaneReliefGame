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
        private readonly Queue<DialogData> _conversationQueue = new Queue<DialogData>();
        private bool _proceedToNextConversation;
        private Coroutine _coroutine;

        private void OnEnable()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (!eventSystem) Instantiate(eventSystemPrefab);
            inputAction.Enable();
            var simpleDialogActionMap = inputAction.FindActionMap("SimpleDialog");
            var simpleDialogNextAction = simpleDialogActionMap.FindAction("Next");
            simpleDialogNextAction.performed += OnNextAction;
            simpleDialogNextAction.started += OnNextAction;
        }

        private void OnNextAction(InputAction.CallbackContext obj)
        {
            _proceedToNextConversation = true;
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

        public void StartConversation(List<DialogData> newConversation)
        {
            _conversationQueue.Clear();
            _conversationQueue.EnqueueRange(newConversation);
            _coroutine = StartCoroutine(ShowConversationForDuration());
        }

        private IEnumerator ShowConversationForDuration()
        {
            while (_conversationQueue.IsNotEmpty())
            {
                var conversation = _conversationQueue.Dequeue();
                ShowImage(conversation);
                textUI.text = conversation.text;
                //StartCoroutine(WaitFiveSeconds());
                yield return new WaitUntil(() => _proceedToNextConversation);
                _proceedToNextConversation = false;
            }
            Destroy(gameObject);
        }

        private IEnumerator WaitFiveSeconds()
        {
            yield return new WaitForSeconds(5);
            _proceedToNextConversation = true;
        }

        private void ShowImage(DialogData dialogData)
        {
            leftImage.sprite = dialogData.sprite;
            centerImage.sprite = dialogData.sprite;
            rightImage.sprite = dialogData.sprite;
            leftImage.enabled = dialogData.sprite && dialogData.side == DialogPlacement.Left;
            centerImage.enabled = dialogData.sprite && dialogData.side == DialogPlacement.Center;
            rightImage.enabled = dialogData.sprite && dialogData.side == DialogPlacement.Right;
        }
    }
}