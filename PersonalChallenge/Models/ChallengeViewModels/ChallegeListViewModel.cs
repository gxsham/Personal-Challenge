using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalChallenge.Models.ChallengeViewModels
{
    public class ChallegeListViewModel
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public int Proofs { get; set; }
		public int Members { get; set; }
		[Display(Name = "Challenge Type")]
		public string ChallengeType { get; set; }
		public Guid ChallengeTypeId { get; set; }
	}
}
