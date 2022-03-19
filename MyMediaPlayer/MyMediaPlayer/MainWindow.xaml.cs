using System;
using Microsoft.Win32;//Diyaloglar için Win kütüphanesi
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
using System.Windows.Threading; //timer kurulumu için kütüphane
using System.IO; //DVD vb. açmak için gerekli kütüphane


namespace MyMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            media.LoadedBehavior = MediaState.Manual;
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += timer_Tick; 
        }

        void timer_Tick(object sender, EventArgs e)
        {
            sld_pos.Value = media.Position.TotalMilliseconds;
        }


        /*..............MENÜ KISMI..............*/

        double video_suresi;
        private void dosya_ekle_Click(object sender, RoutedEventArgs e)
        {
            //Dosya açmak için eklediğimiz butonun içeriği...
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Dosya Aç";
            dialog.Filter = "MP4 Video|*.mp4|WMV Dosyaları|*.wmv|Tüm Dosyalar|*.*"; //Dosyaları filtreler.
            if (dialog.ShowDialog() == true)//Dialog'un ekrana gelip gelmediğini kontrol eder. 
            {
                string yol = dialog.FileName;
                Uri adres = new Uri(yol, UriKind.RelativeOrAbsolute);
                media.Source = adres;
                media.Play();
                System.Threading.Thread.Sleep(1000);
                video_suresi = media.NaturalDuration.TimeSpan.TotalMilliseconds;
                sld_pos.Maximum = video_suresi;
                timer.Start();
            }
        }

        //USB için yaptık
        private void dvd_ac_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "G:\\"; //Local olarak medyanın açılacağı sürücü yazılmalıdır
            file.Filter = "Medya Dosyası |*.mp4";
            file.Title = "Medya Seç";

            if (file.ShowDialog() == true)
            {
                string yol = file.FileName;
                Uri adres = new Uri(yol, UriKind.RelativeOrAbsolute);
                media.Source = adres;
                media.Play();
                System.Threading.Thread.Sleep(1000);
                video_suresi = media.NaturalDuration.TimeSpan.TotalMilliseconds;
                sld_pos.Maximum = video_suresi;
                timer.Start();
            }
        }

        private void cikis_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            anamenu.Visibility = Visibility.Collapsed; //Menü gizlenirken ekranda kapladığı alan kalmasın ise Collapsed
        }

        //Pencere üzerinde mouse tıklatılınca menü görünürlüğünü değiştirme
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (anamenu.Visibility == Visibility.Collapsed)
                anamenu.Visibility = Visibility.Visible;
            else
                anamenu.Visibility = Visibility.Visible;
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
            media.Visibility = Visibility.Visible;
            txt_sure.Text = "Medya Oynatılıyor";
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
            media.LoadedBehavior = MediaState.Manual;
            media.Visibility = Visibility.Hidden;
            txt_sure.Text = "Medya Durduruldu";
        }

        //Videoyu 10 sn ilerletme butonu
        private void forward_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan pozisyon = new TimeSpan();
            pozisyon = media.Position; //üzerinde işlem yapabilmek için medyanın şu anki konumunu atıyoruz
            pozisyon += TimeSpan.FromSeconds(10);
            media.Position = pozisyon;
        }


        /*..............BUTONLAR..............*/

        private void basla_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
            media.Visibility = Visibility.Visible;
            txt_sure.Text = "Medya Oynatılıyor";
            
        }

        private void duraklat_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
            txt_sure.Text = "Medya Duraklatıldı";
        }

        private void durdur_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
            media.LoadedBehavior = MediaState.Manual;
            media.Visibility = Visibility.Hidden;
            txt_sure.Text = "Medya Durduruldu";
        }


        //Ses açma kapama butonu
        private void ses_Click(object sender, RoutedEventArgs e)
        {
            if (media.IsMuted == false)

            {
                media.IsMuted = true;
            }

            else { media.IsMuted = false; }
        }


        /*..............SLİDER.............*/

        //Slider cursor ü hareket ettirnce video iletletme/geri alma işlemi
        private void sld_pos_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan yenipos = new TimeSpan();
            yenipos = TimeSpan.FromMilliseconds(sld_pos.Value);
            media.Position = yenipos;
        }

        //Ses ayarlama bölümü
        private void sld_ses_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            media.Volume = sld_ses.Value / 100;
        }
    }
}
