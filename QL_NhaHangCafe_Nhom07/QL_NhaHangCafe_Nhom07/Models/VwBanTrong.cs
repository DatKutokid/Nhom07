using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class VwBanTrong
{
    public int MaBan { get; set; }

    public string TenBan { get; set; } = null!;

    public int SoChoNgoi { get; set; }

    public string? TrangThai { get; set; }

    public int? MaKhuVuc { get; set; }
}
