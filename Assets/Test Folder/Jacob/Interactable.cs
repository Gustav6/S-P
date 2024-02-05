using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float InteractionRadius;

    public abstract void EnterInteractionRange();

    public abstract void ExitInteractionRange();

    public abstract void Interact();
}
