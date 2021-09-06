using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleDialog.Utility;
using UnityEngine;
using Coroutine = UnityEngine.Coroutine;
using ScriptableObject = UnityEngine.ScriptableObject;

namespace SimpleDialog
{
    [CreateAssetMenu(menuName = "Scriptable Object/DialogDB", fileName = "DialogDB", order = 0)]
    public class DialogDB : ScriptableObject
    {
        public List<DialogData> data = new List<DialogData>();
        private Coroutine _showDialogCoroutine;
        private Queue<DialogData> _queue = new Queue<DialogData>();

        public string GetDialogById(int id) => GetDialogDataById(id).dialog;
        public float GetStartById(int id) => GetDialogDataById(id).start;
        public float GetEndById(int id) => GetDialogDataById(id).end;
        public float GetDurationById(int id) => GetDialogDataById(id).duration;

        private DialogData GetDialogDataById(int id) => data.FirstOrDefault(x => x.id == id);

        public void Show(int id)
        {
            var dialogData = GetDialogDataById(id);
            if (string.IsNullOrEmpty(dialogData.dialog)) return;
            if (_showDialogCoroutine != null)
                _queue.Enqueue(dialogData);
            else
                _showDialogCoroutine = SimpleDialog.Utility.Coroutine.Start(ShowDialogForDuration(dialogData));
        }

        private IEnumerator ShowDialogForDuration(DialogData dialogData)
        {
            Debug.Log($"{dialogData.id} {dialogData.dialog} Start");
            yield return new WaitForSeconds(dialogData.duration);
            Debug.Log($"{dialogData.id} End");
            if (_queue.IsNotEmpty())
                _showDialogCoroutine = SimpleDialog.Utility.Coroutine.Start(ShowDialogForDuration(dialogData));
            else
                _showDialogCoroutine = null;
        }
    }
}
