using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using System.Windows.Threading;


namespace _9._11._18BttonStill
{
    public partial class MainWindow : Window 
    {
        vtClass vtc = new vtClass("GroveDatabase.accdb");
        DispatcherTimer timer , loadingTimer;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public string Pcommand;
        Thread MusicThread;
        public bool isOpen;
        int rows = 0;
        string a, b ;
        Storyboard h1;
        double last;
        void MusicControl()
        {
            DataTable dtc = vtc.veriAl("Select muzik_yolu ,id From Music");
            for (int k = 0; k < dtc.Rows.Count; k++)
            {
                if (!File.Exists((dtc.Rows[k]["muzik_yolu"]).ToString()))
                {
                    vtc.kayitSil("Music","id", dtc.Rows[k]["id"]);
                    Thread.Sleep(100);
                }
            }
        }

        void GetMusic()
        {
            DataTable dt = vtc.veriAl("Select muzik_adi From Music");
            if (rows != dt.Rows.Count)
            {
                stkP.Children.Clear();
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    Button btn = new Button();
                    btn.Cursor = Cursors.Hand;
                    btn.Content = (dt.Rows[k]["muzik_adi"]).ToString();
                    btn.Style = (Style)FindResource("btn1");
                    btn.Click += btn_music_Click;
                    stkP.Children.Add(btn);
                }
                rows = dt.Rows.Count;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //////////////////////////////
            GetMusic();
            MinHeight = 370;
            MinWidth = 525;
            stk1.Width = 50;
            search_txt.Width = 0;
            lbl1.Width = 0;
            cb1.Width = 0;
            rb1.IsChecked=true;
            stkP.Visibility = Visibility.Hidden;
            ////////////////////////////////
            btn_SP.Style = (Style)FindResource("btnStart");
            ////////////////////////////////
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
            loadingTimer = new DispatcherTimer();
            loadingTimer.Interval = TimeSpan.FromMilliseconds(1000);
            loadingTimer.Tick += new EventHandler(LoadingTimer_Tick);
            ///////////////////////////////
            MusicThread = new Thread(MusicControl);
            MusicThread.Start();
            loadingTimer.Start();
        }

        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            if (MusicThread.IsAlive)
            {
                loadingGif.Visibility = Visibility.Visible;
                btnm.IsEnabled = false;
                btnSearch.IsEnabled = false;
            }
            else
            {
                loadingGif.Visibility = Visibility.Hidden;
                btnm.IsEnabled=true;
                btnSearch.IsEnabled=true;
                loadingTimer.Stop();
            }   
        }

        public class Employee
        {
            public string vb1 { get; set; }
            public string vb2 { get; set; }

            public static Employee GetEmployee(string a ,string b)
            {
                var emp = new Employee()
                {
                    vb1 = a,
                    vb2 = b
                };
                return emp;
            }
        }
        public string list1;
        private void btn_music_Click(object sender, RoutedEventArgs e)
        {
            string a;
            a = ((Button)sender).Content.ToString();
            DataTable dtlist = vtc.veriAl("Select muzik_yolu From Music where muzik_adi='" + a + "'");
            list1 = (dtlist.Rows[0]["muzik_yolu"]).ToString();
            System.IO.FileInfo ff = new System.IO.FileInfo(@list1);
            string DosyaUzantisi = ff.Extension;
            if (DosyaUzantisi == ".mp4")
            {
                this.WindowState = WindowState.Maximized;
                stkP.Visibility = Visibility.Hidden;
            }
            me1.Source = new Uri(@list1);
            me1.Play();
            btn_SP.Style = (Style)FindResource("btnPause");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (btn_SP.Style == (Style)FindResource("btnPause"))
            {
                btn_SP.Style = (Style)FindResource("btnStart");
                me1.Pause();
            }
            else if (btn_SP.Style == (Style)FindResource("btnStart"))
            {
                btn_SP.Style = (Style)FindResource("btnPause");
                me1.Play();
            }
        }

        private void sld_ses_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            DataContext = Employee.GetEmployee(a, b); 
            Employee asd = new Employee();
            if(sld_ses.Value>0.5)
            {
                a = "Visible";
                b = "Visible";
                asd.vb1 = a;
                asd.vb2 = b;
            }
            else if (sld_ses.Value <= 0.5 && sld_ses.Value>=0.1)
            {
                
                b= "Hidden";
                a = "Visible";
                asd.vb1=a;
                asd.vb2 = b;
            }
            else if (sld_ses.Value <= 0.1)
            {
                a = "Hidden";
                b = "Hidden";
            asd.vb1= a;
            asd.vb2 = b;
            }
        }

        private void btn_Sound_Click(object sender, RoutedEventArgs e)
        {
            if (btn_Sound.Style == (Style)FindResource("btnSes"))
            {
               btn_Sound.Style = (Style)FindResource("btnSesOff");
               sld_ses.Value = 0;
            }
            else if (btn_Sound.Style == (Style)FindResource("btnSesOff"))
            {
                btn_Sound.Style = (Style)FindResource("btnSes");
                sld_ses.Value = last;
                DataContext = Employee.GetEmployee(a, b);
                Employee asd = new Employee();
                b = "Hidden";
                a = "Visible";
                asd.vb1 = a;
                asd.vb2 = b;
            }
        }

        

        bool kontorl = true;
        private void stk_Ac_Kapa_Click(object sender, RoutedEventArgs e)
        {
            if (kontorl == true)
            {
                h1 = (Storyboard)TryFindResource("cnv1");
                h1.Begin();
                kontorl = false;
            }
            else if (kontorl == false)
            {
                h1 = (Storyboard)TryFindResource("cnv2");
                h1.Begin();
                kontorl = true;
            }
            
        }


        private void search_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(search_txt.Text))
            {
                lbl1.Visibility = Visibility.Visible;
            }
            else
            { lbl1.Visibility = Visibility.Hidden; }

            char tirnak = '"';
            DataTable dt = vtc.veriAl("Select * From Music Where muzik_adi LIKE " + tirnak + "%" + search_txt.Text + "%" + tirnak);
            stkP.Children.Clear();
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                Button btn = new Button();
                btn.Cursor = Cursors.Hand;
                btn.Content = (dt.Rows[k]["muzik_adi"]).ToString();
                btn.Style = (Style)FindResource("btn1");
                btn.Click += btn_music_Click;
                stkP.Children.Add(btn);
            }
            
        }

        private void lbl1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            search_txt.Focus();
        }

        private void me1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (me1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = me1.NaturalDuration.TimeSpan;
                sld_time.Maximum = ts.TotalSeconds;
                sld_time.SmallChange = 1;
                sld_time.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            timer.Start();
        }
        bool isDragging = false;
        void timer_Tick(object sender, EventArgs e)
        {
            if(!isDragging)
            {
                sld_time.Value = me1.Position.TotalSeconds;
            }
        }
        private void sld_time_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void sld_time_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            me1.Position = TimeSpan.FromSeconds(sld_time.Value);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!MusicThread.IsAlive)
            {
                openFileDialog1.Multiselect = true;
                openFileDialog1.Filter = "Media File(*.mpg,*.dat,*.avi,*.wmv,*.wav,*.mp3,*.mp4)|*.wav;*.mp3;*.mpg;*.dat;*.avi;*.wmv;*.mp4";
                try
                {
                    openFileDialog1.ShowDialog();
                    foreach (string filename in openFileDialog1.FileNames)
                    {
                        if (!vtc.veriKontrol("Music", "muzik_yolu", System.IO.Path.GetFullPath(filename)))
                            vtc.kayitEkle("Music", System.IO.Path.GetFileNameWithoutExtension(filename), System.IO.Path.GetFullPath(filename));
                    }

                }
                catch { }
            }
            else MessageBox.Show("Loading...");

        }
        private void sld_ses_MouseEnter(object sender, MouseEventArgs e)
        {
            last = sld_ses.Value;
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rb1.IsChecked == true)
            {
                me1.SpeedRatio = 1;
            }
            else if (rb2.IsChecked == true)
            {
                me1.SpeedRatio = 2;
            }
            else if (rb3.IsChecked == true)
            {
                me1.SpeedRatio = 0.5;
            }
        }

        private void cb_Click(object sender, RoutedEventArgs e)
        {
            if (rb1.IsChecked == true)
            {
                me1.SpeedRatio = 1;
                cb1.SelectedIndex = 0;
            }
            else if (rb2.IsChecked == true)
            {
                me1.SpeedRatio =2 ;
                cb1.SelectedIndex = 1;
            }
            else if (rb3.IsChecked == true)
            {
                me1.SpeedRatio = 0.5;
                cb1.SelectedIndex = 2;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (!MusicThread.IsAlive)
            {
                if (stkP.Visibility == Visibility.Visible)
                {
                    stkP.Visibility = Visibility.Hidden;
                }
                else if (stkP.Visibility == Visibility.Hidden)
                {
                    GetMusic();
                    stkP.Visibility = Visibility.Visible;
                }
            }
            else MessageBox.Show("Loading...");

        }
        int id;
        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dtlist = vtc.veriAl("Select id From Music where muzik_yolu='" + list1 + "'");
                id = Convert.ToInt32(dtlist.Rows[0]["id"]);
                id = id + 1;
                DataTable dtlist1 = vtc.veriAl("Select muzik_yolu From Music where id=" + id);
                list1 = (dtlist1.Rows[0]["muzik_yolu"]).ToString();
                me1.Source = new Uri(@list1);
                me1.Play();
                btn_SP.Style = (Style)FindResource("btnPause");
            }
            catch (Exception)
            {
                try
                {
                    id = 1;
                    DataTable dtlist1 = vtc.veriAl("Select muzik_yolu From Music where id=" + id);
                    list1 = (dtlist1.Rows[0]["muzik_yolu"]).ToString();
                    me1.Source = new Uri(@list1);
                    me1.Play();
                    btn_SP.Style = (Style)FindResource("btnPause");
                }catch { }
               
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtlist = vtc.veriAl("Select id From Music where muzik_yolu='" + list1 + "'");
            id = Convert.ToInt32(dtlist.Rows[0]["id"]);
            try 
            {
                if (id != 1)
                {
                    id = id - 1;
                    DataTable dtlist1 = vtc.veriAl("Select muzik_yolu From Music where id=" + id);
                    list1 = (dtlist1.Rows[0]["muzik_yolu"]).ToString();
                    me1.Source = new Uri(@list1);
                    me1.Play();
                    btn_SP.Style = (Style)FindResource("btnPause");
                }
            }
            catch { }
           
        }
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

            var bc = new BrushConverter();
            grid1.Background = (Brush)bc.ConvertFrom("#363636");
            this.Background= (Brush)bc.ConvertFrom("#363636");
            stk1.Background = (Brush)bc.ConvertFrom("#363636");
            sv1.Background = (Brush)bc.ConvertFrom("#363636");
            sld_time.Foreground = new SolidColorBrush(Colors.White);
            sld_ses.Foreground = new SolidColorBrush(Colors.White);
            sld_time.Style = (Style)FindResource("Horizontal_Slider2");
            sld_ses.Style = (Style)FindResource("Horizontal_Slider3");
            ch1.Foreground = new SolidColorBrush(Colors.White);
            stk_Ac_Kapa.Style = (Style)FindResource("btnmore1");
            lbl1.Foreground = new SolidColorBrush(Colors.White);
            rb1.Foreground = new SolidColorBrush(Colors.White);
            rb2.Foreground = new SolidColorBrush(Colors.White);
            rb3.Foreground = new SolidColorBrush(Colors.White);
            search_txt.BorderBrush = new SolidColorBrush(Colors.White);
            search_txt.Background= new SolidColorBrush(Colors.Gray);
            search_txt.Foreground= new SolidColorBrush(Colors.White);
            btnm.Style = (Style)FindResource("btnMusicList1");
            btnSearch.Style = (Style)FindResource("btnSearch1");


        }


        private void ch1_Unchecked(object sender, RoutedEventArgs e)
        {

            var bc = new BrushConverter();
            sld_time.Foreground = (Brush)bc.ConvertFrom("#363636");
            this.Background = new SolidColorBrush(Colors.White);
            grid1.Background = (Brush)bc.ConvertFrom("#FF37AFF7");
            stk1.Background = new SolidColorBrush(Colors.White);
            ch1.Foreground = (Brush)bc.ConvertFrom("#363636");
            sv1.Background = new SolidColorBrush(Colors.White);
            sld_time.Style = (Style)FindResource("Horizontal_Slider1");
            sld_ses.Style = (Style)FindResource("Horizontal_Slider");
            stk_Ac_Kapa.Style = (Style)FindResource("btnmore");
            lbl1.Foreground= (Brush)bc.ConvertFrom("#363636");
            rb1.Foreground = (Brush)bc.ConvertFrom("#FF888888");
            rb2.Foreground = (Brush)bc.ConvertFrom("#FF888888");
            rb3.Foreground = (Brush)bc.ConvertFrom("#FF888888");
            search_txt.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            search_txt.Background = new SolidColorBrush(Colors.White);
            search_txt.Foreground = (Brush)bc.ConvertFrom("#363636");
            btnm.Style = (Style)FindResource("btnMusicList");
            btnSearch.Style = (Style)FindResource("btnSearch");
        }

        private void me1_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dtlist = vtc.veriAl("Select id From Music where muzik_yolu='" + list1 + "'");
                id = Convert.ToInt32(dtlist.Rows[0]["id"]);
                id = id + 1;
                DataTable dtlist1 = vtc.veriAl("Select muzik_yolu From Music where id=" + id);
                list1 = (dtlist1.Rows[0]["muzik_yolu"]).ToString();
                me1.Source = new Uri(@list1);
                me1.Play();
                btn_SP.Style = (Style)FindResource("btnPause");
            }
            catch (Exception)
            {
                id = 1;
                DataTable dtlist1 = vtc.veriAl("Select muzik_yolu From Music where id=" + id);
                list1 = (dtlist1.Rows[0]["muzik_yolu"]).ToString();
                me1.Source = new Uri(@list1);
                me1.Play();
                btn_SP.Style = (Style)FindResource("btnPause");
            }
        }
   
    }
}
