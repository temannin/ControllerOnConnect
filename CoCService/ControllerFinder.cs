using SharpDX.DirectInput;

namespace CoCService;

public class ControllerFinder
{
    private readonly DirectInput _input = new();
    public bool ControllerIsConnected => _input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly).Count > 0;
}