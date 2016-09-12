using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image m_bgImg;
    private Image m_joystickImg;

    public Vector3 InputDirection { set; get; }

    void Start()
    {
        m_bgImg = GetComponent<Image>();
        m_joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos = Vector2.zero;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(m_bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / m_bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / m_bgImg.rectTransform.sizeDelta.y);

            float x = (m_bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (m_bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDirection = new Vector3(x, 0, y);
            InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

            m_joystickImg.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (m_bgImg.rectTransform.sizeDelta.x / 3) , InputDirection.z * (m_bgImg.rectTransform.sizeDelta.y / 3));
            
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        InputDirection = Vector3.zero;
        m_joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    //public float Horizontal()
    //{
    //    if (InputDirection.x != 0)
    //    {
    //        return InputDirection.x;
    //    }
    //    else
    //        return Input.GetAxis("Horizontal");
    //}

    //public float Vertical()
    //{
    //    if (InputDirection.z != 0)
    //    {
    //        return InputDirection.z;
    //    }
    //    else
    //        return Input.GetAxis("Vertical");
    //}
}
