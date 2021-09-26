using UnityEngine;

namespace UnrealBlankTemplate
{
    public class SpringArm : MonoBehaviour
    {
        [SerializeField] private bool usePawnControlRotation;
        
        private Pawn _pawn;

        private void Awake()
        {
            _pawn = GetComponentInParent<Pawn>();
        }

        private void Update()
        {
            if (usePawnControlRotation)
                transform.rotation = _pawn.GetController().transform.rotation;
        }
    }
}
