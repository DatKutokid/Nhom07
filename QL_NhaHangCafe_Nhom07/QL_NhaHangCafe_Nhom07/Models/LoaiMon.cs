using System;
using System.Collections.Generic;

namespace QL_NhaHangCafe_Nhom07.Models;

public partial class LoaiMon
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public virtual ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();
}
