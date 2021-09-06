using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SimpleDialog.Utility.QueueT;

namespace SimpleDialog
{
    public class DialogCanvas : MonoBehaviour
    {
        [SerializeField] private Text textUI;
        [SerializeField] private Image leftImage;
        [SerializeField] private Image centerImage;
        [SerializeField] private Image rightImage;
        private readonly Queue<ConversationData> _conversationQueue = new Queue<ConversationData>();
        private bool _proceedToNextConversation;
        private Coroutine _coroutine;

        private void OnDisable()
        {
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        public void StartConversation(List<ConversationData> newConversation)
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
                StartCoroutine(WaitFiveSeconds());
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

        private void ShowImage(ConversationData conversationData)
        {
            leftImage.sprite = conversationData.sprite;
            centerImage.sprite = conversationData.sprite;
            rightImage.sprite = conversationData.sprite;
            leftImage.enabled = conversationData.sprite && conversationData.side == ConversationSide.Left;
            centerImage.enabled = conversationData.sprite && conversationData.side == ConversationSide.Center;
            rightImage.enabled = conversationData.sprite && conversationData.side == ConversationSide.Right;
        }
    }
}