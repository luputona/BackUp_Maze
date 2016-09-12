using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 자신만의 셀 데이터를 정의하자.
public class ItemCellData : IReuseCellData{

	#region IReuseCellData
	public int m_Index;
	public int Index{
		get{
			return m_Index;
		}
		set{
			m_Index = value;
		}
	}
	#endregion

	// user data
	public string ImgName;
	public string Value;
}