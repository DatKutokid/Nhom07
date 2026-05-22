using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class ThanhToan
{
    public int MaThanhToan { get; set; }

    public int MaHd { get; set; }

    public string? PhuongThuc { get; set; }

    public decimal SoTien { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; } = null!;
}
