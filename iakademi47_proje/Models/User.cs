using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iakademi47_proje.Models
{
    public class User
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "İsim Soyisim Zorunludur")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter girilebilir")]
        [DisplayName("Ad Soyad")]
        public string? NameSurname { get; set; }

        [StringLength(100, ErrorMessage = "En fazla 100 karakter girilebilir")]
        [Required(ErrorMessage ="Email Zorunludur")]
        [DisplayName("Email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter girilebilir")]
        [DataType(DataType.Password)]
        [DisplayName("Şifre")]
        public string? Password { get; set; }

        [DisplayName("Telefon")]
        public string? Telephone { get; set; }

        [DisplayName("Fatura Adresi")]
        public string? InvoiceAddress { get; set; }

        public bool IsAdmin { get; set; }

        [DisplayName("Aktif")]
        public bool Active { get; set; }

        [NotMappedAttribute]
        public string Message { get; set; }

    }
}
