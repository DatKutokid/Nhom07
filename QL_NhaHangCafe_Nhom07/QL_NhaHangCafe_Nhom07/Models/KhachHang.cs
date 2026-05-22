using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class KhachHang
{
    public int MaKh { get; set; }

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? Email { get; set; }

    public int? DiemTichLuy { get; set; }

    public virtual ICollection<DatBan> DatBans { get; set; } = new List<DatBan>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
