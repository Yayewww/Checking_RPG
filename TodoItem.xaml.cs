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

namespace imnoteeaa
{
    /// <summary>
    /// TodoItem.xaml 的互動邏輯
    /// </summary>
    public partial class TodoItem : UserControl
    {

        public MainWindow w;

        //打勾狀態
        public bool IsChecked
        {
            set
            {
                if (value == true)
                {
                    CheckMark.Visibility = Visibility.Visible;
                }
                else
                {
                    CheckMark.Visibility = Visibility.Collapsed;
                }
            }
            get
            {
                if (CheckMark.Visibility == Visibility)
                    return true;
                else
                    return false;
            }
        }

        public TodoItem()
        {
            InitializeComponent();
        }

        private void CheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CheckMark.Visibility == Visibility)
            {
                CheckMark.Visibility = Visibility.Collapsed;
                w.nowTarget -= 1;
                w.Target.Text = "目標 : " + w.nowTarget + "/" + w.maxTarget;
            }
            else
            {
                CheckMark.Visibility = Visibility.Visible;
                w.nowTarget += 1;
                w.Target.Text = "目標 : " + w.nowTarget + "/" + w.maxTarget;
            }
        }

        private void Contents_Box_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(Contents_Box.Text == "" && e.Key == Key.Back)
            {
                w.Destroy_item(this);
            }

        }
    }
}
