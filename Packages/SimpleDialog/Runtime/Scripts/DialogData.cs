using System;
using UnityEngine;

namespace SimpleDialog
{
    [Serializable]
    public struct DialogData
    {
        public Sprite sprite;
        public DialogPlacement side;
        [Multiline] public string text;
    }
}