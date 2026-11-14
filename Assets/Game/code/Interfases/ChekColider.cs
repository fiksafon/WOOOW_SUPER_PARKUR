using UnityEngine;

public class CheckColider : MonoBehaviour
{
    [SerializeField] private Transform _rayTochka;
    [SerializeField] private float _circleRadius = 0.1f;
    [SerializeField] private LayerMask _interactableLayers ;

    public IInteraktbl CurrentInteractable { get; private set; }
    public bool HasInteractable => CurrentInteractable != null;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Vector2 origin = _rayTochka != null ? _rayTochka.position : transform.position;
        Vector2 direction = _rayTochka != null ? _rayTochka.right : transform.right;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, _circleRadius, _interactableLayers);

        if (hits.Length > 0 && hits[0].TryGetComponent(out IInteraktbl interactable))
        {
            CurrentInteractable = interactable;
        }
        else
        {
            CurrentInteractable = null;
        }

        Debug.DrawRay(origin, direction * _circleRadius, HasInteractable ? Color.green : Color.red);
    }
}
    