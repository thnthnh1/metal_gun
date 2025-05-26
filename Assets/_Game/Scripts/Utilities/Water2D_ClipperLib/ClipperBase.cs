using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Water2D_ClipperLib
{
	public class ClipperBase
	{
		protected const double horizontal = -3.4E+38;

		protected const int Skip = -2;

		protected const int Unassigned = -1;

		protected const double tolerance = 1E-20;

		public const long loRange = 1073741823L;

		public const long hiRange = 4611686018427387903L;

		internal LocalMinima m_MinimaList;

		internal LocalMinima m_CurrentLM;

		internal List<List<TEdge>> m_edges = new List<List<TEdge>>();

		internal bool m_UseFullRange;

		internal bool m_HasOpenPaths;

		private bool _PreserveCollinear_k__BackingField;

		public bool PreserveCollinear
		{
			get;
			set;
		}

		internal ClipperBase()
		{
			this.m_MinimaList = null;
			this.m_CurrentLM = null;
			this.m_UseFullRange = false;
			this.m_HasOpenPaths = false;
		}

		internal static bool near_zero(double val)
		{
			return val > -1E-20 && val < 1E-20;
		}

		public void Swap(ref long val1, ref long val2)
		{
			long num = val1;
			val1 = val2;
			val2 = num;
		}

		internal static bool IsHorizontal(TEdge e)
		{
			return e.Delta.Y == 0L;
		}

		internal bool PointIsVertex(IntPoint pt, OutPt pp)
		{
			OutPt outPt = pp;
			while (!(outPt.Pt == pt))
			{
				outPt = outPt.Next;
				if (outPt == pp)
				{
					return false;
				}
			}
			return true;
		}

		internal bool PointOnLineSegment(IntPoint pt, IntPoint linePt1, IntPoint linePt2, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return (pt.X == linePt1.X && pt.Y == linePt1.Y) || (pt.X == linePt2.X && pt.Y == linePt2.Y) || (pt.X > linePt1.X == pt.X < linePt2.X && pt.Y > linePt1.Y == pt.Y < linePt2.Y && Int128.Int128Mul(pt.X - linePt1.X, linePt2.Y - linePt1.Y) == Int128.Int128Mul(linePt2.X - linePt1.X, pt.Y - linePt1.Y));
			}
			return (pt.X == linePt1.X && pt.Y == linePt1.Y) || (pt.X == linePt2.X && pt.Y == linePt2.Y) || (pt.X > linePt1.X == pt.X < linePt2.X && pt.Y > linePt1.Y == pt.Y < linePt2.Y && (pt.X - linePt1.X) * (linePt2.Y - linePt1.Y) == (linePt2.X - linePt1.X) * (pt.Y - linePt1.Y));
		}

		internal bool PointOnPolygon(IntPoint pt, OutPt pp, bool UseFullRange)
		{
			OutPt outPt = pp;
			while (!this.PointOnLineSegment(pt, outPt.Pt, outPt.Next.Pt, UseFullRange))
			{
				outPt = outPt.Next;
				if (outPt == pp)
				{
					return false;
				}
			}
			return true;
		}

		internal static bool SlopesEqual(TEdge e1, TEdge e2, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return Int128.Int128Mul(e1.Delta.Y, e2.Delta.X) == Int128.Int128Mul(e1.Delta.X, e2.Delta.Y);
			}
			return e1.Delta.Y * e2.Delta.X == e1.Delta.X * e2.Delta.Y;
		}

		protected static bool SlopesEqual(IntPoint pt1, IntPoint pt2, IntPoint pt3, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return Int128.Int128Mul(pt1.Y - pt2.Y, pt2.X - pt3.X) == Int128.Int128Mul(pt1.X - pt2.X, pt2.Y - pt3.Y);
			}
			return (pt1.Y - pt2.Y) * (pt2.X - pt3.X) - (pt1.X - pt2.X) * (pt2.Y - pt3.Y) == 0L;
		}

		protected static bool SlopesEqual(IntPoint pt1, IntPoint pt2, IntPoint pt3, IntPoint pt4, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return Int128.Int128Mul(pt1.Y - pt2.Y, pt3.X - pt4.X) == Int128.Int128Mul(pt1.X - pt2.X, pt3.Y - pt4.Y);
			}
			return (pt1.Y - pt2.Y) * (pt3.X - pt4.X) - (pt1.X - pt2.X) * (pt3.Y - pt4.Y) == 0L;
		}

		public virtual void Clear()
		{
			this.DisposeLocalMinimaList();
			for (int i = 0; i < this.m_edges.Count; i++)
			{
				for (int j = 0; j < this.m_edges[i].Count; j++)
				{
					this.m_edges[i][j] = null;
				}
				this.m_edges[i].Clear();
			}
			this.m_edges.Clear();
			this.m_UseFullRange = false;
			this.m_HasOpenPaths = false;
		}

		private void DisposeLocalMinimaList()
		{
			while (this.m_MinimaList != null)
			{
				LocalMinima next = this.m_MinimaList.Next;
				this.m_MinimaList = null;
				this.m_MinimaList = next;
			}
			this.m_CurrentLM = null;
		}

		private void RangeTest(IntPoint Pt, ref bool useFullRange)
		{
			if (useFullRange)
			{
				if (Pt.X > 4611686018427387903L || Pt.Y > 4611686018427387903L || -Pt.X > 4611686018427387903L || -Pt.Y > 4611686018427387903L)
				{
					throw new ClipperException("Coordinate outside allowed range");
				}
			}
			else if (Pt.X > 1073741823L || Pt.Y > 1073741823L || -Pt.X > 1073741823L || -Pt.Y > 1073741823L)
			{
				useFullRange = true;
				this.RangeTest(Pt, ref useFullRange);
			}
		}

		private void InitEdge(TEdge e, TEdge eNext, TEdge ePrev, IntPoint pt)
		{
			e.Next = eNext;
			e.Prev = ePrev;
			e.Curr = pt;
			e.OutIdx = -1;
		}

		private void InitEdge2(TEdge e, PolyType polyType)
		{
			if (e.Curr.Y >= e.Next.Curr.Y)
			{
				e.Bot = e.Curr;
				e.Top = e.Next.Curr;
			}
			else
			{
				e.Top = e.Curr;
				e.Bot = e.Next.Curr;
			}
			this.SetDx(e);
			e.PolyTyp = polyType;
		}

		private TEdge FindNextLocMin(TEdge E)
		{
			TEdge tEdge;
			while (true)
			{
				while (E.Bot != E.Prev.Bot || E.Curr == E.Top)
				{
					E = E.Next;
				}
				if (E.Dx != -3.4E+38 && E.Prev.Dx != -3.4E+38)
				{
					break;
				}
				while (E.Prev.Dx == -3.4E+38)
				{
					E = E.Prev;
				}
				tEdge = E;
				while (E.Dx == -3.4E+38)
				{
					E = E.Next;
				}
				if (E.Top.Y != E.Prev.Bot.Y)
				{
					goto IL_E3;
				}
			}
			return E;
			IL_E3:
			if (tEdge.Prev.Bot.X < E.Bot.X)
			{
				E = tEdge;
			}
			return E;
		}

		private TEdge ProcessBound(TEdge E, bool LeftBoundIsForward)
		{
			TEdge tEdge = E;
			if (tEdge.OutIdx == -2)
			{
				E = tEdge;
				if (LeftBoundIsForward)
				{
					while (E.Top.Y == E.Next.Bot.Y)
					{
						E = E.Next;
					}
					while (E != tEdge && E.Dx == -3.4E+38)
					{
						E = E.Prev;
					}
				}
				else
				{
					while (E.Top.Y == E.Prev.Bot.Y)
					{
						E = E.Prev;
					}
					while (E != tEdge && E.Dx == -3.4E+38)
					{
						E = E.Next;
					}
				}
				if (E == tEdge)
				{
					if (LeftBoundIsForward)
					{
						tEdge = E.Next;
					}
					else
					{
						tEdge = E.Prev;
					}
				}
				else
				{
					if (LeftBoundIsForward)
					{
						E = tEdge.Next;
					}
					else
					{
						E = tEdge.Prev;
					}
					LocalMinima localMinima = new LocalMinima();
					localMinima.Next = null;
					localMinima.Y = E.Bot.Y;
					localMinima.LeftBound = null;
					localMinima.RightBound = E;
					E.WindDelta = 0;
					tEdge = this.ProcessBound(E, LeftBoundIsForward);
					this.InsertLocalMinima(localMinima);
				}
				return tEdge;
			}
			TEdge tEdge2;
			if (E.Dx == -3.4E+38)
			{
				if (LeftBoundIsForward)
				{
					tEdge2 = E.Prev;
				}
				else
				{
					tEdge2 = E.Next;
				}
				if (tEdge2.OutIdx != -2)
				{
					if (tEdge2.Dx == -3.4E+38)
					{
						if (tEdge2.Bot.X != E.Bot.X && tEdge2.Top.X != E.Bot.X)
						{
							this.ReverseHorizontal(E);
						}
					}
					else if (tEdge2.Bot.X != E.Bot.X)
					{
						this.ReverseHorizontal(E);
					}
				}
			}
			tEdge2 = E;
			if (LeftBoundIsForward)
			{
				while (tEdge.Top.Y == tEdge.Next.Bot.Y && tEdge.Next.OutIdx != -2)
				{
					tEdge = tEdge.Next;
				}
				if (tEdge.Dx == -3.4E+38 && tEdge.Next.OutIdx != -2)
				{
					TEdge tEdge3 = tEdge;
					while (tEdge3.Prev.Dx == -3.4E+38)
					{
						tEdge3 = tEdge3.Prev;
					}
					if (tEdge3.Prev.Top.X == tEdge.Next.Top.X)
					{
						if (!LeftBoundIsForward)
						{
							tEdge = tEdge3.Prev;
						}
					}
					else if (tEdge3.Prev.Top.X > tEdge.Next.Top.X)
					{
						tEdge = tEdge3.Prev;
					}
				}
				while (E != tEdge)
				{
					E.NextInLML = E.Next;
					if (E.Dx == -3.4E+38 && E != tEdge2 && E.Bot.X != E.Prev.Top.X)
					{
						this.ReverseHorizontal(E);
					}
					E = E.Next;
				}
				if (E.Dx == -3.4E+38 && E != tEdge2 && E.Bot.X != E.Prev.Top.X)
				{
					this.ReverseHorizontal(E);
				}
				tEdge = tEdge.Next;
			}
			else
			{
				while (tEdge.Top.Y == tEdge.Prev.Bot.Y && tEdge.Prev.OutIdx != -2)
				{
					tEdge = tEdge.Prev;
				}
				if (tEdge.Dx == -3.4E+38 && tEdge.Prev.OutIdx != -2)
				{
					TEdge tEdge3 = tEdge;
					while (tEdge3.Next.Dx == -3.4E+38)
					{
						tEdge3 = tEdge3.Next;
					}
					if (tEdge3.Next.Top.X == tEdge.Prev.Top.X)
					{
						if (!LeftBoundIsForward)
						{
							tEdge = tEdge3.Next;
						}
					}
					else if (tEdge3.Next.Top.X > tEdge.Prev.Top.X)
					{
						tEdge = tEdge3.Next;
					}
				}
				while (E != tEdge)
				{
					E.NextInLML = E.Prev;
					if (E.Dx == -3.4E+38 && E != tEdge2 && E.Bot.X != E.Next.Top.X)
					{
						this.ReverseHorizontal(E);
					}
					E = E.Prev;
				}
				if (E.Dx == -3.4E+38 && E != tEdge2 && E.Bot.X != E.Next.Top.X)
				{
					this.ReverseHorizontal(E);
				}
				tEdge = tEdge.Prev;
			}
			return tEdge;
		}

		public bool AddPath(List<IntPoint> pg, PolyType polyType, bool Closed)
		{
			if (!Closed)
			{
				throw new ClipperException("AddPath: Open paths have been disabled.");
			}
			int num = pg.Count - 1;
			if (Closed)
			{
				while (num > 0 && pg[num] == pg[0])
				{
					num--;
				}
			}
			while (num > 0 && pg[num] == pg[num - 1])
			{
				num--;
			}
			if ((Closed && num < 2) || (!Closed && num < 1))
			{
				return false;
			}
			List<TEdge> list = new List<TEdge>(num + 1);
			for (int i = 0; i <= num; i++)
			{
				list.Add(new TEdge());
			}
			bool flag = true;
			list[1].Curr = pg[1];
			this.RangeTest(pg[0], ref this.m_UseFullRange);
			this.RangeTest(pg[num], ref this.m_UseFullRange);
			this.InitEdge(list[0], list[1], list[num], pg[0]);
			this.InitEdge(list[num], list[0], list[num - 1], pg[num]);
			for (int j = num - 1; j >= 1; j--)
			{
				this.RangeTest(pg[j], ref this.m_UseFullRange);
				this.InitEdge(list[j], list[j + 1], list[j - 1], pg[j]);
			}
			TEdge tEdge = list[0];
			TEdge tEdge2 = tEdge;
			TEdge tEdge3 = tEdge;
			while (true)
			{
				if (tEdge2.Curr == tEdge2.Next.Curr && (Closed || tEdge2.Next != tEdge))
				{
					if (tEdge2 == tEdge2.Next)
					{
						break;
					}
					if (tEdge2 == tEdge)
					{
						tEdge = tEdge2.Next;
					}
					tEdge2 = this.RemoveEdge(tEdge2);
					tEdge3 = tEdge2;
				}
				else
				{
					if (tEdge2.Prev == tEdge2.Next)
					{
						break;
					}
					if (Closed && ClipperBase.SlopesEqual(tEdge2.Prev.Curr, tEdge2.Curr, tEdge2.Next.Curr, this.m_UseFullRange) && (!this.PreserveCollinear || !this.Pt2IsBetweenPt1AndPt3(tEdge2.Prev.Curr, tEdge2.Curr, tEdge2.Next.Curr)))
					{
						if (tEdge2 == tEdge)
						{
							tEdge = tEdge2.Next;
						}
						tEdge2 = this.RemoveEdge(tEdge2);
						tEdge2 = tEdge2.Prev;
						tEdge3 = tEdge2;
					}
					else
					{
						tEdge2 = tEdge2.Next;
						if (tEdge2 == tEdge3 || (!Closed && tEdge2.Next == tEdge))
						{
							break;
						}
					}
				}
			}
			if ((!Closed && tEdge2 == tEdge2.Next) || (Closed && tEdge2.Prev == tEdge2.Next))
			{
				return false;
			}
			if (!Closed)
			{
				this.m_HasOpenPaths = true;
				tEdge.Prev.OutIdx = -2;
			}
			tEdge2 = tEdge;
			do
			{
				this.InitEdge2(tEdge2, polyType);
				tEdge2 = tEdge2.Next;
				if (flag && tEdge2.Curr.Y != tEdge.Curr.Y)
				{
					flag = false;
				}
			}
			while (tEdge2 != tEdge);
			if (!flag)
			{
				this.m_edges.Add(list);
				TEdge tEdge4 = null;
				if (tEdge2.Prev.Bot == tEdge2.Prev.Top)
				{
					tEdge2 = tEdge2.Next;
				}
				while (true)
				{
					tEdge2 = this.FindNextLocMin(tEdge2);
					if (tEdge2 == tEdge4)
					{
						break;
					}
					if (tEdge4 == null)
					{
						tEdge4 = tEdge2;
					}
					LocalMinima localMinima = new LocalMinima();
					localMinima.Next = null;
					localMinima.Y = tEdge2.Bot.Y;
					bool flag2;
					if (tEdge2.Dx < tEdge2.Prev.Dx)
					{
						localMinima.LeftBound = tEdge2.Prev;
						localMinima.RightBound = tEdge2;
						flag2 = false;
					}
					else
					{
						localMinima.LeftBound = tEdge2;
						localMinima.RightBound = tEdge2.Prev;
						flag2 = true;
					}
					localMinima.LeftBound.Side = EdgeSide.esLeft;
					localMinima.RightBound.Side = EdgeSide.esRight;
					if (!Closed)
					{
						localMinima.LeftBound.WindDelta = 0;
					}
					else if (localMinima.LeftBound.Next == localMinima.RightBound)
					{
						localMinima.LeftBound.WindDelta = -1;
					}
					else
					{
						localMinima.LeftBound.WindDelta = 1;
					}
					localMinima.RightBound.WindDelta = -localMinima.LeftBound.WindDelta;
					tEdge2 = this.ProcessBound(localMinima.LeftBound, flag2);
					if (tEdge2.OutIdx == -2)
					{
						tEdge2 = this.ProcessBound(tEdge2, flag2);
					}
					TEdge tEdge5 = this.ProcessBound(localMinima.RightBound, !flag2);
					if (tEdge5.OutIdx == -2)
					{
						tEdge5 = this.ProcessBound(tEdge5, !flag2);
					}
					if (localMinima.LeftBound.OutIdx == -2)
					{
						localMinima.LeftBound = null;
					}
					else if (localMinima.RightBound.OutIdx == -2)
					{
						localMinima.RightBound = null;
					}
					this.InsertLocalMinima(localMinima);
					if (!flag2)
					{
						tEdge2 = tEdge5;
					}
				}
				return true;
			}
			if (Closed)
			{
				return false;
			}
			tEdge2.Prev.OutIdx = -2;
			if (tEdge2.Prev.Bot.X < tEdge2.Prev.Top.X)
			{
				this.ReverseHorizontal(tEdge2.Prev);
			}
			LocalMinima localMinima2 = new LocalMinima();
			localMinima2.Next = null;
			localMinima2.Y = tEdge2.Bot.Y;
			localMinima2.LeftBound = null;
			localMinima2.RightBound = tEdge2;
			localMinima2.RightBound.Side = EdgeSide.esRight;
			localMinima2.RightBound.WindDelta = 0;
			while (tEdge2.Next.OutIdx != -2)
			{
				tEdge2.NextInLML = tEdge2.Next;
				if (tEdge2.Bot.X != tEdge2.Prev.Top.X)
				{
					this.ReverseHorizontal(tEdge2);
				}
				tEdge2 = tEdge2.Next;
			}
			this.InsertLocalMinima(localMinima2);
			this.m_edges.Add(list);
			return true;
		}

		public bool AddPaths(List<List<IntPoint>> ppg, PolyType polyType, bool closed)
		{
			bool result = false;
			for (int i = 0; i < ppg.Count; i++)
			{
				if (this.AddPath(ppg[i], polyType, closed))
				{
					result = true;
				}
			}
			return result;
		}

		internal bool Pt2IsBetweenPt1AndPt3(IntPoint pt1, IntPoint pt2, IntPoint pt3)
		{
			if (pt1 == pt3 || pt1 == pt2 || pt3 == pt2)
			{
				return false;
			}
			if (pt1.X != pt3.X)
			{
				return pt2.X > pt1.X == pt2.X < pt3.X;
			}
			return pt2.Y > pt1.Y == pt2.Y < pt3.Y;
		}

		private TEdge RemoveEdge(TEdge e)
		{
			e.Prev.Next = e.Next;
			e.Next.Prev = e.Prev;
			TEdge next = e.Next;
			e.Prev = null;
			return next;
		}

		private void SetDx(TEdge e)
		{
			e.Delta.X = e.Top.X - e.Bot.X;
			e.Delta.Y = e.Top.Y - e.Bot.Y;
			if (e.Delta.Y == 0L)
			{
				e.Dx = -3.4E+38;
			}
			else
			{
				e.Dx = (double)e.Delta.X / (double)e.Delta.Y;
			}
		}

		private void InsertLocalMinima(LocalMinima newLm)
		{
			if (this.m_MinimaList == null)
			{
				this.m_MinimaList = newLm;
			}
			else if (newLm.Y >= this.m_MinimaList.Y)
			{
				newLm.Next = this.m_MinimaList;
				this.m_MinimaList = newLm;
			}
			else
			{
				LocalMinima localMinima = this.m_MinimaList;
				while (localMinima.Next != null && newLm.Y < localMinima.Next.Y)
				{
					localMinima = localMinima.Next;
				}
				newLm.Next = localMinima.Next;
				localMinima.Next = newLm;
			}
		}

		protected void PopLocalMinima()
		{
			if (this.m_CurrentLM == null)
			{
				return;
			}
			this.m_CurrentLM = this.m_CurrentLM.Next;
		}

		private void ReverseHorizontal(TEdge e)
		{
			this.Swap(ref e.Top.X, ref e.Bot.X);
		}

		protected virtual void Reset()
		{
			this.m_CurrentLM = this.m_MinimaList;
			if (this.m_CurrentLM == null)
			{
				return;
			}
			for (LocalMinima localMinima = this.m_MinimaList; localMinima != null; localMinima = localMinima.Next)
			{
				TEdge tEdge = localMinima.LeftBound;
				if (tEdge != null)
				{
					tEdge.Curr = tEdge.Bot;
					tEdge.Side = EdgeSide.esLeft;
					tEdge.OutIdx = -1;
				}
				tEdge = localMinima.RightBound;
				if (tEdge != null)
				{
					tEdge.Curr = tEdge.Bot;
					tEdge.Side = EdgeSide.esRight;
					tEdge.OutIdx = -1;
				}
			}
		}

		public static IntRect GetBounds(List<List<IntPoint>> paths)
		{
			int i = 0;
			int count = paths.Count;
			while (i < count && paths[i].Count == 0)
			{
				i++;
			}
			if (i == count)
			{
				return new IntRect(0L, 0L, 0L, 0L);
			}
			IntRect result = default(IntRect);
			result.left = paths[i][0].X;
			result.right = result.left;
			result.top = paths[i][0].Y;
			result.bottom = result.top;
			while (i < count)
			{
				for (int j = 0; j < paths[i].Count; j++)
				{
					if (paths[i][j].X < result.left)
					{
						result.left = paths[i][j].X;
					}
					else if (paths[i][j].X > result.right)
					{
						result.right = paths[i][j].X;
					}
					if (paths[i][j].Y < result.top)
					{
						result.top = paths[i][j].Y;
					}
					else if (paths[i][j].Y > result.bottom)
					{
						result.bottom = paths[i][j].Y;
					}
				}
				i++;
			}
			return result;
		}
	}
}
