using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class ChiTietHoaDon
{
    public int MaCthd { get; set; }

    public int MaHd { get; set; }

    public int MaMon { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public string? GhiChu { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; } = null!;

    public virtual MonAn MaMonNavigation { get; set; } = null!;
}
