using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionZone : MonoBehaviour
{
    public Interactable interactable;   //같은 오브젝트 또는 자식
    public GameObject promptUI;         //"E 키" 안내 패널

    bool _canInteract;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
            if (promptUI) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
            if(promptUI) promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (_canInteract && Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            interactable.Interact();
        }
    }
}
