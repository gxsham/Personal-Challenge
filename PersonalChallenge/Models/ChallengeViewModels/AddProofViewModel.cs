using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalChallenge.Models.ChallengeViewModels
{
    public class AddProofViewModel
    {
		public string Comment { get; set; }
		[Required]
		[Display(Name = "Proof file")]
		public IFormFile ContentPath { get; set; }
		[Required]
		public double Quantity { get; set; }
	}
}
