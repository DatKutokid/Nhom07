using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class DatBan
{
    public int MaDatBan { get; set; }

    public int? MaKh { get; set; }

    public int MaBan { get; set; }

    public int SoLuongKhach { get; set; }

    public DateTime ThoiGianDat { get; set; }

    public string? GhiChu { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual Ban MaBanNavigation { get; set; } = null!;

    public virtual KhachHang? MaKhNavigation { get; set; }
}
