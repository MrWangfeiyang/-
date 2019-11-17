using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;



namespace MyTicketSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 绑定cbo
        /// </summary>
        private void BindCbo()
        {
            DBHelper dbHelper = new DBHelper();
            //sql语句
            string sql = "select * from cityInfo";
            //适配器adapter
            SqlDataAdapter adapter = new SqlDataAdapter(sql,dbHelper.Connecion);
            //数据集
            DataSet ds = new DataSet();
           // 填充数据集
            adapter.Fill(ds, "cityInfo");
            //新的一行
            DataRow row = ds.Tables["cityInfo"].NewRow();
            row[0] = -1;
            row[1] = "请选择";
            //插入
            ds.Tables["cityInfo"].Rows.InsertAt(row, 0);
         
            //获取视图
            DataView div1 = new DataView(ds.Tables["cityInfo"]);
            DataView div2 = new DataView(ds.Tables["cityInfo"]);
            //绑定数据源
            this.cboDestination.ItemsSource = div1;
            this.cboDestination.DisplayMemberPath = "cityName";
            this.cboDestination.SelectedValuePath = "id";
            this.cboDestination.SelectedIndex = 0;
            this.cboLeaveCity.ItemsSource = div1;
            this.cboLeaveCity.DisplayMemberPath = "cityName";
            this.cboLeaveCity.SelectedValuePath = "id";
            this.cboLeaveCity.SelectedIndex = 0;

        }
        private void BingDg()
        {
            DBHelper dbHelper = new DBHelper();
            string sql = string.Format(@"select  flightNo,airways,leaveTime,landTime,price
                                       from flightInfo,airwaysInfo
                                       where flightInfo.airwaysId=airwaysInfo.id
                                       and leaveCity={0}
                                       and destination={1}", cboLeaveCity.SelectedValue,cboDestination.SelectedValue);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, dbHelper.Connecion);
            //数据集
            DataSet ds = new DataSet();
            adapter.Fill(ds, "flightInfo");
            this.dataGrid.ItemsSource = ds.Tables["flightInfo"].DefaultView;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindCbo();
        }

        private void But_select_Click(object sender, RoutedEventArgs e)
        {
            if(cboDestination.Text=="请选择"||cboLeaveCity.Text=="请选择")
            {
                MessageBox.Show("请选择出发地和目的地！", "提示");
                dataGrid.ItemsSource = null;
                return;
            }
            BingDg();
            txt_flihtNo.Text = "";
            textBox_Copy3.Text = "";
            txt_leaveTime.Text = "";
            txt_desTime.Text = "";
            txt_price.Text = "";
            txt_leave.Text = "";
            txt_destination.Text = "";

        }

        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(dataGrid.ItemsSource!=null)
            {
                var a = this.dataGrid.SelectedItem;

                var b = a as DataRowView;
                txt_flihtNo.Text = b["flightNo"].ToString();
                textBox_Copy3.Text= b["airways"].ToString();
                txt_leaveTime.Text = b["leaveTime"].ToString();
                txt_desTime.Text = b["landTime"].ToString();
                txt_price.Text = b["price"].ToString();
                txt_leave.Text = cboLeaveCity.Text;
                txt_destination.Text = cboDestination.Text;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           //Person p = this.dataGrid.SelectedItem as Person;
        }
        int values = 1;
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            values++;
            txt_quantity.Text = values.ToString();
        }

        private void Button1_Copy_Click(object sender, RoutedEventArgs e)
        {

            if (values != 1)
            {
                values--;
                txt_quantity.Text = values.ToString();
            }
            else
            {
                return;
            }
                
            
           
        }

        private void Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();//Close()函数只能关闭当前窗体。如果要关闭当前程序的所有窗体
        }
    /// <summary>
    /// 预定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                Random r = new Random();
                int orderId= r.Next(100000, 9999999);
                string flightNo = txt_flihtNo.Text;
                string leaveData = time.Text;
                string Number = txt_quantity.Text;
                string sql = string.Format("insert into orderInfo(orderId,flightNo,leaveData,Number) values({0},'{1}','{2}',{3})", orderId, flightNo, leaveData, Number);
                DBHelper dbHelper = new DBHelper();
                try
                {
                    SqlCommand cmd = new SqlCommand(sql,dbHelper.Connecion);
                    dbHelper.OpenConnection();
                    int result= cmd.ExecuteNonQuery();
                    if(result>0)
                    {
                        MessageBox.Show("预定成功,订单编号为" + orderId);
                    }
                    else
                    {
                        MessageBox.Show("预定失败,请重试下");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("发生错误，请联系管理员  错误的信息为 "+ex.Message);
                }
                finally
                {
                    dbHelper.CloseConnection();
                }
            }
        }
        private bool ValidateInput()
        {
            if (txt_flihtNo.Text == string.Empty)
            {
                MessageBox.Show("请选择一个航班");
                return false;
            }
            if (time.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("请选择一个正确的出发时间");
                time.Focus();
                return false;
            }
            return true;
        }
    }
}
