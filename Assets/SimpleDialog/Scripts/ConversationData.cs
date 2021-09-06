using System;
using UnityEngine;

namespace SimpleDialog
{
    [Serializable]
    public struct ConversationData
    {
        public Sprite sprite;
        public ConversationSide side;
        [Multiline] public string text;
    }
}