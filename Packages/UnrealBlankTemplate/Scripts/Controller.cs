using System;
using UnityEngine;

namespace UnrealBase
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] protected Pawn pawn;

        [Header("Controller")]
        [SerializeField] private GameObject playerState;

        [SerializeField] private GameObject startSpot;
        [SerializeField] private string stateName;

        protected virtual void Awake()
        {
        }

        public Character GetCharacter()
        {
            return !pawn ? null : pawn.GetComponent<Character>();
        }

        public Quaternion GetControlRotation() => transform.rotation;

        public Pawn GetPawn()
        {
            return pawn;
        }

        public bool IsPlayerController() => this is PlayerController;

        public virtual void Possess(Pawn newPawn)
        {
            pawn = newPawn;
            if (pawn) pawn.SetController(this);
        }

        public virtual void UnPossess()
        {
            pawn = null;
        }
    }
}