using System;
using Microsoft.AspNetCore.Identity;

namespace PersonalChallenge.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
    {
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime Birthday { get; set; }
		public string ProfilePic { get; set; }
		public Guid? TeamId { get; set; }
		public virtual Team Team { get; set; }
		public int ChallengesDone { get; set; }
    }
}
