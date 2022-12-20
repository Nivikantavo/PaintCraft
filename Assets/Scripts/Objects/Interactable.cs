using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Painter Painter;

    public void Select(Painter painter)
    {
        Painter = painter;

        Interact();
    }

    public void Deselect()
    {
        StopInteract();
    }

    protected virtual void Interact()
    {
        Debug.Log($"Interact with {gameObject.name}");
    }

    protected virtual void StopInteract()
    {
        Debug.Log($"Stop interact with {gameObject.name}");
    }
}
