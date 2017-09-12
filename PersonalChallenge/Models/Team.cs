using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalChallenge.Models
{
    public class Team
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ChallengesDone { get; set; }
		public string TeamPic { get; set; }
		public virtual ICollection<ApplicationUser> Members { get; set; }
		public virtual ICollection<Challenge> Challenges { get; set; }
		public Team()
		{
			Members = new List<ApplicationUser>();
			Challenges = new List<Challenge>();
		}
    }
}
