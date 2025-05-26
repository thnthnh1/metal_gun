using System;
using System.Collections.Generic;

public class _StaticTournamentRankData : List<StaticTournamentRankData>
{
	public StaticTournamentRankData GetData(int rankIndex)
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticTournamentRankData staticTournamentRankData = base[i];
			if (staticTournamentRankData.rankIndex == rankIndex)
			{
				return staticTournamentRankData;
			}
		}
		return null;
	}

	public TournamentRank GetCurrentRank(int score)
	{
		for (int i = base.Count - 1; i >= 0; i--)
		{
			StaticTournamentRankData staticTournamentRankData = base[i];
			if (staticTournamentRankData.score <= score)
			{
				return (TournamentRank)staticTournamentRankData.rankIndex;
			}
		}
		return (TournamentRank)base[base.Count - 1].rankIndex;
	}
}
