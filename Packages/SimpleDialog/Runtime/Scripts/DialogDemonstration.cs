using UnityEngine;

namespace SimpleDialog
{
    public class DialogDemonstration : MonoBehaviour
    {
        [SerializeField] private DialogCharacter dialogCharacter;

        private void OnValidate()
        {
            dialogCharacter = CodeExtensions.ScriptableObject.FindIfNull(dialogCharacter);
        }

        private void OnEnable()
        {
            dialogCharacter.DisplayDialog();
        }
    }
}