using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float _maxInteractionRadius;
    [SerializeField] LayerMask _interactableLayer;

    Interactable _focusedInteractable;

    private void Update()
    {
        CheckRadiusForInteractables();
    }

    void CheckRadiusForInteractables()
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
                    _focusedInteractable?.ExitInteractionRange();
                    interactable.EnterInteractionRange();
                    _focusedInteractable = interactable;

                    break;
                }
            }
        }

        if (_focusedInteractable == null)
            return;

        // Out of range from current focused interactable
        if (Vector2.Distance(_focusedInteractable.transform.position, transform.position) > _focusedInteractable.InteractionRadius)
        {
            _focusedInteractable.ExitInteractionRange();
            _focusedInteractable = null;
        }


    }

    void Interact()
	{
        if (_focusedInteractable == null)
            return;

        _focusedInteractable.Interact();
    }
}