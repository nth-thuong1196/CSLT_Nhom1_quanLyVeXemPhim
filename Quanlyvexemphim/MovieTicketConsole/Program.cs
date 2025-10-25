using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CinemaTicket
{
    class Customer
    {
        public string Name;   // Tên khách
        public string PhoneLast4; // 4 số cuối điện thoại
        public int Row;       // Hàng ghế
        public int Col;       // Cột ghế
        public double Price;  // Giá vé
    }
    enum SeatStatus
    {
        Empty = 0,    // Ghế trống
        Booked = 1,   // Ghế đã đặt
        Reserved = 2  // Ghế tạm giữ
    }
    class Program
    {
        const int ROWS = 10;
        const int COLS = 20;
        const double TICKET_PRICE = 50000;

        static SeatStatus[,] seats = new SeatStatus[ROWS, COLS];
        static List<Customer> customers = new List<Customer>();
        static int soldSeats = 0;
        static double revenue = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LoadCustomers();
            ShowWelcome();

            bool isRunning = true; // biến bool điều khiển chương trình

            while (isRunning)
            {
                ShowMenu();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nNhập lựa chọn: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) //nếu nhập đúng thì lưu vào choice, nếu sai thì báo lỗi
                {
                    Console.WriteLine("Vui lòng nhập số!");
                    continue;
                }

                switch (choice)
                {
                    case 1: ShowSeats(); break;
                    case 2: BookTicket(); break;
                    case 3: CancelTicket(); break;
                    case 4: ShowStatistic(); break;
                    case 5: ShowHistory(); break;
                    case 6: SearchTicketByName(); break;
                    case 7: SortCustomersByName(); break;
                    case 8: EditTicket(); break;
                    case 0:
                        Console.WriteLine("Thoát chương trình...");
                        SaveCustomers();
                        isRunning = false; // dừng vòng lặp thay vì while(choice != 0)
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // ====== GIAO DIỆN ======
        static void ShowWelcome()
        {
            SnowEffect(4000, 70, 20); // Hiệu ứng tuyết rơi
            Console.ForegroundColor = ConsoleColor.Cyan; // Màu xanh dương cho tiêu đề
            Console.WriteLine(@"
██╗    ██╗ ███████╗ ██╗  ██╗    ███████╗ ██████╗ ███████╗███████╗
██║    ██║ ██╔════╝ ██║  ██║    ╚════██║██╔═══██╗╚════██║██╔════╝
██║    ██║ █████╗   ███████║    ███████║██║   ██║███████║███████╗
██║    ██║ ██╔══╝   ██║  ██║    ██╔════╝██║   ██║██╔════╝╚════██║
 ╚█████╔═╝ ███████╗ ██║  ██║    ███████╗╚██████╔╝███████╗███████║
  ╚════╝   ╚══════╝ ╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝╚══════╝
");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
 ██████╗██╗███╗   ██╗███████╗███╗   ███╗ █████╗ 
██╔════╝██║████╗  ██║██╔════╝████╗ ████║██╔══██╗
██║     ██║██╔██╗ ██║█████╗  ██╔████╔██║███████║
██║     ██║██║╚██╗██║██╔══╝  ██║╚██╔╝██║██╔══██║
╚██████╗██║██║ ╚████║███████╗██║ ╚═╝ ██║██║  ██║
 ╚═════╝╚═╝╚═╝  ╚═══╝╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝
");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
 ███╗   ██╗██╗  ██╗ ██████╗ ███╗   ███╗     ██╗
 ████╗  ██║██║  ██║██╔═══██╗████╗ ████║     ██║
 ██╔██╗ ██║███████║██║   ██║██╔████╔██║     ██║
 ██║╚██╗██║██╔══██║██║   ██║██║╚██╔╝██║     ██║
 ██║ ╚████║██║  ██║╚██████╔╝██║ ╚═╝ ██║     ██║ 
 ╚═╝  ╚═══╝╚═╝  ╚═╝ ╚═════╝ ╚═╝     ╚═╝     ╚═╝
");
            Console.ResetColor(); // Đặt lại màu mặc định để mấy phần sau không bị ảnh hưởng
            Console.ForegroundColor = ConsoleColor.Yellow; // Màu vàng cho dòng phụ đề
            Console.WriteLine("         🎬 CINEMA TICKET MANAGEMENT 🎬");
            Console.ResetColor(); // Đặt lại màu mặc định
            Thread.Sleep(2000); // Tạm dừng 1 giây để người dùng kịp nhìn
            SmoothClear(); //thực hiện hàm SmoothClear
        }

        static void DrawHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string line = new string('═', title.Length + 6); // +6 để có khoảng trống hai bên
            Console.WriteLine($"╔{line}╗"); // Vẽ đường viền trên
            Console.WriteLine($"║   {title}   ║");  // Vẽ tiêu đề ở giữa
            Console.WriteLine($"╚{line}╝");  // Vẽ đường viền dưới
            Console.ResetColor();
        }

        static void SmoothClear()
        {
            for (int i = 0; i < 3; i++) // Hiệu ứng chấm chấm
            {
                Console.Write(".");
                Thread.Sleep(500); //in ra mỗi dấu chấm là thêm 500ms
            }
            Console.Clear(); //xóa màn hình
        }

        static void ShowMenu() //Giao diện menu
        {
            Console.Clear();
            DrawHeader("🎟️  MENU QUẢN LÝ VÉ RẠP  🎟️"); // Vẽ tiêu đề menu
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. Hiển thị sơ đồ ghế");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("2. Đặt vé");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Hủy vé");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("4. Thống kê");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("5. Xem lịch sử đặt vé");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("6. Tìm vé theo tên khách");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("7. Sắp xếp danh sách khách theo tên (A → Z)");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("8. Sửa vé khách hàng");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("0. Thoát");
            Console.ResetColor();
            Console.WriteLine("\n───────────────────────────────────────");
        }

        // ====== HIỂN THỊ GHẾ ======
        static void ShowSeats(bool wait = true)
        {
            Console.Clear();
            DrawHeader("SƠ ĐỒ GHẾ RẠP");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n                       🎥  MÀN HÌNH  🎥");
            Console.ResetColor();

            Console.Write("     ");
            for (int j = 0; j < COLS; j++)
                Console.Write("{0,3}", j + 1);
            Console.WriteLine();

            for (int i = 0; i < ROWS; i++)
            {
                Console.Write($" {GetRowLetter(i),2}: ");
                for (int j = 0; j < COLS; j++)
                {
                    if (seats[i, j] == SeatStatus.Empty)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" ☐ ");
                    }
                    else if (seats[i, j] == SeatStatus.Booked)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" ☒ ");
                    }

                }
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.WriteLine("\nChú thích: ☐ = Ghế trống, ☒ = Ghế đã đặt");
            if (wait) WaitAndClear(); //sử dụng bool
        }
        static void ShowSeatsOnly() //khác ở hàm trên là không chờ
        {
            ShowSeats(false);
        }


        // ====== ĐẶT VÉ ======
        static void BookTicket()
        {
            ShowSeatsOnly();
            Console.WriteLine("\n=== ĐẶT VÉ ===");

            Console.Write("Bạn đã đặt vé online chưa? (Nếu có, nhập 'y', nếu chưa nhập 'n'): ");

            string online = Console.ReadLine().ToLower(); //chuyển về chữ thường để dễ so sánh

            if (online == "y")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("→ Xác nhận: Khách hàng đã đặt vé online từ trước. Vui lòng nhập thông tin để nhận ghế.");
                Console.ResetColor();

                Console.Write("Số lượng vé: ");
                if (!int.TryParse(Console.ReadLine(), out int soVe))
                {
                    Console.WriteLine("Sai dữ liệu!"); return;
                }

                for (int i = 0; i < soVe; i++)
                {
                    Console.WriteLine($"Vé thứ {i + 1}:");
                    BookSingleTicket(); // GỌI HÀM NẠP CHỒNG
                }
            }
            else
            {
                Console.Write("Nhập tên khách: ");
                string name = Console.ReadLine();
                Console.Write("Nhập 4 số cuối điện thoại: ");
                string phoneLast4 = Console.ReadLine();

                if (phoneLast4.Length != 4 || !int.TryParse(phoneLast4, out _))
                {
                    Console.WriteLine("❌ Vui lòng nhập đúng 4 chữ số!");
                    return;
                }

                Console.WriteLine("Giá vé: {0} VND", TICKET_PRICE);
                Console.Write("Số lượng vé: ");
                if (!int.TryParse(Console.ReadLine(), out int soVe))
                {
                    Console.WriteLine("Sai dữ liệu!"); return;
                }

                for (int i = 0; i < soVe; i++)
                {
                    Console.WriteLine($"Vé thứ {i + 1}:");
                    BookSingleTicket(name, phoneLast4); // GỌI HÀM GỐC (có tên)
                }
            }

            WaitAndClear();
        }


        static void BookSingleTicket(string name, string phoneLast4)
        {
            Console.Write("Nhập hàng ghế (A-{0}): ", GetRowLetter(ROWS - 1));
            string rowInput = Console.ReadLine().ToUpper();
            int row = GetRowIndexFromLetter(rowInput);
            if (row < 0 || row >= ROWS)
            {
                Console.WriteLine("Hàng không hợp lệ!");
                return;
            }

            Console.Write("Nhập số cột (1-{0}): ", COLS);
            if (!int.TryParse(Console.ReadLine(), out int col))
            {
                Console.WriteLine("Sai dữ liệu!");
                return;
            }
            col--;

            if (row < 0 || row >= ROWS || col < 0 || col >= COLS)
            {
                Console.WriteLine("Ghế không hợp lệ!");
                return;
            }
            // Kiểm tra trùng thông tin khách trước
            if (customers.Exists(c =>
                c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                c.PhoneLast4 == phoneLast4 &&
                c.Row == row + 1 && c.Col == col + 1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Khách {name} ({phoneLast4}) đã đặt ghế ({row + 1},{col + 1}) rồi!");
                Console.ResetColor();
                return;
            }

            // Sau đó mới kiểm tra ghế có người khác đặt chưa
            if (seats[row, col] == SeatStatus.Booked)
            {
                Console.WriteLine("❌ Ghế đã có người khác đặt!");
                return;
            }
            seats[row, col] = SeatStatus.Booked;
            soldSeats++; // Tăng số ghế đã bán
            revenue += TICKET_PRICE; // Cộng doanh thu
            // Thêm khách hàng vào danh sách

            Customer c = new Customer
            {
                Name = name,
                PhoneLast4 = phoneLast4,
                Row = row + 1,
                Col = col + 1,
                Price = TICKET_PRICE
            }; // Tạo struct Customer
            customers.Add(c); // Thêm khách hàng vào danh sách dựa vào struct Customer

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($">> Đặt vé thành công cho {name} tại ghế ({GetRowLetter(row)}, {col + 1})");
            Console.ResetColor();

            File.AppendAllText("history.txt",
            $"[{DateTime.Now}] ĐẶT VÉ: {name} ({phoneLast4}) - Ghế ({GetRowLetter(row)}, {col + 1}) - Giá {c.Price:N0} VND\n");

            // Ghi vào lịch sử

            SaveCustomers(); // Lưu khách hàng sau khi đặt vé
        }

        // ====== HÀM NẠP CHỒNG: ĐẶT VÉ TỪ APP VÉ ONLINE ======
        static void BookSingleTicket()
        {
            // Gọi lại bản gốc, dùng tên mặc định "Khách lẻ"
            BookSingleTicket("Khách lẻ", "0000");
        }

        // ====== HỦY VÉ ======
        static void CancelTicket()
        {
            ShowSeatsOnly(); // Hiển thị sơ đồ ghế không chờ
            Console.WriteLine("\n=== HỦY VÉ ==="); // Tiêu đề hủy vé

            Console.Write("Nhập hàng ghế (A-{0}): ", GetRowLetter(ROWS - 1));
            string rowInput = Console.ReadLine().ToUpper();
            int row = GetRowIndexFromLetter(rowInput);
            if (row < 0 || row >= ROWS)
            {
                Console.WriteLine("Hàng không hợp lệ!");
                return;
            }

            Console.Write("Nhập số cột (1-{0}): ", COLS);
            if (!int.TryParse(Console.ReadLine(), out int col))
            {
                Console.WriteLine("Sai dữ liệu!");
                return;
            }
            col--;

            int index = customers.FindIndex(c => c.Row == row + 1 && c.Col == col + 1); // Tìm khách hàng theo ghế
            if (index == -1)
            {
                Console.WriteLine($"Không tìm thấy vé tại ghế ({GetRowLetter(row)}, {col + 1})!");
                return;
            }

            var customer = customers[index]; // Lấy thông tin khách hàng
            seats[row, col] = SeatStatus.Empty; // Đánh dấu ghế trống lại
            soldSeats--; // Giảm số ghế đã bán
            revenue -= customer.Price; // Giảm doanh thu
            customers.RemoveAt(index); // Xóa khách hàng khỏi danh sách

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($">> Hủy vé thành công cho ghế ({GetRowLetter(row)}, {col + 1}).");
            Console.ResetColor();

            File.AppendAllText("history.txt",
            $"[{DateTime.Now}] HỦY VÉ: {customers[index].Name} ({customers[index].PhoneLast4}) Ghế ({GetRowLetter(customers[index].Row - 1)}, {customers[index].Col})\n");


            SaveCustomers();
            WaitAndClear();
        }
        // ====== SỬA VÉ ======
        static void EditTicket()
        {
            Console.Clear();
            DrawHeader("SỬA THÔNG TIN VÉ");

            Console.Write("Nhập tên khách: ");
            string name = Console.ReadLine();
            Console.Write("Nhập 4 số cuối điện thoại: ");
            string phoneLast4 = Console.ReadLine();

            // Tìm vé theo tên + số điện thoại
            var matches = customers.FindAll(c =>
                c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.PhoneLast4 == phoneLast4);

            if (matches.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Không tìm thấy vé khớp với thông tin trên!");
                Console.ResetColor();
                WaitAndClear();
                return;
            }

            // Nếu có nhiều vé cùng tên/số, cho người dùng chọn vé cần sửa
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n>> Tìm thấy {matches.Count} vé:");
            for (int i = 0; i < matches.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Ghế H{matches[i].Row}, C{matches[i].Col} - Giá {matches[i].Price:N0} VND");
            }
            Console.ResetColor();

            Console.Write("\nChọn vé cần sửa (nhập số thứ tự): ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > matches.Count)
            {
                Console.WriteLine("Lựa chọn không hợp lệ!");
                WaitAndClear();
                return;
            }

            Customer oldTicket = matches[choice - 1];
            int index = customers.FindIndex(c => c.Row == oldTicket.Row && c.Col == oldTicket.Col && c.PhoneLast4 == oldTicket.PhoneLast4);

            // Hỏi người dùng muốn đổi gì
            Console.WriteLine("\nBạn muốn sửa gì?");
            Console.WriteLine("1. Đổi ghế");
            Console.WriteLine("2. Đổi tên khách");
            Console.WriteLine("3. Đổi 4 số điện thoại");
            Console.WriteLine("4. Hủy (thoát)");
            Console.Write("Lựa chọn: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowSeatsOnly();
                    Console.Write("Nhập hàng ghế mới (A-{0}): ", GetRowLetter(ROWS - 1));
                    string rowInput = Console.ReadLine().ToUpper();
                    int newRow = GetRowIndexFromLetter(rowInput) + 1; // +1 vì dữ liệu khách lưu là 1-based
                    if (newRow < 1 || newRow > ROWS)
                    {
                        Console.WriteLine("Hàng không hợp lệ!");
                        return;
                    }


                    Console.Write("Nhập cột ghế mới (1-{0}): ", COLS);
                    if (!int.TryParse(Console.ReadLine(), out int newCol) || newCol < 1 || newCol > COLS)
                    {
                        Console.WriteLine("Cột không hợp lệ!");
                        return;
                    }

                    if (seats[newRow - 1, newCol - 1] == SeatStatus.Booked)
                    {
                        Console.WriteLine("❌ Ghế này đã có người đặt!");
                        return;
                    }

                    seats[oldTicket.Row - 1, oldTicket.Col - 1] = SeatStatus.Empty;
                    seats[newRow - 1, newCol - 1] = SeatStatus.Booked;
                    customers[index].Row = newRow;
                    customers[index].Col = newCol;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Đã đổi sang ghế H{newRow}, C{newCol} thành công!");
                    Console.ResetColor();
                    break;

                case "2":
                    Console.Write("Nhập tên mới: ");
                    string newName = Console.ReadLine();
                    customers[index].Name = newName;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Đã cập nhật tên thành: {newName}");
                    Console.ResetColor();
                    break;

                case "3":
                    Console.Write("Nhập 4 số điện thoại mới: ");
                    string newPhone = Console.ReadLine();
                    if (newPhone.Length != 4 || !int.TryParse(newPhone, out _))
                    {
                        Console.WriteLine("❌ Phải nhập đúng 4 chữ số!");
                        return;
                    }
                    customers[index].PhoneLast4 = newPhone;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Đã cập nhật số điện thoại thành: {newPhone}");
                    Console.ResetColor();
                    break;

                default:
                    Console.WriteLine("Đã hủy thao tác sửa vé.");
                    break;
            }

            // Ghi vào lịch sử
            File.AppendAllText("history.txt",
            $"[{DateTime.Now}] SỬA VÉ: {oldTicket.Name} ({oldTicket.PhoneLast4}) -> ({GetRowLetter(customers[index].Row - 1)}, {customers[index].Col})\n");


            SaveCustomers();
            WaitAndClear();
        }

        // ====== TÌM KIẾM KHÁCH HÀNG ======
        static bool FindCustomerByName(string name, out Customer found)
        {
            foreach (var c in customers)
            {
                if (c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    found = c; // gán dữ liệu cho biến out
                    return true; // tìm thấy
                }
            }

            found = new Customer(); // gán giá trị mặc định nếu không tìm thấy
            return false; // không tìm thấy
        }

        // ====== CHỨC NĂNG TÌM KIẾM VÉ ======
        static void SearchTicketByName()
        {
            Console.Clear();
            DrawHeader("TÌM KIẾM VÉ THEO TÊN KHÁCH");
            Console.Write("\nNhập tên khách cần tìm: ");
            string searchName = Console.ReadLine();

            var matches = customers.FindAll(c => c.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
            if (matches.Count == 0)
            {
                Console.WriteLine("\nKhông tìm thấy khách hàng này!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n>> Tìm thấy {matches.Count} kết quả:");
                Console.ResetColor();
                foreach (var r in matches)
                {
                    Console.WriteLine($"- {r.Name} | {r.PhoneLast4} | Ghế: H{r.Row}, C{r.Col} | {r.Price} VND");
                }
            }

            WaitAndClear();
        }



        // ====== THỐNG KÊ ======
        //static void ShowStatistic()
        //{
        //    Console.Clear();
        //    DrawHeader("THỐNG KÊ RẠP");
        //    int totalSeats = ROWS * COLS; // Tổng số ghế
        //    int emptySeats = totalSeats - soldSeats; // Ghế trống
        //    double occupancy = (double)soldSeats / totalSeats * 100; // Tỷ lệ lấp đầy

        //    Console.WriteLine($"Tổng số ghế: {totalSeats}");
        //    Console.WriteLine($"Đã bán: {soldSeats}");
        //    Console.WriteLine($"Còn trống: {emptySeats}");
        //    Console.WriteLine($"Tỷ lệ lấp đầy: {occupancy:F2}%");
        //    Console.WriteLine($"Doanh thu: {revenue} VND");

        //    Console.WriteLine("\nTình trạng rạp:");
        //    Console.Write("Ghế đã bán:  ");
        //    DrawProgressBar(soldSeats, totalSeats, ConsoleColor.Red); // Thanh tiến độ cho ghế đã bán
        //    Console.Write("Ghế trống:   ");
        //    DrawProgressBar(emptySeats, totalSeats, ConsoleColor.Green); // Thanh tiến độ cho ghế trống

        //    WaitAndClear();
        //}
        static void ShowStatistic()
        {
            Console.Clear();
            DrawHeader("THỐNG KÊ RẠP");
            int totalSeats = ROWS * COLS; // Tổng số ghế
            int emptySeats = totalSeats - soldSeats; // Ghế trống
            double occupancy = (double)soldSeats / totalSeats * 100; // Tỷ lệ lấp đầy

            Console.WriteLine($"Tổng số ghế: {totalSeats}");
            Console.WriteLine($"Đã bán: {soldSeats}");
            Console.WriteLine($"Còn trống: {emptySeats}");
            Console.WriteLine($"Tỷ lệ lấp đầy: {occupancy:F2}%");
            Console.WriteLine($"Doanh thu: {revenue} VND");

            Console.WriteLine("\nTình trạng rạp:");
            Console.Write("Ghế đã bán:  ");
            DrawProgressBar(soldSeats, totalSeats, ConsoleColor.Red); // Thanh tiến độ cho ghế đã bán
            Console.Write("Ghế trống:   ");
            DrawProgressBar(emptySeats, totalSeats, ConsoleColor.Green); // Thanh tiến độ cho ghế trống

            WaitAndClear();
        }

        static void DrawProgressBar(int value, int total, ConsoleColor color) // Vẽ thanh tiến độ
        {
            int width = 30;
            int filled = (int)((double)value / total * width); // Tính số phần đã lấp đầy
            Console.ForegroundColor = color;
            Console.Write("[");
            Console.Write(new string('█', filled));
            Console.Write(new string(' ', width - filled));
            Console.WriteLine("]");
            Console.ResetColor();
        }

        // ====== LỊCH SỬ ======
        static void ShowHistory()
        {
            Console.Clear();
            DrawHeader("LỊCH SỬ ĐẶT / HỦY VÉ");
            if (!File.Exists("history.txt")) { Console.WriteLine("Chưa có lịch sử!"); WaitAndClear(); return; }
            string[] history = File.ReadAllLines("history.txt");
            if (history.Length == 0) Console.WriteLine("Lịch sử rỗng!");
            else foreach (string line in history) Console.WriteLine(line);
            WaitAndClear();
        }

        // ====== HIỂN THỊ GHẾ KHÔNG CHỜ ======
        

        // ====== LƯU / NẠP KHÁCH ======
        static void SaveCustomers()
        {
            try
            {
                // tạo bản sao .bak trước khi ghi đè
                if (File.Exists("customers.txt"))
                    File.Copy("customers.txt", "customers.bak", true);

                using (StreamWriter sw = new StreamWriter("customers.txt"))
                {
                    foreach (var c in customers)
                        sw.WriteLine($"{c.Name}|{c.PhoneLast4}|{c.Row}|{c.Col}|{c.Price}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu file: " + ex.Message);
            }
        }


        static void LoadCustomers() // Nạp khách từ file
        {
            try
            {
                if (!File.Exists("customers.txt")) return;
                string[] lines = File.ReadAllLines("customers.txt");
                customers.Clear();
                soldSeats = 0;
                revenue = 0;
                Array.Clear(seats, 0, seats.Length);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length != 5) continue;

                    string name = parts[0];
                    string phone = parts[1];

                    if (!int.TryParse(parts[2], out int row)) continue;
                    if (!int.TryParse(parts[3], out int col)) continue;
                    if (!double.TryParse(parts[4], out double price)) continue;

                    Customer c = new Customer
                    {
                        Name = name,
                        PhoneLast4 = phone,
                        Row = row,
                        Col = col,
                        Price = price
                    };
                    customers.Add(c);

                    // bảo đảm chỉ đánh dấu ghế khi số hàng/cột hợp lệ
                    if (row - 1 >= 0 && row - 1 < ROWS && col - 1 >= 0 && col - 1 < COLS)
                    {
                        seats[row - 1, col - 1] = SeatStatus.Booked;
                        soldSeats++;
                        revenue += c.Price;
                    }
                    else
                    {
                        // Nếu dữ liệu sai (ghế vượt quá rạp),
                        Console.WriteLine($"Bỏ qua bản ghi không hợp lệ: {line}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi nạp dữ liệu: " + ex.Message);
                // không throw tiếp, để chương trình vẫn chạy
            }
        }


        static void WaitAndClear()
        {
            Console.WriteLine("\nNhấn Enter để quay về menu...");
            Console.ReadLine();
            SmoothClear();
        }
        static void SortCustomersByName()
        {
            if (customers.Count == 0)
            {
                Console.WriteLine("Chưa có khách nào để sắp xếp!");
                WaitAndClear();
                return;
            }

            customers.Sort((a, b) =>
                string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n>> Danh sách khách sau khi sắp xếp theo tên (A → Z):\n");
            Console.ResetColor();

            foreach (var c in customers)
            {
                Console.WriteLine($"- {c.Name,-20} | {c.PhoneLast4} | H{c.Row}, C{c.Col} | {c.Price} VND");
            }

            WaitAndClear();
        }
        static string GetRowLetter(int rowIndex)
        {
            // rowIndex = 0-based (hàng 0 -> 'A', hàng 1 -> 'B', ...)
            return ((char)('A' + rowIndex)).ToString();
        }
        static int GetRowIndexFromLetter(string letter)
        {
            if (string.IsNullOrEmpty(letter)) return -1;
            char c = char.ToUpper(letter[0]);
            return c - 'A'; // A→0, B→1, C→2, ...
        }


        // ====== HIỆU ỨNG TUYẾT RƠI ======
        static void SnowEffect(int durationMs = 4000, int width = 10, int height = 10)
        {
            Console.Clear();
            Random rnd = new Random();
            DateTime endTime = DateTime.Now.AddMilliseconds(durationMs);
            char[] flakes = { '*', '.', '❄', '❅' };
            List<(int x, int y, char c)> snow = new List<(int, int, char)>();

            while (DateTime.Now < endTime)
            {
                // Tạo hạt tuyết mới ngẫu nhiên
                if (snow.Count < 80)
                    snow.Add((rnd.Next(0, width), 0, flakes[rnd.Next(flakes.Length)]));

                // Vẽ lại toàn màn hình
                Console.SetCursorPosition(0, 0);
                char[,] screen = new char[height, width];
                foreach (var s in snow)
                {
                    if (s.y < height && s.x < width)
                        screen[s.y, s.x] = s.c;
                }

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                        Console.Write(screen[i, j] == '\0' ? ' ' : screen[i, j]);
                    Console.WriteLine();
                }

                // Cho tuyết rơi xuống
                for (int i = 0; i < snow.Count; i++)
                {
                    var s = snow[i];
                    s.y++;
                    if (s.y >= height)
                        snow[i] = (rnd.Next(0, width), 0, flakes[rnd.Next(flakes.Length)]);
                    else
                        snow[i] = s;
                }

                Thread.Sleep(100);
            }

            Console.Clear();
        }


    }
}
