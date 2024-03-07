using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float _maxInteractionRadius;
    [SerializeField] LayerMask _interactableLayer;

    [SerializeField] InputAction _interactIA;

    Interactable _focusedInteractable;

    private void Awake()
    {
        _interactIA.performed += ctx => { Interact(); };
    }

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
            // Edge case, only occurs if another object is accidentally set to the Interactable layer
            if (!nearbyColliders[i].TryGetComponent(out Interactable interactable))
                continue;

            if (Mathf.Abs((nearbyColliders[i].transform.position - transform.position).magnitude) <= interactable.InteractionRadius && interactable.enabled)
            {
                // New focused interactable
                if (interactable != _focusedInteractable)
                {
                    if (_focusedInteractable != null)
                        _focusedInteractable.ExitInteractionRange();

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
            Debug.Log("Out of range");
            _focusedInteractable.ExitInteractionRange();
            _focusedInteractable = null;
        }


    }

    void Interact()
	{
        if (_focusedInteractable == null)
            return;

        _focusedInteractable.Interact();

        _focusedInteractable.ExitInteractionRange();
        _focusedInteractable = null;
    }

    public void OnEnable()
    {
        _interactIA.Enable();
    }

    public void OnDisable()
    {
        _interactIA.Disable();
    }
}