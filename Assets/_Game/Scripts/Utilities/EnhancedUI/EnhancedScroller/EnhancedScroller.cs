using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EnhancedUI.EnhancedScroller
{
	[RequireComponent(typeof(ScrollRect))]
	public class EnhancedScroller : MonoBehaviour
	{
		public enum ScrollDirectionEnum
		{
			Vertical,
			Horizontal
		}

		public enum CellViewPositionEnum
		{
			Before,
			After
		}

		public enum ScrollbarVisibilityEnum
		{
			OnlyIfNeeded,
			Always,
			Never
		}

		public enum LoopJumpDirectionEnum
		{
			Closest,
			Up,
			Down
		}

		private enum ListPositionEnum
		{
			First,
			Last
		}

		public enum TweenType
		{
			immediate,
			linear,
			spring,
			easeInQuad,
			easeOutQuad,
			easeInOutQuad,
			easeInCubic,
			easeOutCubic,
			easeInOutCubic,
			easeInQuart,
			easeOutQuart,
			easeInOutQuart,
			easeInQuint,
			easeOutQuint,
			easeInOutQuint,
			easeInSine,
			easeOutSine,
			easeInOutSine,
			easeInExpo,
			easeOutExpo,
			easeInOutExpo,
			easeInCirc,
			easeOutCirc,
			easeInOutCirc,
			easeInBounce,
			easeOutBounce,
			easeInOutBounce,
			easeInBack,
			easeOutBack,
			easeInOutBack,
			easeInElastic,
			easeOutElastic,
			easeInOutElastic
		}

		private sealed class _TweenPosition_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal EnhancedScroller.TweenType tweenType;

			internal float time;

			internal float end;

			internal float _newPosition___1;

			internal float start;

			internal Action tweenComplete;

			internal EnhancedScroller _this;

			internal object _current;

			internal bool _disposing;

			internal int _PC;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public _TweenPosition_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					if (this.tweenType == EnhancedScroller.TweenType.immediate || this.time == 0f)
					{
						this._this.ScrollPosition = this.end;
						goto IL_827;
					}
					this._this._scrollRect.velocity = Vector2.zero;
					this._this.IsTweening = true;
					if (this._this.scrollerTweeningChanged != null)
					{
						this._this.scrollerTweeningChanged(this._this, true);
					}
					this._this._tweenTimeLeft = 0f;
					this._newPosition___1 = 0f;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				if (this._this._tweenTimeLeft < this.time)
				{
					switch (this.tweenType)
					{
					case EnhancedScroller.TweenType.linear:
						this._newPosition___1 = this._this.linear(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.spring:
						this._newPosition___1 = EnhancedScroller.spring(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInQuad:
						this._newPosition___1 = EnhancedScroller.easeInQuad(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutQuad:
						this._newPosition___1 = EnhancedScroller.easeOutQuad(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutQuad:
						this._newPosition___1 = EnhancedScroller.easeInOutQuad(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInCubic:
						this._newPosition___1 = EnhancedScroller.easeInCubic(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutCubic:
						this._newPosition___1 = EnhancedScroller.easeOutCubic(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutCubic:
						this._newPosition___1 = EnhancedScroller.easeInOutCubic(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInQuart:
						this._newPosition___1 = EnhancedScroller.easeInQuart(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutQuart:
						this._newPosition___1 = EnhancedScroller.easeOutQuart(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutQuart:
						this._newPosition___1 = EnhancedScroller.easeInOutQuart(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInQuint:
						this._newPosition___1 = EnhancedScroller.easeInQuint(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutQuint:
						this._newPosition___1 = EnhancedScroller.easeOutQuint(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutQuint:
						this._newPosition___1 = EnhancedScroller.easeInOutQuint(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInSine:
						this._newPosition___1 = EnhancedScroller.easeInSine(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutSine:
						this._newPosition___1 = EnhancedScroller.easeOutSine(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutSine:
						this._newPosition___1 = EnhancedScroller.easeInOutSine(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInExpo:
						this._newPosition___1 = EnhancedScroller.easeInExpo(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutExpo:
						this._newPosition___1 = EnhancedScroller.easeOutExpo(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutExpo:
						this._newPosition___1 = EnhancedScroller.easeInOutExpo(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInCirc:
						this._newPosition___1 = EnhancedScroller.easeInCirc(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutCirc:
						this._newPosition___1 = EnhancedScroller.easeOutCirc(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutCirc:
						this._newPosition___1 = EnhancedScroller.easeInOutCirc(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInBounce:
						this._newPosition___1 = EnhancedScroller.easeInBounce(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutBounce:
						this._newPosition___1 = EnhancedScroller.easeOutBounce(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutBounce:
						this._newPosition___1 = EnhancedScroller.easeInOutBounce(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInBack:
						this._newPosition___1 = EnhancedScroller.easeInBack(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutBack:
						this._newPosition___1 = EnhancedScroller.easeOutBack(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutBack:
						this._newPosition___1 = EnhancedScroller.easeInOutBack(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInElastic:
						this._newPosition___1 = EnhancedScroller.easeInElastic(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeOutElastic:
						this._newPosition___1 = EnhancedScroller.easeOutElastic(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					case EnhancedScroller.TweenType.easeInOutElastic:
						this._newPosition___1 = EnhancedScroller.easeInOutElastic(this.start, this.end, this._this._tweenTimeLeft / this.time);
						break;
					}
					if (this._this.loop)
					{
						if (this.end > this.start && this._newPosition___1 > this._this._loopLastJumpTrigger)
						{
							this._newPosition___1 = this._this._loopFirstScrollPosition + (this._newPosition___1 - this._this._loopLastJumpTrigger);
						}
						else if (this.start > this.end && this._newPosition___1 < this._this._loopFirstJumpTrigger)
						{
							this._newPosition___1 = this._this._loopLastScrollPosition - (this._this._loopFirstJumpTrigger - this._newPosition___1);
						}
					}
					this._this.ScrollPosition = this._newPosition___1;
					this._this._tweenTimeLeft += Time.unscaledDeltaTime;
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				this._this.ScrollPosition = this.end;
				IL_827:
				if (this.tweenComplete != null)
				{
					this.tweenComplete();
				}
				this._this.IsTweening = false;
				if (this._this.scrollerTweeningChanged != null)
				{
					this._this.scrollerTweeningChanged(this._this, false);
				}
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._disposing = true;
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		public EnhancedScroller.ScrollDirectionEnum scrollDirection;

		public float spacing;

		public RectOffset padding;

		[SerializeField]
		private bool loop;

		[SerializeField]
		private EnhancedScroller.ScrollbarVisibilityEnum scrollbarVisibility;

		public bool snapping;

		public float snapVelocityThreshold;

		public float snapWatchOffset;

		public float snapJumpToOffset;

		public float snapCellCenterOffset;

		public bool snapUseCellSpacing;

		public EnhancedScroller.TweenType snapTweenType;

		public float snapTweenTime;

		public CellViewVisibilityChangedDelegate cellViewVisibilityChanged;

		public CellViewWillRecycleDelegate cellViewWillRecycle;

		public ScrollerScrolledDelegate scrollerScrolled;

		public ScrollerSnappedDelegate scrollerSnapped;

		public ScrollerScrollingChangedDelegate scrollerScrollingChanged;

		public ScrollerTweeningChangedDelegate scrollerTweeningChanged;

		private bool _IsScrolling_k__BackingField;

		private bool _IsTweening_k__BackingField;

		private ScrollRect _scrollRect;

		private RectTransform _scrollRectTransform;

		private Scrollbar _scrollbar;

		private RectTransform _container;

		private HorizontalOrVerticalLayoutGroup _layoutGroup;

		private IEnhancedScrollerDelegate _delegate;

		private bool _reloadData;

		private bool _refreshActive;

		private SmallList<EnhancedScrollerCellView> _recycledCellViews = new SmallList<EnhancedScrollerCellView>();

		private LayoutElement _firstPadder;

		private LayoutElement _lastPadder;

		private RectTransform _recycledCellViewContainer;

		private SmallList<float> _cellViewSizeArray = new SmallList<float>();

		private SmallList<float> _cellViewOffsetArray = new SmallList<float>();

		private float _scrollPosition;

		private SmallList<EnhancedScrollerCellView> _activeCellViews = new SmallList<EnhancedScrollerCellView>();

		private int _activeCellViewsStartIndex;

		private int _activeCellViewsEndIndex;

		private int _loopFirstCellIndex;

		private int _loopLastCellIndex;

		private float _loopFirstScrollPosition;

		private float _loopLastScrollPosition;

		private float _loopFirstJumpTrigger;

		private float _loopLastJumpTrigger;

		private float _lastScrollRectSize;

		private bool _lastLoop;

		private int _snapCellViewIndex;

		private int _snapDataIndex;

		private bool _snapJumping;

		private bool _snapInertia;

		private EnhancedScroller.ScrollbarVisibilityEnum _lastScrollbarVisibility;

		private float _tweenTimeLeft;

		public IEnhancedScrollerDelegate Delegate
		{
			get
			{
				return this._delegate;
			}
			set
			{
				this._delegate = value;
				this._reloadData = true;
			}
		}

		public float ScrollPosition
		{
			get
			{
				return this._scrollPosition;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, this.GetScrollPositionForCellViewIndex(this._cellViewSizeArray.Count - 1, EnhancedScroller.CellViewPositionEnum.Before));
				if (this._scrollPosition != value)
				{
					this._scrollPosition = value;
					if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
					{
						this._scrollRect.verticalNormalizedPosition = 1f - this._scrollPosition / this.ScrollSize;
					}
					else
					{
						this._scrollRect.horizontalNormalizedPosition = this._scrollPosition / this.ScrollSize;
					}
					this._refreshActive = true;
				}
			}
		}

		public float ScrollSize
		{
			get
			{
				if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
				{
					return this._container.rect.height - this._scrollRectTransform.rect.height;
				}
				return this._container.rect.width - this._scrollRectTransform.rect.width;
			}
		}

		public float NormalizedScrollPosition
		{
			get
			{
				return this._scrollPosition / this.ScrollSize;
			}
		}

		public bool Loop
		{
			get
			{
				return this.loop;
			}
			set
			{
				if (this.loop != value)
				{
					float scrollPosition = this._scrollPosition;
					this.loop = value;
					this._Resize(false);
					if (this.loop)
					{
						this.ScrollPosition = this._loopFirstScrollPosition + scrollPosition;
					}
					else
					{
						this.ScrollPosition = scrollPosition - this._loopFirstScrollPosition;
					}
					this.ScrollbarVisibility = this.scrollbarVisibility;
				}
			}
		}

		public EnhancedScroller.ScrollbarVisibilityEnum ScrollbarVisibility
		{
			get
			{
				return this.scrollbarVisibility;
			}
			set
			{
				this.scrollbarVisibility = value;
				if (this._scrollbar != null && this._cellViewOffsetArray != null && this._cellViewOffsetArray.Count > 0)
				{
					if (this._cellViewOffsetArray.Last() < this.ScrollRectSize || this.loop)
					{
						this._scrollbar.gameObject.SetActive(this.scrollbarVisibility == EnhancedScroller.ScrollbarVisibilityEnum.Always);
					}
					else
					{
						this._scrollbar.gameObject.SetActive(this.scrollbarVisibility != EnhancedScroller.ScrollbarVisibilityEnum.Never);
					}
				}
			}
		}

		public Vector2 Velocity
		{
			get
			{
				return this._scrollRect.velocity;
			}
			set
			{
				this._scrollRect.velocity = value;
			}
		}

		public float LinearVelocity
		{
			get
			{
				return (this.scrollDirection != EnhancedScroller.ScrollDirectionEnum.Vertical) ? this._scrollRect.velocity.x : this._scrollRect.velocity.y;
			}
			set
			{
				if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
				{
					this._scrollRect.velocity = new Vector2(0f, value);
				}
				else
				{
					this._scrollRect.velocity = new Vector2(value, 0f);
				}
			}
		}

		public bool IsScrolling
		{
			get;
			private set;
		}

		public bool IsTweening
		{
			get;
			private set;
		}

		public int StartCellViewIndex
		{
			get
			{
				return this._activeCellViewsStartIndex;
			}
		}

		public int EndCellViewIndex
		{
			get
			{
				return this._activeCellViewsEndIndex;
			}
		}

		public int StartDataIndex
		{
			get
			{
				return this._activeCellViewsStartIndex % this.NumberOfCells;
			}
		}

		public int EndDataIndex
		{
			get
			{
				return this._activeCellViewsEndIndex % this.NumberOfCells;
			}
		}

		public int NumberOfCells
		{
			get
			{
				return (this._delegate == null) ? 0 : this._delegate.GetNumberOfCells(this);
			}
		}

		public ScrollRect ScrollRect
		{
			get
			{
				return this._scrollRect;
			}
		}

		public float ScrollRectSize
		{
			get
			{
				if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
				{
					return this._scrollRectTransform.rect.height;
				}
				return this._scrollRectTransform.rect.width;
			}
		}

		public EnhancedScrollerCellView GetCellView(EnhancedScrollerCellView cellPrefab)
		{
			EnhancedScrollerCellView enhancedScrollerCellView = this._GetRecycledCellView(cellPrefab);
			if (enhancedScrollerCellView == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cellPrefab.gameObject);
				enhancedScrollerCellView = gameObject.GetComponent<EnhancedScrollerCellView>();
				enhancedScrollerCellView.transform.SetParent(this._container);
				enhancedScrollerCellView.transform.localPosition = Vector3.zero;
				enhancedScrollerCellView.transform.localRotation = Quaternion.identity;
			}
			return enhancedScrollerCellView;
		}

		public void ReloadData(float scrollPositionFactor = 0f)
		{
			this._reloadData = false;
			this._RecycleAllCells();
			if (this._delegate != null)
			{
				this._Resize(false);
			}
			if (this._scrollRect == null || this._scrollRectTransform == null || this._container == null)
			{
				this._scrollPosition = 0f;
				return;
			}
			this._scrollPosition = scrollPositionFactor * this.ScrollSize;
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				this._scrollRect.verticalNormalizedPosition = 1f - scrollPositionFactor;
			}
			else
			{
				this._scrollRect.horizontalNormalizedPosition = scrollPositionFactor;
			}
		}

		public void RefreshActiveCellViews()
		{
			for (int i = 0; i < this._activeCellViews.Count; i++)
			{
				this._activeCellViews[i].RefreshCellView();
			}
		}

		public void ClearAll()
		{
			this.ClearActive();
			this.ClearRecycled();
		}

		public void ClearActive()
		{
			for (int i = 0; i < this._activeCellViews.Count; i++)
			{
				UnityEngine.Object.DestroyImmediate(this._activeCellViews[i].gameObject);
			}
			this._activeCellViews.Clear();
		}

		public void ClearRecycled()
		{
			for (int i = 0; i < this._recycledCellViews.Count; i++)
			{
				UnityEngine.Object.DestroyImmediate(this._recycledCellViews[i].gameObject);
			}
			this._recycledCellViews.Clear();
		}

		public void ToggleLoop()
		{
			this.Loop = !this.loop;
		}

		public void JumpToDataIndex(int dataIndex, float scrollerOffset = 0f, float cellOffset = 0f, bool useSpacing = true, EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.immediate, float tweenTime = 0f, Action jumpComplete = null, EnhancedScroller.LoopJumpDirectionEnum loopJumpDirection = EnhancedScroller.LoopJumpDirectionEnum.Closest)
		{
			if (this.StartDataIndex == 0 && this.EndDataIndex == this.NumberOfCells - 1)
			{
				if (jumpComplete != null)
				{
					jumpComplete();
				}
				return;
			}
			float num = 0f;
			if (cellOffset != 0f)
			{
				float num2 = (this._delegate == null) ? 0f : this._delegate.GetCellViewSize(this, dataIndex);
				if (useSpacing)
				{
					num2 += this.spacing;
					if (dataIndex > 0 && dataIndex < this.NumberOfCells - 1)
					{
						num2 += this.spacing;
					}
				}
				num = num2 * cellOffset;
			}
			float num3 = 0f;
			float num4 = -(scrollerOffset * this.ScrollRectSize) + num;
			if (this.loop)
			{
				float num5 = this.GetScrollPositionForCellViewIndex(dataIndex, EnhancedScroller.CellViewPositionEnum.Before) + num4;
				float num6 = this.GetScrollPositionForCellViewIndex(dataIndex + this.NumberOfCells, EnhancedScroller.CellViewPositionEnum.Before) + num4;
				float num7 = this.GetScrollPositionForCellViewIndex(dataIndex + this.NumberOfCells * 2, EnhancedScroller.CellViewPositionEnum.Before) + num4;
				float num8 = Mathf.Abs(this._scrollPosition - num5);
				float num9 = Mathf.Abs(this._scrollPosition - num6);
				float num10 = Mathf.Abs(this._scrollPosition - num7);
				if (loopJumpDirection != EnhancedScroller.LoopJumpDirectionEnum.Closest)
				{
					if (loopJumpDirection != EnhancedScroller.LoopJumpDirectionEnum.Up)
					{
						if (loopJumpDirection == EnhancedScroller.LoopJumpDirectionEnum.Down)
						{
							num3 = num7;
						}
					}
					else
					{
						num3 = num5;
					}
				}
				else if (num8 < num9)
				{
					if (num8 < num10)
					{
						num3 = num5;
					}
					else
					{
						num3 = num7;
					}
				}
				else if (num9 < num10)
				{
					num3 = num6;
				}
				else
				{
					num3 = num7;
				}
			}
			else
			{
				num3 = this.GetScrollPositionForDataIndex(dataIndex, EnhancedScroller.CellViewPositionEnum.Before) + num4;
			}
			num3 = Mathf.Clamp(num3, 0f, this.GetScrollPositionForCellViewIndex(this._cellViewSizeArray.Count - 1, EnhancedScroller.CellViewPositionEnum.Before));
			if (useSpacing)
			{
				num3 = Mathf.Clamp(num3 - this.spacing, 0f, this.GetScrollPositionForCellViewIndex(this._cellViewSizeArray.Count - 1, EnhancedScroller.CellViewPositionEnum.Before));
			}
			base.StartCoroutine(this.TweenPosition(tweenType, tweenTime, this.ScrollPosition, num3, jumpComplete));
		}

		public void Snap()
		{
			if (this.NumberOfCells == 0)
			{
				return;
			}
			this._snapJumping = true;
			this.LinearVelocity = 0f;
			this._snapInertia = this._scrollRect.inertia;
			this._scrollRect.inertia = false;
			float position = this.ScrollPosition + this.ScrollRectSize * Mathf.Clamp01(this.snapWatchOffset);
			this._snapCellViewIndex = this.GetCellViewIndexAtPosition(position);
			this._snapDataIndex = this._snapCellViewIndex % this.NumberOfCells;
			this.JumpToDataIndex(this._snapDataIndex, this.snapJumpToOffset, this.snapCellCenterOffset, this.snapUseCellSpacing, this.snapTweenType, this.snapTweenTime, new Action(this.SnapJumpComplete), EnhancedScroller.LoopJumpDirectionEnum.Closest);
		}

		public float GetScrollPositionForCellViewIndex(int cellViewIndex, EnhancedScroller.CellViewPositionEnum insertPosition)
		{
			if (this.NumberOfCells == 0)
			{
				return 0f;
			}
			if (cellViewIndex == 0 && insertPosition == EnhancedScroller.CellViewPositionEnum.Before)
			{
				return 0f;
			}
			if (cellViewIndex >= this._cellViewOffsetArray.Count)
			{
				return this._cellViewOffsetArray[this._cellViewOffsetArray.Count - 2];
			}
			if (insertPosition == EnhancedScroller.CellViewPositionEnum.Before)
			{
				return this._cellViewOffsetArray[cellViewIndex - 1] + this.spacing + (float)((this.scrollDirection != EnhancedScroller.ScrollDirectionEnum.Vertical) ? this.padding.left : this.padding.top);
			}
			return this._cellViewOffsetArray[cellViewIndex] + (float)((this.scrollDirection != EnhancedScroller.ScrollDirectionEnum.Vertical) ? this.padding.left : this.padding.top);
		}

		public float GetScrollPositionForDataIndex(int dataIndex, EnhancedScroller.CellViewPositionEnum insertPosition)
		{
			return this.GetScrollPositionForCellViewIndex((!this.loop) ? dataIndex : (this._delegate.GetNumberOfCells(this) + dataIndex), insertPosition);
		}

		public int GetCellViewIndexAtPosition(float position)
		{
			return this._GetCellIndexAtPosition(position, 0, this._cellViewOffsetArray.Count - 1);
		}

		private void _Resize(bool keepPosition)
		{
			float scrollPosition = this._scrollPosition;
			this._cellViewSizeArray.Clear();
			float num = this._AddCellViewSizes();
			if (this.loop)
			{
				if (num < this.ScrollRectSize)
				{
					int numberOfTimes = Mathf.CeilToInt(this.ScrollRectSize / num);
					this._DuplicateCellViewSizes(numberOfTimes, this._cellViewSizeArray.Count);
				}
				this._loopFirstCellIndex = this._cellViewSizeArray.Count;
				this._loopLastCellIndex = this._loopFirstCellIndex + this._cellViewSizeArray.Count - 1;
				this._DuplicateCellViewSizes(2, this._cellViewSizeArray.Count);
			}
			this._CalculateCellViewOffsets();
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				this._container.sizeDelta = new Vector2(this._container.sizeDelta.x, this._cellViewOffsetArray.Last() + (float)this.padding.top + (float)this.padding.bottom);
			}
			else
			{
				this._container.sizeDelta = new Vector2(this._cellViewOffsetArray.Last() + (float)this.padding.left + (float)this.padding.right, this._container.sizeDelta.y);
			}
			if (this.loop)
			{
				this._loopFirstScrollPosition = this.GetScrollPositionForCellViewIndex(this._loopFirstCellIndex, EnhancedScroller.CellViewPositionEnum.Before) + this.spacing * 0.5f;
				this._loopLastScrollPosition = this.GetScrollPositionForCellViewIndex(this._loopLastCellIndex, EnhancedScroller.CellViewPositionEnum.After) - this.ScrollRectSize + this.spacing * 0.5f;
				this._loopFirstJumpTrigger = this._loopFirstScrollPosition - this.ScrollRectSize;
				this._loopLastJumpTrigger = this._loopLastScrollPosition + this.ScrollRectSize;
			}
			this._ResetVisibleCellViews();
			if (keepPosition)
			{
				this.ScrollPosition = scrollPosition;
			}
			else if (this.loop)
			{
				this.ScrollPosition = this._loopFirstScrollPosition;
			}
			else
			{
				this.ScrollPosition = 0f;
			}
			this.ScrollbarVisibility = this.scrollbarVisibility;
		}

		private float _AddCellViewSizes()
		{
			float num = 0f;
			for (int i = 0; i < this.NumberOfCells; i++)
			{
				this._cellViewSizeArray.Add(this._delegate.GetCellViewSize(this, i) + ((i != 0) ? this._layoutGroup.spacing : 0f));
				num += this._cellViewSizeArray[this._cellViewSizeArray.Count - 1];
			}
			return num;
		}

		private void _DuplicateCellViewSizes(int numberOfTimes, int cellCount)
		{
			for (int i = 0; i < numberOfTimes; i++)
			{
				for (int j = 0; j < cellCount; j++)
				{
					this._cellViewSizeArray.Add(this._cellViewSizeArray[j] + ((j != 0) ? 0f : this._layoutGroup.spacing));
				}
			}
		}

		private void _CalculateCellViewOffsets()
		{
			this._cellViewOffsetArray.Clear();
			float num = 0f;
			for (int i = 0; i < this._cellViewSizeArray.Count; i++)
			{
				num += this._cellViewSizeArray[i];
				this._cellViewOffsetArray.Add(num);
			}
		}

		private EnhancedScrollerCellView _GetRecycledCellView(EnhancedScrollerCellView cellPrefab)
		{
			for (int i = 0; i < this._recycledCellViews.Count; i++)
			{
				if (this._recycledCellViews[i].cellIdentifier == cellPrefab.cellIdentifier)
				{
					return this._recycledCellViews.RemoveAt(i);
				}
			}
			return null;
		}

		private void _ResetVisibleCellViews()
		{
			int num;
			int num2;
			this._CalculateCurrentActiveCellRange(out num, out num2);
			int i = 0;
			SmallList<int> smallList = new SmallList<int>();
			while (i < this._activeCellViews.Count)
			{
				if (this._activeCellViews[i].cellIndex < num || this._activeCellViews[i].cellIndex > num2)
				{
					this._RecycleCell(this._activeCellViews[i]);
				}
				else
				{
					smallList.Add(this._activeCellViews[i].cellIndex);
					i++;
				}
			}
			if (smallList.Count == 0)
			{
				for (i = num; i <= num2; i++)
				{
					this._AddCellView(i, EnhancedScroller.ListPositionEnum.Last);
				}
			}
			else
			{
				for (i = num2; i >= num; i--)
				{
					if (i < smallList.First())
					{
						this._AddCellView(i, EnhancedScroller.ListPositionEnum.First);
					}
				}
				for (i = num; i <= num2; i++)
				{
					if (i > smallList.Last())
					{
						this._AddCellView(i, EnhancedScroller.ListPositionEnum.Last);
					}
				}
			}
			this._activeCellViewsStartIndex = num;
			this._activeCellViewsEndIndex = num2;
			this._SetPadders();
		}

		private void _RecycleAllCells()
		{
			while (this._activeCellViews.Count > 0)
			{
				this._RecycleCell(this._activeCellViews[0]);
			}
			this._activeCellViewsStartIndex = 0;
			this._activeCellViewsEndIndex = 0;
		}

		private void _RecycleCell(EnhancedScrollerCellView cellView)
		{
			if (this.cellViewWillRecycle != null)
			{
				this.cellViewWillRecycle(cellView);
			}
			this._activeCellViews.Remove(cellView);
			this._recycledCellViews.Add(cellView);
			cellView.transform.SetParent(this._recycledCellViewContainer);
			cellView.dataIndex = 0;
			cellView.cellIndex = 0;
			cellView.active = false;
			if (this.cellViewVisibilityChanged != null)
			{
				this.cellViewVisibilityChanged(cellView);
			}
		}

		private void _AddCellView(int cellIndex, EnhancedScroller.ListPositionEnum listPosition)
		{
			if (this.NumberOfCells == 0)
			{
				return;
			}
			int dataIndex = cellIndex % this.NumberOfCells;
			EnhancedScrollerCellView cellView = this._delegate.GetCellView(this, dataIndex, cellIndex);
			cellView.cellIndex = cellIndex;
			cellView.dataIndex = dataIndex;
			cellView.active = true;
			cellView.transform.SetParent(this._container, false);
			cellView.transform.localScale = Vector3.one;
			LayoutElement layoutElement = cellView.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = cellView.gameObject.AddComponent<LayoutElement>();
			}
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				layoutElement.minHeight = this._cellViewSizeArray[cellIndex] - ((cellIndex <= 0) ? 0f : this._layoutGroup.spacing);
			}
			else
			{
				layoutElement.minWidth = this._cellViewSizeArray[cellIndex] - ((cellIndex <= 0) ? 0f : this._layoutGroup.spacing);
			}
			if (listPosition == EnhancedScroller.ListPositionEnum.First)
			{
				this._activeCellViews.AddStart(cellView);
			}
			else
			{
				this._activeCellViews.Add(cellView);
			}
			if (listPosition == EnhancedScroller.ListPositionEnum.Last)
			{
				cellView.transform.SetSiblingIndex(this._container.childCount - 2);
			}
			else if (listPosition == EnhancedScroller.ListPositionEnum.First)
			{
				cellView.transform.SetSiblingIndex(1);
			}
			if (this.cellViewVisibilityChanged != null)
			{
				this.cellViewVisibilityChanged(cellView);
			}
		}

		private void _SetPadders()
		{
			if (this.NumberOfCells == 0)
			{
				return;
			}
			float num = this._cellViewOffsetArray[this._activeCellViewsStartIndex] - this._cellViewSizeArray[this._activeCellViewsStartIndex];
			float num2 = this._cellViewOffsetArray.Last() - this._cellViewOffsetArray[this._activeCellViewsEndIndex];
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				this._firstPadder.minHeight = num;
				this._firstPadder.gameObject.SetActive(this._firstPadder.minHeight > 0f);
				this._lastPadder.minHeight = num2;
				this._lastPadder.gameObject.SetActive(this._lastPadder.minHeight > 0f);
			}
			else
			{
				this._firstPadder.minWidth = num;
				this._firstPadder.gameObject.SetActive(this._firstPadder.minWidth > 0f);
				this._lastPadder.minWidth = num2;
				this._lastPadder.gameObject.SetActive(this._lastPadder.minWidth > 0f);
			}
		}

		private void _RefreshActive()
		{
			this._refreshActive = false;
			Vector2 velocity = Vector2.zero;
			if (this.loop)
			{
				if (this._scrollPosition < this._loopFirstJumpTrigger)
				{
					velocity = this._scrollRect.velocity;
					this.ScrollPosition = this._loopLastScrollPosition - (this._loopFirstJumpTrigger - this._scrollPosition);
					this._scrollRect.velocity = velocity;
				}
				else if (this._scrollPosition > this._loopLastJumpTrigger)
				{
					velocity = this._scrollRect.velocity;
					this.ScrollPosition = this._loopFirstScrollPosition + (this._scrollPosition - this._loopLastJumpTrigger);
					this._scrollRect.velocity = velocity;
				}
			}
			int num;
			int num2;
			this._CalculateCurrentActiveCellRange(out num, out num2);
			if (num == this._activeCellViewsStartIndex && num2 == this._activeCellViewsEndIndex)
			{
				return;
			}
			this._ResetVisibleCellViews();
		}

		private void _CalculateCurrentActiveCellRange(out int startIndex, out int endIndex)
		{
			startIndex = 0;
			endIndex = 0;
			float scrollPosition = this._scrollPosition;
			float position = this._scrollPosition + ((this.scrollDirection != EnhancedScroller.ScrollDirectionEnum.Vertical) ? this._scrollRectTransform.rect.width : this._scrollRectTransform.rect.height);
			startIndex = this.GetCellViewIndexAtPosition(scrollPosition);
			endIndex = this.GetCellViewIndexAtPosition(position);
		}

		private int _GetCellIndexAtPosition(float position, int startIndex, int endIndex)
		{
			if (startIndex >= endIndex)
			{
				return startIndex;
			}
			int num = (startIndex + endIndex) / 2;
			if (this._cellViewOffsetArray[num] + (float)((this.scrollDirection != EnhancedScroller.ScrollDirectionEnum.Vertical) ? this.padding.left : this.padding.top) >= position)
			{
				return this._GetCellIndexAtPosition(position, startIndex, num);
			}
			return this._GetCellIndexAtPosition(position, num + 1, endIndex);
		}

		public void CreateContainer()
		{
			this._scrollRect = base.GetComponent<ScrollRect>();
			this._scrollRectTransform = this._scrollRect.GetComponent<RectTransform>();
			if (this._scrollRect.content != null)
			{
				UnityEngine.Object.DestroyImmediate(this._scrollRect.content.gameObject);
			}
			GameObject gameObject = new GameObject("Container", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(this._scrollRectTransform);
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				gameObject.AddComponent<VerticalLayoutGroup>();
			}
			else
			{
				gameObject.AddComponent<HorizontalLayoutGroup>();
			}
			this._container = gameObject.GetComponent<RectTransform>();
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				this._container.anchorMin = new Vector2(0f, 1f);
				this._container.anchorMax = Vector2.one;
				this._container.pivot = new Vector2(0.5f, 1f);
			}
			else
			{
				this._container.anchorMin = Vector2.zero;
				this._container.anchorMax = new Vector2(0f, 1f);
				this._container.pivot = new Vector2(0f, 0.5f);
			}
			this._container.offsetMax = Vector2.zero;
			this._container.offsetMin = Vector2.zero;
			this._container.localPosition = Vector3.zero;
			this._container.localRotation = Quaternion.identity;
			this._container.localScale = Vector3.one;
			this._scrollRect.content = this._container;
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				this._scrollbar = this._scrollRect.verticalScrollbar;
			}
			else
			{
				this._scrollbar = this._scrollRect.horizontalScrollbar;
			}
			this._layoutGroup = this._container.GetComponent<HorizontalOrVerticalLayoutGroup>();
			this._layoutGroup.spacing = this.spacing;
			this._layoutGroup.padding = this.padding;
			this._layoutGroup.childAlignment = TextAnchor.UpperLeft;
			this._layoutGroup.childForceExpandHeight = true;
			this._layoutGroup.childForceExpandWidth = true;
			this._scrollRect.horizontal = (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal);
			this._scrollRect.vertical = (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical);
			gameObject = new GameObject("First Padder", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(this._container, false);
			this._firstPadder = gameObject.GetComponent<LayoutElement>();
			gameObject = new GameObject("Last Padder", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(this._container, false);
			this._lastPadder = gameObject.GetComponent<LayoutElement>();
			gameObject = new GameObject("Recycled Cells", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(this._scrollRect.transform, false);
			this._recycledCellViewContainer = gameObject.GetComponent<RectTransform>();
			this._recycledCellViewContainer.gameObject.SetActive(false);
			this._lastScrollRectSize = this.ScrollRectSize;
			this._lastLoop = this.loop;
			this._lastScrollbarVisibility = this.scrollbarVisibility;
		}

		private void Update()
		{
			if (this._reloadData)
			{
				this.ReloadData(0f);
			}
			if ((this.loop && this._lastScrollRectSize != this.ScrollRectSize) || this.loop != this._lastLoop)
			{
				this._Resize(true);
				this._lastScrollRectSize = this.ScrollRectSize;
				this._lastLoop = this.loop;
			}
			if (this._lastScrollbarVisibility != this.scrollbarVisibility)
			{
				this.ScrollbarVisibility = this.scrollbarVisibility;
				this._lastScrollbarVisibility = this.scrollbarVisibility;
			}
			if (this.LinearVelocity != 0f && !this.IsScrolling)
			{
				this.IsScrolling = true;
				if (this.scrollerScrollingChanged != null)
				{
					this.scrollerScrollingChanged(this, true);
				}
			}
			else if (this.LinearVelocity == 0f && this.IsScrolling)
			{
				this.IsScrolling = false;
				if (this.scrollerScrollingChanged != null)
				{
					this.scrollerScrollingChanged(this, false);
				}
			}
		}

		private void LateUpdate()
		{
			if (this._refreshActive)
			{
				this._RefreshActive();
			}
		}

		private void OnEnable()
		{
			if (this._scrollRect == null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
			}
			this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this._ScrollRect_OnValueChanged));
		}

		private void OnDisable()
		{
			this._scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this._ScrollRect_OnValueChanged));
		}

		private void _ScrollRect_OnValueChanged(Vector2 val)
		{
			if (this.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				this._scrollPosition = (1f - val.y) * this.ScrollSize;
			}
			else
			{
				this._scrollPosition = val.x * this.ScrollSize;
			}
			this._refreshActive = true;
			if (this.scrollerScrolled != null)
			{
				this.scrollerScrolled(this, val, this._scrollPosition);
			}
			if (this.snapping && !this._snapJumping && Mathf.Abs(this.LinearVelocity) <= this.snapVelocityThreshold && this.LinearVelocity != 0f)
			{
				this.Snap();
			}
			this._RefreshActive();
		}

		private void SnapJumpComplete()
		{
			this._snapJumping = false;
			this._scrollRect.inertia = this._snapInertia;
			EnhancedScrollerCellView cellView = null;
			for (int i = 0; i < this._activeCellViews.Count; i++)
			{
				if (this._activeCellViews[i].dataIndex == this._snapDataIndex)
				{
					cellView = this._activeCellViews[i];
					break;
				}
			}
			if (this.scrollerSnapped != null)
			{
				this.scrollerSnapped(this, this._snapCellViewIndex, this._snapDataIndex, cellView);
			}
		}

		private IEnumerator TweenPosition(EnhancedScroller.TweenType tweenType, float time, float start, float end, Action tweenComplete)
		{
			EnhancedScroller._TweenPosition_c__Iterator0 _TweenPosition_c__Iterator = new EnhancedScroller._TweenPosition_c__Iterator0();
			_TweenPosition_c__Iterator.tweenType = tweenType;
			_TweenPosition_c__Iterator.time = time;
			_TweenPosition_c__Iterator.end = end;
			_TweenPosition_c__Iterator.start = start;
			_TweenPosition_c__Iterator.tweenComplete = tweenComplete;
			_TweenPosition_c__Iterator._this = this;
			return _TweenPosition_c__Iterator;
		}

		private float linear(float start, float end, float val)
		{
			return Mathf.Lerp(start, end, val);
		}

		private static float spring(float start, float end, float val)
		{
			val = Mathf.Clamp01(val);
			val = (Mathf.Sin(val * 3.14159274f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
			return start + (end - start) * val;
		}

		private static float easeInQuad(float start, float end, float val)
		{
			end -= start;
			return end * val * val + start;
		}

		private static float easeOutQuad(float start, float end, float val)
		{
			end -= start;
			return -end * val * (val - 2f) + start;
		}

		private static float easeInOutQuad(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val + start;
			}
			val -= 1f;
			return -end / 2f * (val * (val - 2f) - 1f) + start;
		}

		private static float easeInCubic(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val + start;
		}

		private static float easeOutCubic(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val + 1f) + start;
		}

		private static float easeInOutCubic(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val + 2f) + start;
		}

		private static float easeInQuart(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val + start;
		}

		private static float easeOutQuart(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return -end * (val * val * val * val - 1f) + start;
		}

		private static float easeInOutQuart(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val + start;
			}
			val -= 2f;
			return -end / 2f * (val * val * val * val - 2f) + start;
		}

		private static float easeInQuint(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val * val + start;
		}

		private static float easeOutQuint(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val * val * val + 1f) + start;
		}

		private static float easeInOutQuint(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val * val * val + 2f) + start;
		}

		private static float easeInSine(float start, float end, float val)
		{
			end -= start;
			return -end * Mathf.Cos(val / 1f * 1.57079637f) + end + start;
		}

		private static float easeOutSine(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Sin(val / 1f * 1.57079637f) + start;
		}

		private static float easeInOutSine(float start, float end, float val)
		{
			end -= start;
			return -end / 2f * (Mathf.Cos(3.14159274f * val / 1f) - 1f) + start;
		}

		private static float easeInExpo(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
		}

		private static float easeOutExpo(float start, float end, float val)
		{
			end -= start;
			return end * (-Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
		}

		private static float easeInOutExpo(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
			}
			val -= 1f;
			return end / 2f * (-Mathf.Pow(2f, -10f * val) + 2f) + start;
		}

		private static float easeInCirc(float start, float end, float val)
		{
			end -= start;
			return -end * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}

		private static float easeOutCirc(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - val * val) + start;
		}

		private static float easeInOutCirc(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return -end / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
			}
			val -= 2f;
			return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
		}

		private static float easeInBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			return end - EnhancedScroller.easeOutBounce(0f, end, num - val) + start;
		}

		private static float easeOutBounce(float start, float end, float val)
		{
			val /= 1f;
			end -= start;
			if (val < 0.363636374f)
			{
				return end * (7.5625f * val * val) + start;
			}
			if (val < 0.727272749f)
			{
				val -= 0.545454562f;
				return end * (7.5625f * val * val + 0.75f) + start;
			}
			if ((double)val < 0.90909090909090906)
			{
				val -= 0.8181818f;
				return end * (7.5625f * val * val + 0.9375f) + start;
			}
			val -= 0.954545438f;
			return end * (7.5625f * val * val + 0.984375f) + start;
		}

		private static float easeInOutBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			if (val < num / 2f)
			{
				return EnhancedScroller.easeInBounce(0f, end, val * 2f) * 0.5f + start;
			}
			return EnhancedScroller.easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
		}

		private static float easeInBack(float start, float end, float val)
		{
			end -= start;
			val /= 1f;
			float num = 1.70158f;
			return end * val * val * ((num + 1f) * val - num) + start;
		}

		private static float easeOutBack(float start, float end, float val)
		{
			float num = 1.70158f;
			end -= start;
			val = val / 1f - 1f;
			return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
		}

		private static float easeInOutBack(float start, float end, float val)
		{
			float num = 1.70158f;
			end -= start;
			val /= 0.5f;
			if (val < 1f)
			{
				num *= 1.525f;
				return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
			}
			val -= 2f;
			num *= 1.525f;
			return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
		}

		private static float easeInElastic(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= num;
			if (val == 1f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
			}
			val -= 1f;
			return -(num3 * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val * num - num4) * 6.28318548f / num2)) + start;
		}

		private static float easeOutElastic(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= num;
			if (val == 1f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
			}
			return num3 * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val * num - num4) * 6.28318548f / num2) + end + start;
		}

		private static float easeInOutElastic(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= num / 2f;
			if (val == 2f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
			}
			if (val < 1f)
			{
				val -= 1f;
				return -0.5f * (num3 * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val * num - num4) * 6.28318548f / num2)) + start;
			}
			val -= 1f;
			return num3 * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val * num - num4) * 6.28318548f / num2) * 0.5f + end + start;
		}
	}
}
