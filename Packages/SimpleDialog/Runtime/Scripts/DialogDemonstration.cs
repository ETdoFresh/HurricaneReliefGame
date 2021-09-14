using CodeExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static CodeExtensions.ObjectExtension;

namespace SimpleDialog
{
    public class DialogDemonstration : MonoBehaviour
    {
        [SerializeField] private DialogScriptableObject dialogScriptableObject;
        [SerializeField] private InputAction restartAction;

        private void OnValidate()
        {
            dialogScriptableObject = FindIfNull(dialogScriptableObject);
        }

        private void OnEnable()
        {
            dialogScriptableObject.dialogStart.AddPersistentListener(OnDialogStart);
            dialogScriptableObject.dialogEnd.AddPersistentListener(OnDialogEnd);
            dialogScriptableObject.DisplayDialog();
            restartAction.performed += OnRestart;
            restartAction.Enable();
        }

        private void OnDisable()
        {
            dialogScriptableObject.dialogStart.RemovePersistentListener(OnDialogStart);
            dialogScriptableObject.dialogEnd.RemovePersistentListener(OnDialogEnd);
            restartAction.performed -= OnRestart;
        }

        private void OnDialogStart()
        {
            Functions.Pause();
        }

        private void OnDialogEnd()
        {
            Functions.Resume();
        }

        private void OnRestart(InputAction.CallbackContext obj)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}