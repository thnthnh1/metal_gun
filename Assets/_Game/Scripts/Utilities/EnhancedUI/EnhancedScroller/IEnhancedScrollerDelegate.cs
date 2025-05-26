using System;

namespace EnhancedUI.EnhancedScroller
{
	public interface IEnhancedScrollerDelegate
	{
		int GetNumberOfCells(EnhancedScroller scroller);

		float GetCellViewSize(EnhancedScroller scroller, int dataIndex);

		EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);
	}
}
