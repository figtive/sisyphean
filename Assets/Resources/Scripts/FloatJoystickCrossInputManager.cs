using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FloatJoystickCrossInputManager : MonoBehaviour {
    [SerializeField] private string _horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    [SerializeField] private string _verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
    [SerializeField] private string _pressButtonName = "Fire1"; // The name given to the vertical axis for the cross platform input
    [SerializeField] private bool _useButton;
    [SerializeField] private FloatingJoystick _joystick;

    private CrossPlatformInputManager.VirtualAxis _horizontalVirtualAxis; // Reference to the joystick in the cross platform input
    private CrossPlatformInputManager.VirtualAxis _verticalVirtualAxis; // Reference to the joystick in the cross platform input
    private CrossPlatformInputManager.VirtualButton _pressVirtualButton;
    private bool _useX;
    private bool _useY;
    private bool lastState;

    void OnEnable () {
        CreateVirtualAxes();
    }

    void Update () {
        Vector3 moveVector = (Vector3.right * _joystick.Horizontal + Vector3.up * _joystick.Vertical);
        bool pressed = _joystick.pressed;
        UpdateVirtualInput(moveVector, pressed, lastState);
        lastState = pressed;
    }

    void OnDisable() {
        // remove the joysticks from the cross platform input
        if (_useX) { _horizontalVirtualAxis.Remove(); }
        if (_useY) { _verticalVirtualAxis.Remove(); }
        if (_useButton) { _pressVirtualButton.Remove(); }
    }

    void CreateVirtualAxes() {
        // set axes to use
        _useX = (_joystick.joystickMode == JoystickMode.AllAxis || _joystick.joystickMode == JoystickMode.Horizontal);
        _useY = (_joystick.joystickMode == JoystickMode.AllAxis || _joystick.joystickMode == JoystickMode.Vertical);

        // create new axes based on axes to use
        if (_useX) {
            _horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(_horizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(_horizontalVirtualAxis);
        }
        if (_useY) {
            _verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(_verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(_verticalVirtualAxis);
        }
        if (_useButton) {
            _pressVirtualButton = new CrossPlatformInputManager.VirtualButton(_pressButtonName);
            CrossPlatformInputManager.RegisterVirtualButton(_pressVirtualButton);
        }
    }

    void UpdateVirtualInput(Vector3 value, bool pressed, bool lastState) {
        if (_useX) { _horizontalVirtualAxis.Update(value.x); }
        if (_useY) { _verticalVirtualAxis.Update(value.y); }
        if (_useButton && (pressed != lastState)) { if (pressed) _pressVirtualButton.Pressed(); else _pressVirtualButton.Released(); }
    }
}