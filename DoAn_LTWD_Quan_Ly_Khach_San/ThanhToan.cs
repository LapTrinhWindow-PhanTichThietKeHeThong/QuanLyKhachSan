﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DoAn_LTWD_Quan_Ly_Khach_San
{
    public partial class Payment : Form
    {
        string MySQLConnectionString = @"server=localhost;user id=root;password = 260601;persistsecurityinfo=True;database=ql_khach_san";
        MySqlConnection Connection; // khởi tạo chuỗi kết nối
        MySqlCommand MySqlcommand;  // khởi tạo biến thực hiện
        MySqlDataAdapter adapter = new MySqlDataAdapter();  // khởi tạo vùng lưu trữ hiện hành
        DataTable table = new DataTable(); // khởi tạo một bẳng để lưu trữ các giá trị
        int ans,sum;
        public Payment()
        {
            InitializeComponent();
        }
        private void ThanhToanLoad()
        {
            Connection.Open();
            cbMaPhong.Items.Clear();
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandType = CommandType.Text;
            MySqlcommand.CommandText = "select maphong from phong ";
            MySqlcommand.ExecuteNonQuery();
            DataTable dt = new DataTable();
            adapter = new MySqlDataAdapter(MySqlcommand);
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                cbMaPhong.Items.Add(dr["maphong"].ToString());
            }
            Connection.Close();
        }
        private void ThanhToan_Load(object sender, EventArgs e)
        {
            Connection = new MySqlConnection(MySQLConnectionString);
            showdata();
            ThanhToanLoad();
        }
        void showdata()
        {
            Connection.Open();
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "select* from khachhang where TinhtrangTT='Chưa Thanh Toán'";
             adapter.SelectCommand = MySqlcommand;
            table.Clear();
            adapter.Fill(table);
            dgShowKhachHang.DataSource = table;
            Connection.Close();
        }


        private void btnTimKiemTheoMaPhong_Click(object sender, EventArgs e)
        {
            Connection.Open();
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "select* from khachhang where maphong = '" + cbMaPhong.Text + "'";
            adapter.SelectCommand = MySqlcommand;
            table.Clear();
            adapter.Fill(table);
            dgShowKhachHang.DataSource = table;
            ans = dgShowKhachHang.CurrentRow.Index;
            txtHoTen.Text = dgShowKhachHang.Rows[ans].Cells[1].Value.ToString();
            cbMaPhong.Text = dgShowKhachHang.Rows[ans].Cells[4].Value.ToString();
            Connection.Close();
        }

        private void btnTimKiemTheoHoTen_Click(object sender, EventArgs e)
        {
            Connection.Open();
            MySqlcommand = Connection.CreateCommand();
            // câu lệnh truy vấn
            MySqlcommand.CommandText = "select* from khachhang where hoten = '" + txtHoTen.Text + "'";
            // gọi hàm thực hiện các yêu cầu
            adapter.SelectCommand = MySqlcommand;
            table.Clear();
            adapter.Fill(table);
            dgShowKhachHang.DataSource = table;
            ans = dgShowKhachHang.CurrentRow.Index;
            txtHoTen.Text = dgShowKhachHang.Rows[ans].Cells[1].Value.ToString();
            cbMaPhong.Text = dgShowKhachHang.Rows[ans].Cells[4].Value.ToString();
            Connection.Close();
        }
        string Mysql;
        MySqlDataReader Read_Data; // khởi tạo 1 biến để đọc giá trị
        int i = 0;
        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            Connection.Open();
            Mysql = "Select khachhang.makh,ngayden,PHONG.MAPHONG,KHACHHANG.HOTEN,datediff(curdate() , khachhang.ngayden), datediff(curdate(),khachhang.NGAYDEN) *phong.DONGIA FROM PHONG Inner Join KHACHHANG ON PHONG.MAPHONG = KHACHHANG.MAPHONG Where(KHACHHANG.MAPHONG = N'" + cbMaPhong.Text + @"') and(KHACHHANG.HOTEN = N'" + txtHoTen.Text + @"')";
            MySqlCommand mySqlCommand = new MySqlCommand(Mysql,Connection);
            Read_Data = mySqlCommand.ExecuteReader();
            // đọc dữ liệu từ data rồi đổ xuống cho các label để hiển thị ra màn hình thông tin của khách hàng cần thanh toán
            while(Read_Data.Read())
            {
                lbMaKH.Text = Read_Data[0].ToString();
                lbphong.Text = Read_Data[2].ToString();
                lbngay.Text = Read_Data[4].ToString();
                lbtongtien.Text = Read_Data[5].ToString() + " VND";
                i++;
                sum = Convert.ToInt32(Read_Data[5].ToString());
            }
            Read_Data.Close();
            Connection.Close();
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xin chào " + txtHoTen.Text + ". Số tiền bạn cần phải thanh toán là : " + lbtongtien.Text + " ! " + "Bạn thật sự có muốn thanh toán hay không?", "Thông Báo!", MessageBoxButtons.YesNo);
            Connection.Open();
            /// thực hiện thao tác update lại tình trạng của phòng ở bảng phòng
            MySqlcommand = Connection.CreateCommand();
            // khi chúng ta thanh toán hệ thống sẽ liên kết với cơ sở dữ liệu và cập nhật lại tình trạng của phòng trong bảng phòng
            MySqlcommand.CommandText = "update phong set tinhtrang = 'Còn Trống' where maphong = '" + cbMaPhong.Text + "'";
            adapter.SelectCommand = MySqlcommand;
            MySqlcommand.ExecuteNonQuery();
            /// thực hiện thao tác update lại tình trạng thanh toán xuông bảng khách hàng
            // tạo một chuỗi kết nối mới để thực hiện cập nhật lại tình trạng thanh toán của khách hàng trong bảng khách hàng để hiển thị cho người quản lý biết được thông tin của khách hàng đã thanh toán
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "update khachhang set tinhtrangTT = 'Đã Thanh Toán' where Makh = '" + lbMaKH.Text + "'";
            adapter.SelectCommand = MySqlcommand;
            MySqlcommand.ExecuteNonQuery();
            // xóa bản hiển thị cơ sở dữ liệu trên datagridview rồi sau đó liên kết với database cập nhật lại thông tin cho khách hàng
            table.Clear();
            adapter.Fill(table);
            dgShowKhachHang.DataSource = table;
            /// thực hiện tạo hóa đơn cho khách hàng và lưu thông tin xuống bảng thống kê giao dịch
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "insert into thongkegiaodich(makh,maphong,ngayden,ngaydi,sotiendathanhtoan) values('" + lbMaKH.Text + "','" + cbMaPhong.Text + "','" + dateNgayDen.Text + "',curdate(),'" + sum + "')";
            adapter.SelectCommand = MySqlcommand;
            MySqlcommand.ExecuteNonQuery();
            Connection.Close();
            showdata();
            clearlb();
        }

        private void dgShowKhachHang_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int ans;
            ans = dgShowKhachHang.CurrentRow.Index;
            txtHoTen.Text = dgShowKhachHang.Rows[ans].Cells[1].Value.ToString();
            dateNgayDen.Text = dgShowKhachHang.Rows[ans].Cells[3].Value.ToString();
            cbMaPhong.Text = dgShowKhachHang.Rows[ans].Cells[4].Value.ToString();
        }
        private void clearlb()
        {
            lbMaKH.Text = "";
            lbngay.Text = "";
            lbphong.Text = "";
            lbtongtien.Text = ""; 
        }

    }
}
