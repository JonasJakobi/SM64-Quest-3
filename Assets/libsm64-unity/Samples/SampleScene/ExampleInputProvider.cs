using UnityEngine;
using LibSM64;

public class ExampleInputProvider : SM64InputProvider
{
    public GameObject cameraObject = null;

    public override Vector3 GetCameraLookDirection()
    {
        return cameraObject.transform.forward;
    }

    public override Vector2 GetJoystickAxes()
    {
        return new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") );
    }

    public override bool GetButtonHeld( Button button )
    {
        switch( button )
        {
            case SM64InputProvider.Button.Jump:  return Input.GetButton("Jump");
            case SM64InputProvider.Button.Kick:  return Input.GetButton("Kick");
            case SM64InputProvider.Button.Stomp: return Input.GetButton("Z");
        }
        return false;
    }
}