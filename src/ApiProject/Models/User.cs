using System.ComponentModel.DataAnnotations;
using InterviewApiDemo.Data.Annotations;

namespace InterviewApiDemo.Models;

public class User
{
    [Key]
    [Required]
    [Length(5, 40)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [UserDateOfBirthValidation]
    public DateTime DateOfBirth { get; set; } = default;
}
