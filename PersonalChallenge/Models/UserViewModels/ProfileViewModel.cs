using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalChallenge.Models.UserViewModels
{
	public class ProfileViewModel
    {
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public DateTime Birthday { get; set; }
		public string ProfilePic { get; set; }
		public Guid? TeamId { get; set; }
		[Display(Name = "Team Name")]
		public string TeamName { get; set; }
		public int ChallengesDone { get; set; }
	}
}
