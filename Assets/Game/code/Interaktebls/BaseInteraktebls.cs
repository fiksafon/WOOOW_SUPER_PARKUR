using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseInteraktebls : MonoBehaviour, IInteraktbl
{
    [SerializeField] protected BoxCollider2D _BoxCollider;
    protected bool _isInteracting = false;

    
    public void Interact(GameObject interactor)
    {
        OnInterakt();

    }
    public bool CanInteract(GameObject interactor)
    {
        return _isInteracting;
    }

    protected  virtual void OnInterakt()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
