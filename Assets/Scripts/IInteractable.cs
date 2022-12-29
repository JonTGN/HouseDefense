using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();

    string GetDescription();
    
    Items GetType();
}

public interface IInteractableBase
{
    void Interact();
}
