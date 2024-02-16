using FishNet.Object;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        if (Owner.IsLocalClient)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            
        }
    }
}
