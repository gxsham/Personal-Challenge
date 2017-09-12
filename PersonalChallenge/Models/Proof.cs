using System;

namespace PersonalChallenge.Models
{
	public class Proof
    {
		public Guid Id { get; set; }
		public string Comment { get; set; }
		public string ContentPath { get; set; }
		public string UserId { get; set; }
		public virtual ApplicationUser User { get; set; }
		public Guid ChallengeId { get; set; }
		public virtual Challenge Challenge { get; set; }
		public double Quantity { get; set; }
    }
}
