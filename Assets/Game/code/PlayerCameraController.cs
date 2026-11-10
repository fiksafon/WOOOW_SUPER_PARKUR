using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
     [SerializeField] private Vector3 _offset = new Vector3(0f, 1f, -10f);
    private EventBus _inet;
    private Vector3 _TargetPosition;
    public void Initialize(EventBus bus)
    {
        _inet = bus;
        _TargetPosition = _offset;
    }

    public void GetPlayerPosition(Vector3 playerPosition)
    {
        _TargetPosition = playerPosition + _offset;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _TargetPosition, 0.1f);
    }       

}