using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class Ban
{
    public int MaBan { get; set; }

    public string TenBan { get; set; } = null!;

    public int SoChoNgoi { get; set; }

    public string TrangThai { get; set; } = null!;

    public int? MaKhuVuc { get; set; }

    public virtual ICollection<DatBan> DatBans { get; set; } = new List<DatBan>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual KhuVuc? MaKhuVucNavigation { get; set; }
}
