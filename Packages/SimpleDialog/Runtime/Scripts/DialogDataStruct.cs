using System;
using UnityEngine;

namespace SimpleDialog
{
    [Serializable]
    public struct DialogDataStruct
    {
        public Sprite sprite;
        public DialogPlacementEnum side;
        [Multiline] public string text;
        public float charactersPerSecond;
    }
}