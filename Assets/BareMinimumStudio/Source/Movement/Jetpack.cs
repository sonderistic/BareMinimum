namespace BMS.Movement
{
    using UnityEngine;
    using BMS.Core;
    using System.Collections;

    public class Jetpack : Module
    {
        [Header("Jetpack Settings")]
        [SerializeField]
        private KeyCode jetpackKey;
        [SerializeField]
        private float upwardForce;
        [SerializeField]
        private float maxSpeed;
        [SerializeField]
        private float fuelAmount;
        [SerializeField]
        private float fuelCapacity;
        [SerializeField]
        private float fuelConsumptionPerSec;
        [SerializeField]
        private float fuelRegenPerSec;
        [SerializeField]
        private float fuelRegenMultiplier;
        [SerializeField]
        private float fuelRegenDelaySec;

        private bool isJetpacking;
        private Coroutine regenerateFuelRoutine;
        private WaitForSeconds regenerateWaitForSeconds;
        private bool jetpackIsOnCooldown;
        private Coroutine jetpackCooldownRoutine;
        private WaitForSeconds waitForSecondsJetpackCooldown;

        private void Update()
        {
            fuelAmount = Mathf.Clamp(fuelAmount, 0, fuelCapacity);

            if (Input.GetKey(jetpackKey) &&
                fuelAmount > 0)
            {
                if (regenerateFuelRoutine != null)
                {
                    StopCoroutine(regenerateFuelRoutine);
                    regenerateFuelRoutine = null;
                }

                isJetpacking = true;
                fuelAmount -= fuelConsumptionPerSec * Time.deltaTime;
            }

            if (((Input.GetKeyUp(jetpackKey) && isJetpacking) || fuelAmount <= 0) &&
                regenerateFuelRoutine == null)
            {
                isJetpacking = false;
                regenerateFuelRoutine = StartCoroutine(RegenerateFuel());
            }   
        }

        private IEnumerator RegenerateFuel()
        {
            yield return regenerateWaitForSeconds;

            while (fuelAmount < fuelCapacity)
            {
                fuelAmount += fuelRegenPerSec * Time.deltaTime * fuelRegenMultiplier;
                yield return null;
            }

             regenerateFuelRoutine = null;
        }

        private IEnumerator JetpackCooldownCoroutine()
        {
            yield return waitForSecondsJetpackCooldown;

            jetpackIsOnCooldown = false;
        }

        private void FixedUpdate()
        {
            if (isJetpacking && !jetpackIsOnCooldown)
            {
                Debug.Log("I am jetpacking");
                Motor.AddImpulse(Motor.transform.up * upwardForce, 0.5f);
                Motor.SetVelocity(Vector3.ClampMagnitude(Motor.Velocity, maxSpeed));
                jetpackIsOnCooldown = true;
                jetpackCooldownRoutine = StartCoroutine(JetpackCooldownCoroutine());
            }
        }

        public override void Awake()
        {
            base.Awake();
            regenerateWaitForSeconds = new WaitForSeconds(fuelRegenDelaySec);
            waitForSecondsJetpackCooldown = new WaitForSeconds(2f);
        }
    }
}