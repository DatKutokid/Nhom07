using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class KhuVuc
{
    public int MaKhuVuc { get; set; }

    public string TenKhuVuc { get; set; } = null!;

    public virtual ICollection<Ban> Bans { get; set; } = new List<Ban>();
}
