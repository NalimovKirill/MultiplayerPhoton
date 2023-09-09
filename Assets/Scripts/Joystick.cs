using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    [SerializeField] private GameObject _joystick;
    [SerializeField] private GameObject _joystickBackGround;

    private Vector2 _joystickVec;
    public Vector2 JoystikVec { get { return _joystickVec; } }
    private Vector2 _joystickVecLastPosition;
    public Vector2 JoystickVecLastPosition { get { return _joystickVecLastPosition; } }

    private Vector2 _joystickTouchPos;
    private Vector2 _joystickOriginalPos;
    private float _joyStickRadius;


    private void Start()
    {
        _joystickOriginalPos = _joystickBackGround.transform.position;
        _joyStickRadius = _joystickBackGround.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void PointerDown()
    {
        _joystick.transform.position = Input.mousePosition;
        _joystickTouchPos = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        _joystickVecLastPosition = (dragPos - _joystickTouchPos).normalized;
        _joystickVec = (dragPos - _joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, _joystickTouchPos);

        if (joystickDist < _joyStickRadius)
        {
            _joystick.transform.position = _joystickTouchPos + _joystickVec * joystickDist;
        }
        else
        {
            _joystick.transform.position = _joystickTouchPos + _joystickVec * _joyStickRadius;
        }
    }

    public void PointerUp()
    {
        _joystickVec = Vector2.zero;
        _joystick.transform.position = _joystickOriginalPos;
        _joystickBackGround.transform.position = _joystickOriginalPos;
    }
}
