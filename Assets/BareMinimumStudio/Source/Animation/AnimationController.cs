namespace BMS.Animation
{
    using BMS.Core;
    using BMS.Movement;
    using UnityEngine;

    public class AnimationController : Module
    {
        [SerializeField]
        private Animator animationController;
        private Movement movement;

        public override void Awake()
        {
            base.Awake();
            movement = (Movement)Motor.GetModule<Movement>();
            if (movement == null)
            {
                Debug.LogError("Movement module not found on motor");
            }
        }

        private void Update()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(Motor.Velocity);
            animationController.SetFloat("HorizontalAxisVel", localVelocity.x);
            animationController.SetFloat("VerticalAxisVel", localVelocity.z);
        }
    }
}