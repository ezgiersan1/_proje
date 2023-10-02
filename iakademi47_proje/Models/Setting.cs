using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iakademi47_proje.Models
{
    public class Setting
    {
		internal string telephone;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }
        public string? Telephone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int MainPageCount { get; set; }
        public int SubPageCount { get; set; }
        public int MyProperty { get; set; }

    }
}
