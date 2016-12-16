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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RussianCube.Data;
using System.Threading;
using System.IO;

namespace RussianCube
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Windows.Forms.Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            DataCache.GameSpeed = 1;
            this.KeyDown += MainWindow_KeyDown;
            DataCache.MainWindowHandler = this;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(BlockRegularlyDown);
            timer.Enabled = false;
            timer.Interval = 700 / DataCache.GameSpeed;
            timer.Start();
            DataCache.CurrentRectangleHeap = new Rectangle[20, 12];
            DataCache.GamePanelTop = new int[12];
            int length = DataCache.GamePanel.GetLength(1);
            for (int j = 0; j < length; j++)
            {
                DataCache.GamePanel[19, j] = 1;
            }
            for (int j = 0; j < DataCache.GamePanel.GetLength(1); j++)
            {
                DataCache.GamePanelTop[j] = 19;
            }
            DataCache.CurrentCube.Component = new int[4, 4];
            DataCache.HasLineNeedToClear = false;
            Methods.CreateNewBlock();
        }

        private void BlockRegularlyDown(object sender, EventArgs e)
        {
            Methods.BlockRegularlyDown(false, 1);
        }

        public void NextCubeBrush()
        {
            int iLength = DataCache.NextCube.Component.GetLength(0);
            int jLength = DataCache.NextCube.Component.GetLength(1);
            int position = 1;

            foreach (var i in DataCache.NextCubeRecList)
                NextCubeGrid.Children.Remove(i);
            DataCache.NextCubeRecList.Clear();

            if (DataCache.NextCube.CubeType == 3)
            {
                position = 2;
            }
            else position = 1;

            for (int i = 0; i < iLength; i++)
            {
                for (int j = 0; j < jLength; j++)
                {
                    if (DataCache.NextCube.Component[i, j] != 0)
                    {
                        Rectangle _rec = new Rectangle();
                        _rec.Fill = new SolidColorBrush(Color.FromArgb(255, 126, 255, 100));
                        _rec.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                        DropShadowEffect shadoweffect = new DropShadowEffect();
                        shadoweffect.BlurRadius = 10;
                        shadoweffect.Color = Colors.Gray;
                        shadoweffect.ShadowDepth = 2;
                        _rec.Effect = shadoweffect;
                        this.NextCubeGrid.Children.Add(_rec);
                        Grid.SetRow(_rec, 5 + i);
                        Grid.SetColumn(_rec, position + j);
                        DataCache.NextCubeRecList.Add(_rec);
                    }
                }
            }
        }

        public void CubeBrush()
        {
            int iLength = DataCache.CurrentCube.Component.GetLength(0);
            int jLength = DataCache.CurrentCube.Component.GetLength(1);

            foreach (var i in DataCache.CurrentCubeRecList)
                this.GamePanelGrid.Children.Remove(i);
            DataCache.CurrentCubeRecList.Clear();

            for (int i = 0; i < iLength; i++)
            {
                for (int j = 0; j < jLength; j++)
                {
                    if (DataCache.CurrentCube.Component[i, j] != 0)
                    {
                        Rectangle _rec = new Rectangle();
                        _rec.Fill = new SolidColorBrush(Color.FromArgb(255, 126, 255, 100));
                        _rec.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                        DropShadowEffect shadoweffect = new DropShadowEffect();
                        shadoweffect.BlurRadius = 10;
                        shadoweffect.Color = Colors.Gray;
                        shadoweffect.ShadowDepth = 2;
                        _rec.Effect = shadoweffect;
                        this.GamePanelGrid.Children.Add(_rec);
                        Grid.SetRow(_rec, DataCache.CurrentCube.Y + i);
                        Grid.SetColumn(_rec, DataCache.CurrentCube.X + j);
                        DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = _rec;
                        DataCache.CurrentCubeRecList.Add(_rec);
                    }
                }
            }
            //////////////////////////
            //int time = 0;
            //for (int i = 0; i < 20; i++)
            //{
            //    for (int j = 0; j < 12; j++)
            //    {
            //        DataCache.ErrorReport.Append(DataCache.CurrentRectangleHeap[i, j] != null ? "1" : "0");
            //    }
            //    DataCache.ErrorReport.AppendLine();
            //}
            //DataCache.ErrorReport.AppendLine();
            //time++;
            //DataCache.ErrorReport1.Clear();

            //DataCache.ErrorReport1.Append("Top" + time.ToString() + ".txt");
            //if (!File.Exists(DataCache.ErrorReport1.ToString()))
            //{
            //    File.Create(DataCache.ErrorReport1.ToString()).Dispose();
            //}
            //File.WriteAllText(DataCache.ErrorReport1.ToString(), DataCache.ErrorReport.ToString());
            /////////////////////////////
        }

        private void GetColor()
        {
        }

        public void UpdateScorerTextBlock()
        {
            ScorerTextBlock.Text = DataCache.Scorer.ToString();
        }

        //处理键盘方向输入
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        //DataCache.ErrorReport.AppendLine("Key.Up");
                        Methods.RotateBlock();
                        break;
                    }
                case Key.Down:
                    {
                        //DataCache.ErrorReport.AppendLine("Key.Down");
                        Methods.BlockQuickDown();
                        break;
                    }
                case Key.Left:
                    {
                        //DataCache.ErrorReport.AppendLine("Key.Left");
                        Methods.BlockMoveLeft();
                        break;
                    }
                case Key.Right:
                    {
                        //DataCache.ErrorReport.AppendLine("Key.Right");
                        Methods.BlockMoveRight();
                        break;
                    }
                case Key.Pause:
                    {
                        break;
                    }
            }
        }

        private void DragWindowMove(object sender, RoutedEventArgs e)
        {
            DragMove();
        }

        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShrinkWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
