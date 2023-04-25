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
    }

    protected virtual void StopInteract()
    {
    }
}
