using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [TextArea] 
    public string prompt = "Press E to interact";
    public abstract void Interact();
}
