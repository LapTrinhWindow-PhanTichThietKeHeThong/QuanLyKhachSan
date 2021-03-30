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
    public partial class ThongTinKhachHang : Form
    {
        // khai báo một chuỗi kết nối = MySQLConnectionString
        string MySQLConnectionString = @"server=localhost;user id=root;password = 260601;persistsecurityinfo=True;database=ql_khach_san";
        MySqlConnection Connection;
        MySqlCommand MySqlcommand;
        // Khởi tạo đôi tượng có tên là adapter
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        // khởi tạo đối tượng có tên là table dùng để lưu các giá trị của bảng vào database
        DataTable table = new DataTable();
        public ThongTinKhachHang()
        {
            InitializeComponent();
        }
        public void ThongTinKhachHang_Load(object sender, EventArgs e)
        {
            // khỏi tạo giá trị có để kết nối dữ liệu với mysql
            Connection = new MySqlConnection(MySQLConnectionString);
            Connection.Open();
            ShowData();
            ThanhToanLoad();
        }
        public void ThanhToanLoad()
        {
            cbMaPhong.Items.Clear();
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandType = CommandType.Text;
            MySqlcommand.CommandText = "select maphong from phong";
            MySqlcommand.ExecuteNonQuery();
            DataTable dt = new DataTable();
            adapter = new MySqlDataAdapter(MySqlcommand);
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                cbMaPhong.Items.Add(dr["maphong"].ToString());
            }
        }

        void ShowData()
        { 
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "select* from khachhang";
            adapter.SelectCommand = MySqlcommand;
            table.Clear();
            adapter.Fill(table);
            dgShowThongTin.DataSource = table;
        }

        private void dgShowThongTin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // mạc định cho textbox MaKh chỉ được đọc chứ không được xóa hoặc sửa vì nó là khóa chính nên không có thể đụng vô đc
            txtMaKH.ReadOnly = true;
            int ans;
            // trỏ đến thành phần giá trị của thuộc tính trong datagridview để hiển thị lên phần điền thông tin khách hàng
            ans = dgShowThongTin.CurrentRow.Index;
            txtMaKH.Text = dgShowThongTin.Rows[ans].Cells[0].Value.ToString();
            txtHoTen.Text = dgShowThongTin.Rows[ans].Cells[1].Value.ToString();
            txtCMND.Text = dgShowThongTin.Rows[ans].Cells[2].Value.ToString();
            dateNgayDen.Text = dgShowThongTin.Rows[ans].Cells[3].Value.ToString();
            cbMaPhong.Text = dgShowThongTin.Rows[ans].Cells[4].Value.ToString();
            cbTinhTrangThanhToan.Text = dgShowThongTin.Rows[ans].Cells[5].Value.ToString();

        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            // khởi tạo một giá trị mới để gán dữ liệu
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "insert into khachhang values('"+ txtMaKH.Text + "', '" + txtHoTen.Text + "', '" + txtCMND.Text + "','" + dateNgayDen.Text + "', '" + cbMaPhong.Text + "','"+cbTinhTrangThanhToan.Text +"')";
            // nếu câu lệnh truy vấn sai thì sẽ báo lỗi
            MySqlcommand.ExecuteNonQuery();
            ShowData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "delete from khachhang where makh = '" + txtMaKH.Text + "'";
            MySqlcommand.ExecuteNonQuery();
            ShowData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "update khachhang set hoten = '" + txtHoTen.Text + "',socmnd = '" + txtCMND.Text + "', ngayden =  '" + dateNgayDen.Text + "', maphong = '" + cbMaPhong.Text + "',tinhtrangthanhtoan = '" +cbTinhTrangThanhToan.Text + "' where makh = '" + txtMaKH.Text + "'";
            MySqlcommand.ExecuteNonQuery();
            ShowData();
        }
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThanhToan thanhtoan = new ThanhToan();
            thanhtoan.Show();
        }

        private void tìnhTrạngPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongTinPhong Phong= new ThongTinPhong();
            Phong.Show();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn có muốn lưu thông tin khách hàng vào cơ sở dữ liệu không?", "Thông Báo!", MessageBoxButtons.YesNo);
            MySqlcommand = Connection.CreateCommand();
            MySqlcommand.CommandText = "update phong set tinhtrang = 'Đã Có Khách' where maphong = '" + cbMaPhong.Text + "'";
            adapter.SelectCommand = MySqlcommand;
            adapter.Fill(table);
        }
    }
}