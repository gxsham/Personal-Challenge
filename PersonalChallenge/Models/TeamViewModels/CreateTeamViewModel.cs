using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalChallenge.Models.TeamViewModels
{
    public class CreateTeamViewModel
    {
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		[Display(Name = "Team Picture")]
		public IFormFile TeamPic { get; set; }
    }
}
