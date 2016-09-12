using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollSnapMap : MonoBehaviour 
{
    public RectTransform m_panel;
    public Button[] m_btn;
    public RectTransform m_center;
    public int btnLenght;
    public Vector2 m_vHighLightScale;
    public Vector2 m_vDefaultScale;

    public float[] m_fDistance;
    public float[] m_fDistReposition;
    private bool m_bDragging = false;
    private int m_nBtnDistance;
    private int m_nMinButtonNum;

    void Start()
    {
        btnLenght = m_btn.Length;
        m_fDistance = new float[btnLenght];
        m_fDistReposition = new float[btnLenght];

        m_nBtnDistance = (int)Mathf.Abs(m_btn[1].GetComponent<RectTransform>().anchoredPosition.x - m_btn[0].GetComponent<RectTransform>().anchoredPosition.x);

        for(int i = 0; i < m_btn.Length; i++)
        {
            m_vDefaultScale = new Vector2(1, 1);
        }       
    }

    void Update()
    {
        for(int i = 0; i < m_btn.Length; i++)
        {
            m_fDistReposition[i] = m_center.GetComponent<RectTransform>().position.x - m_btn[i].GetComponent<RectTransform>().position.x;
            m_fDistance[i] = Mathf.Abs(m_fDistReposition[i]);

            if(m_fDistReposition[i] > 400)
            {
                float curX = m_btn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = m_btn[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (btnLenght * m_nBtnDistance), curY);
                m_btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
            }
            if (m_fDistReposition[i] < -400)
            {
                float curX = m_btn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = m_btn[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (btnLenght * m_nBtnDistance), curY);
                m_btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
            }
            if(m_fDistReposition[i] <= 1 && m_fDistReposition[i] >= -1)
            {
                //m_btn[i]GetComponent<RectTransform>().localScale = Vector2.Lerp(m_btn[i]GetComponent<RectTransform>().localScale , )
            }
        }
        float fMinDistance = Mathf.Min(m_fDistance); // Get the min distance

        for(int i = 0; i< m_btn.Length; i++)
        {
            if(fMinDistance == m_fDistance[i])
            {
                m_nMinButtonNum = i;
            }
        }

        if(!m_bDragging)
        {
            //LerpToBtn(m_nMinButtonNum * -m_nBtnDistance);
            LerpToBtn(-m_btn[m_nMinButtonNum].GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    void LerpToBtn(float _position)
    {
        float newX = Mathf.Lerp(m_panel.anchoredPosition.x, _position, Time.deltaTime * 5.0f);
        Vector2 newPosition = new Vector2(newX, m_panel.anchoredPosition.y);

        m_panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        m_bDragging = true;
    }

    public void EndDrag()
    {
        m_bDragging = false;
    }
}
