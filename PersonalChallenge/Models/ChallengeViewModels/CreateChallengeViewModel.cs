using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalChallenge.Models.ChallengeViewModels
{
	public class CreateChallengeViewModel
    {
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		//automatically add a proof
		public string Comment { get; set; }
		[Display(Name = "Proof picture")]
		public IFormFile ContentPath { get; set; }
		[Required]
		[Display(Name = "Challenge type")]
		public Guid ChallengeTypeId { get; set; }
		public double Quantity { get; set; }
	}
}
