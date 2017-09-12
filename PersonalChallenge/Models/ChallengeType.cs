using System;

namespace PersonalChallenge.Models
{
	public class ChallengeType
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string MeasuredIn { get; set; }
		public bool Approved { get; set; }
    }
}
