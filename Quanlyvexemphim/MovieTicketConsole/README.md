# CHƯƠNG TRÌNH QUẢN LÝ VÉ XEM PHIM
Dự án môn học lập trình C# Console — UEH 2025  
---
## Giới thiệu
Chương trình Quản lý vé xem phim là ứng dụng console được viết bằng ngôn ngữ C#.  
Mục tiêu của chương trình là mô phỏng hệ thống quản lý vé cơ bản trong rạp phim, cho phép:
- Hiển thị sơ đồ ghế
- Đặt và hủy vé
- Tìm kiếm, sắp xếp và chỉnh sửa thông tin vé khách hàng
- Thống kê và xem lịch sử đặt vé  
Chương trình đáp ứng các tiêu chí kỹ thuật của môn học:
- Sử dụng biến, hàm, mảng 1 chiều và 2 chiều
- Cài đặt thuật toán sắp xếp và tìm kiếm
- Có đọc/ghi file và xử lý ngoại lệ (try-catch)
- Thể hiện rõ kỹ năng lập trình cấu trúc và hướng đối tượng cơ bản  
---
## Chức năng chính (Menu Console)
| STT | Chức năng                       | Mô tả                                                                                         |
|-----|---------------------------------|-----------------------------------------------------------------------------------------------|
| 1   | Hiển thị sơ đồ ghế              | In ra ma trận ghế (mảng 2 chiều), đánh dấu ghế trống và ghế đã đặt                            |
| 2   | Đặt vé                          | Người dùng nhập tên khách, chọn vị trí ghế → chương trình kiểm tra hợp lệ và lưu thông tin vé |
| 3   | Hủy vé                          | Nhập tên khách hoặc vị trí ghế để hủy vé tương ứng                                            |
| 4   | Thống kê                        | Thống kê số vé đã bán, số ghế trống, tổng doanh thu                                           |
| 5   | Xem lịch sử đặt vé              | Hiển thị toàn bộ danh sách khách hàng đã đặt vé                                               |
| 6   | Tìm vé theo tên khách           | Tìm kiếm tuyến tính (Linear Search) theo tên khách hàng                                       |
| 7   | Sắp xếp danh sách khách (A → Z) | Sắp xếp danh sách khách theo tên bằng thuật toán Bubble Sort                                  |
| 8   | Sửa vé khách hàng               | Cập nhật thông tin khách hoặc vị trí ghế đã đặt                                               |
| 0   | Thoát chương trình              | Lưu dữ liệu, kết thúc an toàn |
---
## Cấu trúc chương trình
MovieTicketConsole/
┣ Dependencies/ # Thư viện bên ngoài
┣ Program.cs # Chứa hàm Main menu chính và các hàm
┗ README.md # Tài liệu mô tả chương trình
---
## Thuật toán và kỹ thuật sử dụng
### Mảng
- Mảng 2 chiều: lưu sơ đồ ghế trong rạp  
  `seatMap[row, col] = true/false` (đã đặt hoặc trống)
- Mảng 1 chiều: quản lý danh sách vé khách hàng  
### Thuật toán
- Sắp xếp (Bubble Sort): sắp xếp khách hàng theo tên (A → Z)
- Tìm kiếm (Linear Search): tìm vé theo tên khách hàng  
### Hàm và biến
- Có hàm nạp chồng (overloading), ref/out, tham số mặc định
- Dùng const để định nghĩa số hàng, số cột ghế
- Thể hiện rõ biến cục bộ và biến thành viên
### File I/O
- Ghi và đọc dữ liệu vé từ file tickets.txt
- Kiểm tra file tồn tại trước khi ghi
- Dùng try-catch xử lý ngoại lệ
- Tạo file backup (.bak) trước khi ghi đè dữ liệu
---
## Flowchart tổng quan
```text
┌───────────────────────────┐
│      BẮT ĐẦU CHƯƠNG TRÌNH │
└─────────────┬─────────────┘
              ▼
     ┌─────────────────────┐
     │     HIỂN THỊ MENU   │
     └─────────┬───────────┘
               ▼
     ┌─────────────────────┐
     │   NGƯỜI DÙNG CHỌN   │
     │     CHỨC NĂNG       │
     └─────────┬───────────┘
               ▼
     ┌─────────────────────┐
     │  XỬ LÝ TƯƠNG ỨNG    │
     │  (Đặt/Hủy/Tìm/...)  │
     └─────────┬───────────┘
               ▼
     ┌─────────────────────┐
     │CẬP NHẬT FILE DỮ LIỆU│
     └─────────┬───────────┘
               ▼
     ┌─────────────────────┐
     │  QUAY LẠI MENU CHÍNH│
     └─────────┬───────────┘
               ▼
     ┌─────────────────────┐
     │  THOÁT (Lưu dữ liệu)│
     └─────────┬───────────┘
               ▼
        ┌────────────────┐
        │    KẾT THÚC    │
        └────────────────┘
---
## Hướng dẫn sử dụng
1. Tải dự án từ file hoặc Github.
2. Mở dự án trong Visual Studio.
3. Biên dịch và chạy chương trình.
4. Sử dụng menu console để tương tác với hệ thống quản lý vé.
5. Dữ liệu vé sẽ được lưu tự động vào file tickets.txt khi thoát chương trình.
---
## Nhóm tác giả
- Nhóm sinh viên Nhóm 1 - Lớp CSLT K50 25C1INF50900504 - Đại học Kinh tế TP.HCM (UEH)
- Giảng viên hướng dẫn: TS. Nguyễn Thành Huy
- Danh sách thành viên:
| STT | Họ và tên             | MSSV         |
|-----|-----------------------|--------------|
| 1   | Nguyễn Thị Hoài Thương| 31241026283  |
| 2   | Trần Thị Bảo Ngọc     | 31241025665  |
| 3   | Lữ Võ Hoàng Phúc      | 31241025389  |
| 4   | Dương Thúy Hiền       | 31241024221  |
| 5   | Phạm Hoài Thương      | 31241025110  |
---


