using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Shapes;

namespace RussianCube.Data
{
    public class Times : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _t { get; set; }
        public int t
        {
            get { return _t; }
            set
            {
                _t = value;
                NotifyPropertyChanged("t");
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }  
    }

    public class DataCache
    {
        public static List<Rectangle> CurrentCubeRecList = new List<Rectangle>();
        public static List<Rectangle> NextCubeRecList = new List<Rectangle>();
        public static Rectangle[,] CurrentRectangleHeap { get; set; }
        public static MainWindow MainWindowHandler { get; set; }
        public static int t = 0;
        public static int Scorer { get; set; }
        public static int[,] GamePanel = new int[20, 12];
        public static Cube CurrentCube = new Cube();
        public static Cube NextCube { get; set; }
        public static bool IsPaused { get; set; }
        public static Thread TimerThread { get; set; }
        public static int GameSpeed { get; set; }
        public static int[] GamePanelTop { get; set; }

        //public static StringBuilder ErrorReport = new StringBuilder();
        //public static StringBuilder ErrorReport1 = new StringBuilder();
        public static bool HasLineNeedToClear { get; set; }
        public static int BaseScore = 200;
    }
}
