using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VicemAPI.Models.Entities
{
  public class Department
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required(ErrorMessage = "Mã bộ môn không được để trống")]
    [MaxLength(10, ErrorMessage = "Mã bộ môn không được vượt quá 10 ký tự")]
    [Column(TypeName = "varchar(10)")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Tên bộ môn không được để trống")]
    [MaxLength(100, ErrorMessage = "Tên bộ môn không được vượt quá 100 ký tự")]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Column(TypeName = "varchar(15)")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Column(TypeName = "varchar(50)")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    [Column(TypeName = "nvarchar(200)")]
    [MaxLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
    public string Address { get; set; }

    [Column(TypeName = "nvarchar(500)")]
    [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string? Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; } = false;

    public ICollection<Advisor>? Advisors { get; set; } = new List<Advisor>();

    public ICollection<Student>? Students { get; set; }

    public ICollection<Council>? Councils { get; set; }

  }
}