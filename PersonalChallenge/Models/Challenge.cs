using System;
using System.Collections.Generic;

namespace PersonalChallenge.Models
{
	public class Challenge
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid TeamId { get; set; }
		public virtual Team Team { get; set; }
		public bool IsActive { get; set; }
		public string UserId { get; set; }
		public virtual ApplicationUser User { get; set; }
		public virtual ICollection<Proof> Proofs { get; set; }
		public Guid ChallengeTypeId { get; set; }
		public virtual ChallengeType ChallengeType { get; set; }
    }
}
