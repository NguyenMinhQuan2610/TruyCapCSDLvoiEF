using Microsoft.AspNetCore.Identity;

namespace WebBanHang.Models
{
    public class ApplicationUser:IdentityUser
    {
        //Bổ sung các tt còn thiếu
        public string Fullname { set; get; }
        public DateTime BirthDay { set; get; }
    }
}
