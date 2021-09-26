using CodeExtensions;
using UnityEngine;
using UnrealBlankTemplate;

[RequireComponent(typeof(Animator))]
public class ThirdPerson_AnimBP : MonoBehaviour
{
    private Animator _animator;
    private Pawn _pawn;
    private Character _character;
    
    private static readonly int IsInAir = Animator.StringToHash("IsInAir?");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_pawn) _pawn = GetComponentInParent<Pawn>();
        if (!_pawn) return;
        if (!_character) _character = _pawn.GetComponent<Character>();
        if (!_character) return;
        _animator.SetBool(IsInAir, _character.IsFalling);
        _animator.SetFloat(Speed, _character.Velocity.GetX0Z().magnitude);
    }
}
