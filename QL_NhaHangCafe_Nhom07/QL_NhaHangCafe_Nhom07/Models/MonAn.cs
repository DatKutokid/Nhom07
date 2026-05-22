using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class MonAn
{
    public int MaMon { get; set; }

    public string TenMon { get; set; } = null!;

    public decimal Gia { get; set; }

    public string? TrangThai { get; set; }

    public string? HinhAnh { get; set; }

    public string? MoTa { get; set; }

    public int? MaLoai { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual LoaiMon? MaLoaiNavigation { get; set; }
}
