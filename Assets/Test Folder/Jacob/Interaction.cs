using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float _maxInteractionRadius;
    [SerializeField] LayerMask _interactableLayer;

    Interactable _focusedInteractable;

    void Interact()
	{
        // Where the hell did my beautiful OverLapCircleNonAlloc go??? It's supposed to exist. WGHERE DID IT GO????
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, _maxInteractionRadius, _interactableLayer);

        for (int i = 0; i < nearbyColliders.Length; i++)
		{
            if (!nearbyColliders[i].TryGetComponent(out Interactable interactable))
                continue;

            if (interactable.InteractionRadius <= Mathf.Abs((nearbyColliders[i].transform.position - transform.position).magnitude))
            {
                // New focused interactable
                if (interactable != _focusedInteractable)
                {
                    _focusedInteractable.ExitInteractionRange();
                    interactable.EnterInteractionRange();
                    _focusedInteractable = interactable;

                    break;
                }
			}
		}
    }
}

public abstract class Interactable : MonoBehaviour
{
    public float InteractionRadius;

    public abstract void EnterInteractionRange();

    public abstract void ExitInteractionRange();

    public abstract void Interact();
}