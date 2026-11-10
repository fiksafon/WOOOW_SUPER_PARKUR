using UnityEngine;

public class Door : BaseInteraktebls
{
   protected override void OnInterakt()
    {
        Debug.Log("Door interacted with " + gameObject.name);
    }
}
