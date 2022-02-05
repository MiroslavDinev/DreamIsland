namespace DreamIsland.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DreamIsland.Data.DataConstants.User;
    using static DreamIsland.Data.DataConstants.ContactMessage;
    using static DreamIsland.Data.DataConstants.ContactSubject;

    public class ContactFormViewModel
    {
        [Required]
        [StringLength(NicknameMaxLength, MinimumLength = NicknameMinLength, ErrorMessage = "The name length should be between {2} and {1} symbols")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(SubjectMaxLength, MinimumLength = SubjectMinLength, ErrorMessage = "The subject length should be between {2} and {1} symbols")]
        public string Subject { get; set; }

        [Required]
        [StringLength(MessageMaxLength, MinimumLength = MessageMinLength, ErrorMessage = "The subject length should be between {2} and {1} symbols")]
        public string Content { get; set; }
    }
}
