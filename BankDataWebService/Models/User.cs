using System.Drawing;

namespace BankDataWebService.Models
{
    public class User
    {
        public string? userName { get; set; }

        public string? email { get; set; }

        public string? address { get; set; }

        public string? password { get; set; }

        public int phone { get; set; }

        public Bitmap picture { get; set; }
    }
}
