using System;
using CodeExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialog
{
    public class DialogDemonstration : MonoBehaviour
    {
        [SerializeField] private DialogScriptableObject dialogScriptableObject;

        private void OnValidate()
        {
            dialogScriptableObject = CodeExtensions.ScriptableObject.FindIfNull(dialogScriptableObject);
        }

        private void OnEnable()
        {
            dialogScriptableObject.dialogStart.AddPersistentListener(OnDialogStart);
            dialogScriptableObject.dialogEnd.AddPersistentListener(OnDialogEnd);
            dialogScriptableObject.DisplayDialog();
        }

        private void OnDisable()
        {
            dialogScriptableObject.dialogStart.AddPersistentListener(OnDialogStart);
            dialogScriptableObject.dialogEnd.AddPersistentListener(OnDialogEnd);
        }

        private void OnDialogStart()
        {
            Functions.Pause();
        }

        private void OnDialogEnd()
        {
            Functions.Resume();
        }
    }
}