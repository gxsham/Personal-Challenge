using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalChallenge.Models.ChallengeViewModels
{
    public class ChallengeDetailsViewModel
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid TeamId { get; set; }
		public bool IsActive { get; set; }
		[Display(Name = "Created by")]
		public string OwnerName { get; set; }
		public ICollection<ProofViewModel> Proofs { get; set; }
		public bool AlreadyAdded { get; set; }
		[Display(Name = "Challenge Type")]
		public string ChallengeType { get; set; }
		public ChallengeDetailsViewModel()
		{
			Proofs = new List<ProofViewModel>();
		}
	}

	public class ProofViewModel
	{
		public string Comment { get; set; }
		public string ContentPath { get; set; }
		public string UserId { get; set; }
		public string OwnerName { get; set; }
		public double Quantity { get; set; }

		[Display(Name = "Measured In")]
		public string MesuredIn { get; set; }
	}
}
