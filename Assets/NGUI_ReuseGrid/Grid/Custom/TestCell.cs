using UnityEngine;
using System.Collections;

// 셀 데이터를 받아서 스크롤뷰셀의 데이터를 갱신한다.
public class TestCell : UIReuseScrollViewCell 
{
	public UILabel label;

	public override void UpdateData (IReuseCellData CellData)
	{
		ItemCellData item = CellData as ItemCellData;
		if( item == null )
			return;

		label.text = string.Format( "{0} {1}",item.ImgName, item.Index );
	}
}
