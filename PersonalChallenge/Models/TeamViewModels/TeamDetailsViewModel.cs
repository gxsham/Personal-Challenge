using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalChallenge.Models.TeamViewModels
{
    public class TeamDetailsViewModel
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public int ChallengesDone { get; set; }
		public string TeamPic { get; set; }
		public virtual IEnumerable<MemberDetailsViewModel> Members { get; set; }
		public virtual IEnumerable<ChallengeDetailsViewModel> Challenges { get; set; }
		
	}
	public class MemberDetailsViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public int ChallengesDone { get; set; }
	}

	public class ChallengeDetailsViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
	}
}
