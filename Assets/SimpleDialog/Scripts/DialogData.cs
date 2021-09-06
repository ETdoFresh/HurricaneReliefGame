using System;
using UnityEngine;

namespace SimpleDialog
{
    [Serializable]
    public struct DialogData
    {
        public int id;
        [Multiline] public string dialog;
        public DialogType dialogType;
        public float start;
        public float duration;
        public float end;
    }

    public enum DialogType
    {
        Skip_Walk,
        Skip_Still,
        NoSkip_Walk,
        NoSkip_Still,
        Timed_Skip_Walk,
        Timed_Skip_Still,
        Timed_NoSkip_Walk,
        Timed_NoSkip_Still,
    }
}