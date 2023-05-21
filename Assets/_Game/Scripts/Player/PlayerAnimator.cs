using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator arm1Animator;
        [SerializeField] private Animator arm2Animator;
        private static readonly int StartDig = Animator.StringToHash("StartDig");
        private static readonly int StopDig = Animator.StringToHash("StopDig");
        private static readonly int Digging = Animator.StringToHash("Digging");

        public void StartDigAnim()
        {
            arm1Animator.SetTrigger(StartDig);
        }

        public void StopDigAnim()
        {
            arm1Animator.SetTrigger(StopDig);
            arm2Animator.SetBool(Digging, false);
        }

        //Event Function! Arm1Startanim
        public void StartArm2DigAnim()
        {
            arm2Animator.SetBool(Digging, true);
        }

        //Event Function Arm1ForStopanim
        public void StopArm2DigAnim()
        {
            arm2Animator.SetBool(Digging, false);
        }
    }
}