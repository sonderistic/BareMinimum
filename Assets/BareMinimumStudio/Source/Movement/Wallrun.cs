using BMS.Core;
using UnityEngine;

public class Wallrun : Module
{
    [SerializeField]
    private float wallrunSpeed = 15f;
    [SerializeField]
    private float wallrunJumpForce = 20;
    
    private Camera camera;
    private bool canWallRun;
    private bool isWallRunning;
    private bool isWallJump;
    private Vector3 wallrunDir;
    private LineRenderer line;

    public override void Awake()
    {
        base.Awake();
        camera = Camera.main;
        line = gameObject.AddComponent<LineRenderer>();
        line.enabled = false;
    }

    private void Update()
    {
        canWallRun = false;

        // If we walljumped, we don't want to allow wallrunning until we are off the wall
        if (isWallJump && Motor.WallSurface != null)
        {
            return;
        }

        isWallRunning = false;
        isWallJump = false;

        if (Motor.WallSurface != null && Motor.Airtime > 0)
        {
            wallrunDir = Vector3.ProjectOnPlane(camera.transform.forward, Motor.WallSurface.Normal);

            if (Vector3.Dot(wallrunDir, camera.transform.forward) > 0.7f)
            {
                canWallRun = true;
            }
        }

        if (canWallRun && Input.GetKey(KeyCode.LeftShift))
        {
            isWallRunning = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isWallRunning)
        {
            return;
        }
        
        if (isWallJump)
        {
            Vector3 jumpDir = Quaternion.AngleAxis(45f, Vector3.Cross(Vector3.up, Motor.WallSurface.Normal).normalized) * Vector3.up;
            float jumpforceMultiplier = Vector3.Dot(camera.transform.forward, wallrunDir);
            Vector3 jumpVelocity = Vector3.Lerp(jumpDir * wallrunJumpForce, 
                camera.transform.forward * wallrunSpeed * jumpforceMultiplier, 0.5f);
            Motor.SetVelocity(jumpVelocity);
            //line.SetPositions(new Vector3[] { transform.position, transform.position + jumpVelocity });
        }
        else
        {
            Motor.SetVelocity(wallrunDir * wallrunSpeed);
        }
    }
}
