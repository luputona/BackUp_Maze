using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Yielders 
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    static Dictionary<float, WaitForSeconds> m_timeInterval = new Dictionary<float, WaitForSeconds>(100, new FloatComparer());

    static WaitForEndOfFrame m_endFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame EndOfFrame
    {
        get
        {
            return m_endFrame;
        }
    }

    static WaitForFixedUpdate m_fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate
    {
        get
        {
            return m_fixedUpdate;
        }
    }

    public static WaitForSeconds Get(float second)
    {
        WaitForSeconds waitforsecond;
        if(!m_timeInterval.TryGetValue(second,out waitforsecond))
        {
            m_timeInterval.Add(second, waitforsecond = new WaitForSeconds(second));            
        }
        return waitforsecond;
    }
}
