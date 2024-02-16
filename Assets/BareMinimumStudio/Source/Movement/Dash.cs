namespace BMS.Movement
{
    using BMS.Core;
    using System.Collections;
    using UnityEngine;

    public class Dash : Module
    {
        [Header("Dash Settings")]
        [SerializeField]
        private KeyCode dashKey;
        [SerializeField]
        private float dashSpeed;
        [SerializeField]
        private float dashCooldown;

        private WaitForSeconds slideCooldownWaitForSeconds;
        private Coroutine slideCooldownRoutine;
        private bool isSlide;

        public override void Awake()
        {
            base.Awake();
            slideCooldownWaitForSeconds = new WaitForSeconds(dashCooldown);
        }

        private void Update()
        {
            if (Input.GetKeyDown(dashKey))
            {
                if (CanSlide())
                {
                    Debug.Log("I can dash now");
                    isSlide = true;   
                }
            }
        }

        private void FixedUpdate()
        {
            if (isSlide)
            {
                Debug.Log(" I am dashing now");
                isSlide = false;
                Motor.AddImpulse(Motor.transform.forward * dashSpeed);
                slideCooldownRoutine = StartCoroutine(SlideCooldownRoutine());
            }
        }

        private bool CanSlide()
        {
            Debug.Log(isSlide);
            return slideCooldownRoutine == null && !isSlide;
        }

        private IEnumerator SlideCooldownRoutine()
        {
            yield return slideCooldownWaitForSeconds;
            slideCooldownRoutine = null;
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }
    }
}