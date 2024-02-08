using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableEntity : MonoBehaviour, IInteractableEntity
{
    protected Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void Push(Vector3 dir, float force, GameObject frog)
    {
        _rb.AddForce(dir * force, ForceMode.VelocityChange);
    }
}

public interface IInteractableEntity
{
    public void Push(Vector3 dir, float force, GameObject frog);
}
