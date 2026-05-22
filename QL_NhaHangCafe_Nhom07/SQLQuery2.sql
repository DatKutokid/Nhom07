CREATE DATABASE QL_NhaHangCafe;
GO

USE QL_NhaHangCafe;
GO

-- =========================================
-- 1. TÀI KHOẢN
-- =========================================
CREATE TABLE TaiKhoan
(
    MaTK INT IDENTITY(1,1),
    TenDangNhap VARCHAR(50) NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    VaiTro NVARCHAR(30) NOT NULL,
    TrangThai BIT DEFAULT 1,

    CONSTRAINT PK_TaiKhoan PRIMARY KEY (MaTK),
    CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE (TenDangNhap)
);
GO

-- =========================================
-- 2. NHÂN VIÊN
-- =========================================
CREATE TABLE NhanVien
(
    MaNV INT IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    SoDienThoai VARCHAR(15),
    Email VARCHAR(100),
    DiaChi NVARCHAR(255),
    ChucVu NVARCHAR(50),
    Luong DECIMAL(18,2),
    MaTK INT,

    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNV),
    CONSTRAINT UQ_NhanVien_SDT UNIQUE (SoDienThoai),
    CONSTRAINT FK_NhanVien_TaiKhoan
        FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

-- =========================================
-- 3. KHÁCH HÀNG
-- =========================================
CREATE TABLE KhachHang
(
    MaKH INT IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    Email VARCHAR(100),
    DiemTichLuy INT DEFAULT 0,

    CONSTRAINT PK_KhachHang PRIMARY KEY (MaKH),
    CONSTRAINT UQ_KhachHang_SDT UNIQUE (SoDienThoai)
);
GO

-- =========================================
-- 4. KHU VỰC
-- =========================================
CREATE TABLE KhuVuc
(
    MaKhuVuc INT IDENTITY(1,1),
    TenKhuVuc NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_KhuVuc PRIMARY KEY (MaKhuVuc)
);
GO

-- =========================================
-- 5. BÀN
-- =========================================
CREATE TABLE Ban
(
    MaBan INT IDENTITY(1,1),
    TenBan NVARCHAR(50) NOT NULL,
    SoChoNgoi INT NOT NULL,

    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Trống',
    MaKhuVuc INT,

    CONSTRAINT PK_Ban PRIMARY KEY (MaBan),

    CONSTRAINT CK_Ban_SoChoNgoi CHECK (SoChoNgoi > 0),

    CONSTRAINT CK_Ban_TrangThai CHECK
    (
        TrangThai IN (N'Trống', N'Đang có khách', N'Đã đặt')
    ),

    CONSTRAINT FK_Ban_KhuVuc
        FOREIGN KEY (MaKhuVuc) REFERENCES KhuVuc(MaKhuVuc)
);
GO

-- =========================================
-- 6. LOẠI MÓN
-- =========================================
CREATE TABLE LoaiMon
(
    MaLoai INT IDENTITY(1,1),
    TenLoai NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_LoaiMon PRIMARY KEY (MaLoai)
);
GO

-- =========================================
-- 7. MÓN ĂN
-- =========================================
CREATE TABLE MonAn
(
    MaMon INT IDENTITY(1,1),
    TenMon NVARCHAR(100) NOT NULL,
    Gia DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(30) DEFAULT N'Còn món',
    HinhAnh NVARCHAR(255),
    MoTa NVARCHAR(500),
    MaLoai INT,

    CONSTRAINT PK_MonAn PRIMARY KEY (MaMon),

    CONSTRAINT CK_MonAn_Gia CHECK (Gia >= 0),

    CONSTRAINT CK_MonAn_TrangThai CHECK
    (
        TrangThai IN (N'Còn món', N'Hết món')
    ),

    CONSTRAINT FK_MonAn_LoaiMon
        FOREIGN KEY (MaLoai) REFERENCES LoaiMon(MaLoai)
);
GO

-- =========================================
-- 8. ĐẶT BÀN
-- =========================================
CREATE TABLE DatBan
(
    MaDatBan INT IDENTITY(1,1),
    MaKH INT,
    MaBan INT NOT NULL,
    SoLuongKhach INT NOT NULL,
    ThoiGianDat DATETIME NOT NULL,
    GhiChu NVARCHAR(255),
    TrangThai NVARCHAR(30) DEFAULT N'Chờ xác nhận',
    NgayTao DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_DatBan PRIMARY KEY (MaDatBan),

    CONSTRAINT CK_DatBan_SoLuong CHECK (SoLuongKhach > 0),

    CONSTRAINT CK_DatBan_TrangThai CHECK
    (
        TrangThai IN
        (
            N'Chờ xác nhận',
            N'Đã xác nhận',
            N'Đã hủy',
            N'Hoàn thành'
        )
    ),

    CONSTRAINT FK_DatBan_KhachHang
        FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),

    CONSTRAINT FK_DatBan_Ban
        FOREIGN KEY (MaBan) REFERENCES Ban(MaBan)
);
GO

-- =========================================
-- 9. HÓA ĐƠN
-- =========================================
CREATE TABLE HoaDon
(
    MaHD INT IDENTITY(1,1),
    MaBan INT NOT NULL,
    MaNV INT,
    MaKH INT,
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(30) DEFAULT N'Chưa thanh toán',

    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHD),

    CONSTRAINT CK_HoaDon_TongTien CHECK (TongTien >= 0),

    CONSTRAINT CK_HoaDon_TrangThai CHECK
    (
        TrangThai IN (N'Chưa thanh toán', N'Đã thanh toán', N'Đã hủy')
    ),

    CONSTRAINT FK_HoaDon_Ban FOREIGN KEY (MaBan) REFERENCES Ban(MaBan),
    CONSTRAINT FK_HoaDon_NV FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_HoaDon_KH FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO

-- =========================================
-- 10. CHI TIẾT HÓA ĐƠN
-- =========================================
CREATE TABLE ChiTietHoaDon
(
    MaCTHD INT IDENTITY(1,1),
    MaHD INT NOT NULL,
    MaMon INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    GhiChu NVARCHAR(255),

    CONSTRAINT PK_CTHD PRIMARY KEY (MaCTHD),

    CONSTRAINT CK_CTHD_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_CTHD_DonGia CHECK (DonGia >= 0),

    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    CONSTRAINT FK_CTHD_MonAn FOREIGN KEY (MaMon) REFERENCES MonAn(MaMon)
);
GO

-- =========================================
-- 11. THANH TOÁN
-- =========================================
CREATE TABLE ThanhToan
(
    MaThanhToan INT IDENTITY(1,1),
    MaHD INT NOT NULL,
    PhuongThuc NVARCHAR(50),
    SoTien DECIMAL(18,2) NOT NULL,
    NgayThanhToan DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_ThanhToan PRIMARY KEY (MaThanhToan),

    CONSTRAINT CK_ThanhToan_PhuongThuc CHECK
    (
        PhuongThuc IN (N'Tiền mặt', N'Chuyển khoản', N'Thẻ')
    ),

    CONSTRAINT CK_ThanhToan_SoTien CHECK (SoTien >= 0),

    CONSTRAINT FK_ThanhToan_HoaDon
        FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD)
);
GO


-- =========================================
-- TÀI KHOẢN
-- =========================================
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro)
VALUES 
('admin', '123456', N'Admin'),
('nhanvien1', '123456', N'NhanVien'),
('thungan1', '123456', N'ThuNgan');
GO

-- =========================================
-- NHÂN VIÊN
-- =========================================
INSERT INTO NhanVien (HoTen, GioiTinh, SoDienThoai, Email, ChucVu, Luong, MaTK)
VALUES
(N'Nguyễn Văn A', N'Nam', '0901111111', 'a@gmail.com', N'Quản lý', 15000000, 1),
(N'Trần Thị B', N'Nữ', '0902222222', 'b@gmail.com', N'Phục vụ', 8000000, 2),
(N'Lê Văn C', N'Nam', '0903333333', 'c@gmail.com', N'Thu ngân', 9000000, 3);
GO

-- =========================================
-- KHÁCH HÀNG
-- =========================================
INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiemTichLuy)
VALUES
(N'Phạm Văn D', '0911111111', 'd@gmail.com', 10),
(N'Lê Minh Nhật', '0922222222', 'nhat@gmail.com', 50),
(N'Hoàng Thị E', '0933333333', 'e@gmail.com', 0);
GO

-- =========================================
-- KHU VỰC
-- =========================================
INSERT INTO KhuVuc (TenKhuVuc)
VALUES
(N'Tầng 1'),
(N'Tầng 2'),
(N'Sân vườn');
GO

-- =========================================
-- BÀN
-- =========================================
INSERT INTO Ban (TenBan, SoChoNgoi, TrangThai, MaKhuVuc)
VALUES
(N'B01', 2, N'Trống', 1),
(N'B02', 4, N'Trống', 1),
(N'B03', 6, N'Đang có khách', 2),
(N'B04', 8, N'Đã đặt', 3),
(N'B05', 4, N'Trống', 2);
GO

-- =========================================
-- LOẠI MÓN
-- =========================================
INSERT INTO LoaiMon (TenLoai)
VALUES
(N'Món chính'),
(N'Nước uống'),
(N'Tráng miệng');
GO

-- =========================================
-- MÓN ĂN
-- =========================================
INSERT INTO MonAn (TenMon, Gia, TrangThai, MaLoai)
VALUES
(N'Pizza Hải Sản', 250000, N'Còn món', 1),
(N'Bò Steak', 320000, N'Còn món', 1),
(N'Gà nướng', 180000, N'Còn món', 1),
(N'Coca Cola', 20000, N'Còn món', 2),
(N'Trà Đào', 45000, N'Còn món', 2),
(N'Bánh Tiramisu', 60000, N'Còn món', 3);
GO

-- =========================================
-- ĐẶT BÀN
-- =========================================
INSERT INTO DatBan (MaKH, MaBan, SoLuongKhach, ThoiGianDat, GhiChu, TrangThai)
VALUES
(1, 2, 4, '2026-06-01 18:00:00', N'Đặt bàn sinh nhật', N'Đã xác nhận'),
(2, 3, 2, '2026-06-02 19:00:00', N'Ngồi ngoài trời', N'Chờ xác nhận');
GO

-- =========================================
-- HÓA ĐƠN
-- =========================================
INSERT INTO HoaDon (MaBan, MaNV, MaKH, TongTien, TrangThai)
VALUES
(3, 2, 2, 370000, N'Chưa thanh toán');
GO

-- =========================================
-- CHI TIẾT HÓA ĐƠN
-- =========================================
INSERT INTO ChiTietHoaDon (MaHD, MaMon, SoLuong, DonGia, GhiChu)
VALUES
(1, 1, 1, 250000, N'Pizza'),
(1, 4, 2, 20000, N'Nước ngọt'),
(1, 5, 1, 45000, N'Trà đào');
GO

-- =========================================
-- THANH TOÁN
-- =========================================
INSERT INTO ThanhToan (MaHD, PhuongThuc, SoTien)
VALUES
(1, N'Tiền mặt', 370000);
GO