using UnityEngine;
using LibSM64;
public class OculusInput : SM64InputProvider
{
    public GameObject cameraObject = null;

    public override Vector3 GetCameraLookDirection()
    {
        return cameraObject.transform.forward;
    }

    public override Vector2 GetJoystickAxes()
    {
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
    }

    public override bool GetButtonHeld( Button button )
    {
        switch( button )
        {
            case SM64InputProvider.Button.Jump:  return OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch);
            case SM64InputProvider.Button.Kick:  return OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch);
            case SM64InputProvider.Button.Stomp: return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        }
        return false;
    }
}