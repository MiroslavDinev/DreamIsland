namespace DreamIsland.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using static DataConstants.User;

    public class User : IdentityUser
    {
        [MaxLength(NicknameMaxLength)]
        public string Nickname { get; set; }

        [MaxLength(OccupationMaxLength)]
        public string OccupationalField { get; set; }
    }
}
