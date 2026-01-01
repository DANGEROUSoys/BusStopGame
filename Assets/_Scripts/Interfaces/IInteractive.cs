using UnityEngine;

public interface IInteractive : IRaycastHittable
{
    public void Interact();
    public void StopInteract();
}
