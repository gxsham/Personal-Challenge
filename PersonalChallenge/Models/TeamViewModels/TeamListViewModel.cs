using System;

namespace PersonalChallenge.Models.TeamViewModels
{
	public class TeamListViewModel
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int ChallengesDone { get; set; }
		public int Members { get; set; }
		public int ActiveChallenges { get; set; }
		public string TeamPic { get; set; }
	}
}
