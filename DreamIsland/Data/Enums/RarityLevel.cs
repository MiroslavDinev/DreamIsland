namespace DreamIsland.Data.Enums
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public enum RarityLevel
    {
        Rare = 1,

        [Display(Name = "Very Rare")]
        [Description("Very Rare")]
        VeryRare = 2,
        Unique = 3,
        Legendary = 4
    }
}
