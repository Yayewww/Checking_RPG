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
using System.IO; //直接抓IO 省code
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace imnoteeaa
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //創建圖片盒子
        BitmapImage openedBitmap;
        //創建item的List
        List<TodoItem> items = new List<TodoItem>();
        //創建圖片的路徑
        public string filePath = "";
        //創建稱號的內容
        string[] myTitles = { "小廢物", "養成習慣中", "瑣事小達人", "解每日是一種態度","Deadline Driven","學園都市 Lv.6" };
        //宣告經驗值最大值
        int nowExp = 0;
        int MaxExp = 10000;
        //宣告目標數最大值
        public int nowTarget = 0;
        public int maxTarget = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // 建立元件
            TodoItem item = new TodoItem();
            item.w = this;
            // 放入到 StackPanel
            TaskPanel.Children.Add(item);
            items.Add(item);
            //目標最大值+1
            maxTarget += 1;
            Target.Text = "目標 : " + nowTarget + "/" + maxTarget;
        }


        //結算按鈕
        private void Finish_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //經驗值加總有打勾的項目，並清除全部
            foreach (TodoItem t in items.ToArray()) //網路上找的ToArray神做法，不懂原理。 用了就解決了 (跪)
            {
                items.Remove(t);
                TaskPanel.Children.Remove(t);
                if (t.IsChecked == true)
                nowExp += 100;
            }

            //清除目標
            nowTarget = 0;
            maxTarget = 0;

            //更新字串
            string Status = nowExp + "/" + MaxExp;
            exp.Text = Status;
            Target.Text = "目標 : " + nowTarget + "/" + maxTarget;
            TitleColor();
            

   

        }

        void TitleColor()
        {
            //稱號判斷式
            if (nowExp <= 1000)
            {
                myTitle.Text = myTitles[0];
                myTitle.Foreground = Brushes.Gray;
            }
            else if (nowExp <= 2000 && nowExp > 1000)
            {
                myTitle.Text = myTitles[1];
                myTitle.Foreground = Brushes.DarkCyan;
            }
            else if (nowExp <= 3000 && nowExp > 2000)
            {
                myTitle.Text = myTitles[2];
                myTitle.Foreground = Brushes.SteelBlue;
            }
            else if (nowExp <= 4000 && nowExp > 3000)
            {
                myTitle.Text = myTitles[3];
                myTitle.Foreground = Brushes.DarkCyan;
            }
            else if (nowExp <= 5000 && nowExp > 4000)
            {
                myTitle.Text = myTitles[4];
                myTitle.Foreground = Brushes.Tomato;
            }
            else if (nowExp > 5000)
            {
                myTitle.Text = myTitles[5];
                myTitle.Foreground = Brushes.Yellow;
            }
        }

        public void Destroy_item(TodoItem item)
        {
            //砍掉當前個體
            items.Remove(item);
            TaskPanel.Children.Remove(item);
        }

        #region Datas存取
        // 關閉視窗的事件
        private void Window_Closed(object sender, EventArgs e)
        {
            List<String> datas = new List<String>();
            foreach (TodoItem t in items)
            {
                string data = "";

                //打勾符號
                if (t.IsChecked == true)
                    data += "+";
                else
                    data += "-";

                //文字
                data += "|" + t.Contents_Box.Text;   
                //放到List中
                datas.Add(data);
            }
            //內容存到以下路徑
            File.WriteAllLines(@"C:\Daily_Data\data.txt", datas);

            //儲存圖片路徑
            if(img.Source !=null)
            {
                File.WriteAllText(@"C:\Daily_Data\imgData.txt", filePath);
            }
            //儲存使用者資料
            string[] userData = {   myName.Text,    myTitle.Text,  exp.Text , Target.Text , nowTarget.ToString() ,maxTarget.ToString() };
            File.WriteAllLines(@"C:\Daily_Data\userData.txt", userData);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //讀當前日期
            DateTime dt = DateTime.Now;
            Date.Text = dt.Month + "/" + dt.Day;

            //如果檔案存在，就讀進來
            if (File.Exists(@"C:\Daily_Data\data.txt"))
            {
                string[] datas = File.ReadAllLines(@"C:\Daily_Data\data.txt");
                
                //個別產生項目
                foreach (string data in datas)
                {
                    //符號分割
                    string[] parts = data.Split('|');

                    //建立元件
                    TodoItem item = new TodoItem();
                    item.Contents_Box.Text = parts[1];
                    //打勾符號
                    if (parts[0] == "+")
                        item.IsChecked = true;
                    else
                        item.IsChecked = false;

                    item.w = this;//將此個體指向該類別
                    //放到List中
                    items.Add(item);
                    //放到Panel
                    TaskPanel.Children.Add(item);
                }
            }

            //如果圖片路徑存在，讀取圖片
            if(File.Exists(@"C:\Daily_Data\imgData.txt"))
            {
                string path = File.ReadAllText(@"C:\Daily_Data\imgData.txt");
                //路徑不是空的，讀圖
                if(path !="")
                openedBitmap = new BitmapImage(new Uri(path));
                img.Source = openedBitmap;
                //關掉Load_text
                Load_text.Visibility = Visibility.Collapsed;
                filePath = path;
            }

            //讀使用者Data
            if(File.Exists(@"C:\Daily_Data\userData.txt"))
            {
                string[] userData = File.ReadAllLines(@"C:\Daily_Data\userData.txt");
                myName.Text = userData[0];
                myTitle.Text = userData[1];
                exp.Text = userData[2];
                Target.Text = userData[3];
                //讀目標值
                nowTarget = int.Parse(userData[4]);
                maxTarget = int.Parse(userData[5]);
                //讀exp值
                string[] parts = userData[2].Split('/');
                nowExp = int.Parse(parts[0]);
                TitleColor();
            }
        }


        //讀圖片
        private void Load_img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files(*.jpg) | *.jpg";

            // 顯示視窗
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // 取得檔案路徑 
                filePath = dlg.FileName;
                openedBitmap = new BitmapImage(new Uri(filePath));
                img.Source = openedBitmap;
                //關掉Load_text
                Load_text.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region 視窗操作
        private void Tops_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}