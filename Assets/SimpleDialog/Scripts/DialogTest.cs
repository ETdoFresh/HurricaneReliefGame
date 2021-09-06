using UnityEngine;
using ScriptableObject = SimpleDialog.Utility.ScriptableObject;

namespace SimpleDialog
{
    public class DialogTest : MonoBehaviour
    {
        [SerializeField] private DialogDB dialogDB;
        [SerializeField] private DialogCharacter dialogCharacter;

        private void OnValidate()
        {
            if (!dialogDB) dialogDB = ScriptableObject.Find<DialogDB>();
            if (!dialogCharacter) dialogCharacter = ScriptableObject.Find<DialogCharacter>();
        }

        private void Start()
        {
            //dialogDB.Show(0);
            dialogCharacter.DisplayDialog();
        }
    }
}