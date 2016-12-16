using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace RussianCube.Data
{
    public class Methods
    {
        //添加新方块
        public static void CreateNewBlock()
        {
            int iLength = DataCache.CurrentCube.Component.GetLength(0);
            int jLength = DataCache.CurrentCube.Component.GetLength(1);

            if (CreateValidity())
            {
                if (!DataCache.HasLineNeedToClear)
                    UpdateGamePanelTop(iLength, jLength);
                DataCache.HasLineNeedToClear = false;
                /////////////
                //for (int i = 0; i < 12; i++)
                //    Console.Write(DataCache.GamePanelTop[i] + " ");
                //Console.WriteLine();
                ////////////////

                if (DataCache.CurrentCube.X == 0 && DataCache.CurrentCube.Y == 0)
                {
                    Random rand = new Random();
                    Cube cube = GetCubeType(rand.Next(0, 5));
                    if (cube != null)
                    {
                        cube.X = 4;
                        cube.Y = 0;
                        DataCache.CurrentCube = cube;
                    }

                    Cube _cube = GetCubeType(rand.Next(0, 5));
                    if (cube != null)
                    {
                        cube.X = 4;
                        cube.Y = 0;
                        DataCache.NextCube = _cube;
                    }
                }
                else
                {
                    Random rand = new Random();
                    DataCache.CurrentCube = DataCache.NextCube;
                    Cube _cube = GetCubeType(rand.Next(0, 5));
                    if (_cube != null)
                    {
                        _cube.X = 4;
                        _cube.Y = 0;
                        DataCache.NextCube = _cube;
                    }
                }



                for (int i = 0; i < DataCache.CurrentCube.Component.GetLength(0); i++)
                {
                    for (int j = 0; j < DataCache.CurrentCube.Component.GetLength(1); j++)
                    {
                        if (DataCache.CurrentCube.Component[i, j] != 0)
                        {
                            DataCache.GamePanel[i + DataCache.CurrentCube.Y, j + DataCache.CurrentCube.X] = 1;
                        }
                    }
                }
                DataCache.MainWindowHandler.CubeBrush();
                DataCache.MainWindowHandler.NextCubeBrush();
            }
            else
            {
                DataCache.MainWindowHandler.timer.Stop();
                MessageBox.Show("Game Is Over");
            }

            //for (int i = 0; i < DataCache.GamePanel.GetLength(0); i++)
            //{
            //    for (int j = 0; j > DataCache.GamePanel.GetLength(1); j++)
            //        DataCache.ErrorReport.Append(DataCache.GamePanel[i, j].ToString());
            //    DataCache.ErrorReport.AppendLine();
            //}
        }

        //检查新添加方块合法性
        private static bool CreateValidity()
        {
            bool valid = true;

            for (int i = 4; i < 8; i++)
            {
                if (DataCache.GamePanel[0, i] != 0)
                {
                    valid = false;
                    return valid;
                }
            }

            return valid;
        }

        //方块每秒例行下落
        public static void BlockRegularlyDown(bool CheckGamePanelTopAfterDown, int DownDistance = 1)
        {
            int iLength = DataCache.CurrentCube.Component.GetLength(0);
            int jLength = DataCache.CurrentCube.Component.GetLength(1);

            if (CheckDownValidity())
            {
                for (int i = iLength - 1; i >= 0; i--)
                {
                    for (int j = jLength - 1; j >= 0; j--)
                    {
                        if (j + DataCache.CurrentCube.X != 12 && DataCache.CurrentCube.Component[i, j] != 0)
                        {
                            DataCache.GamePanel[i + DataCache.CurrentCube.Y + DownDistance, j + DataCache.CurrentCube.X] = DataCache.CurrentCube.Component[i, j];
                            DataCache.GamePanel[i + DataCache.CurrentCube.Y, j + DataCache.CurrentCube.X] = 0;
                            DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = null;
                        }
                    }
                }
                DataCache.CurrentCube.Y += DownDistance;
                DataCache.MainWindowHandler.CubeBrush();
            }
            else
            {
                CheckGamePanelTopAfterDown = true;
            }

            if (CheckGamePanelTopAfterDown)
            {
                ClearBlock();
                DataCache.CurrentCubeRecList.Clear();
                CreateNewBlock();
            }

            //////////////////////////////
            //int time = 0;
            //for (int i = 0; i < 20; i++)
            //{
            //    for (int j = 0; j < 12; j++)
            //    {
            //        DataCache.ErrorReport.Append(DataCache.GamePanel[i, j]);
            //    }
            //    DataCache.ErrorReport.AppendLine();
            //}
            //time++;
            //DataCache.ErrorReport1.Clear();

            //DataCache.ErrorReport1.Append("Record" + time.ToString() + ".txt");
            //if (!File.Exists(DataCache.ErrorReport1.ToString()))
            //{
            //    File.Create(DataCache.ErrorReport1.ToString()).Dispose();
            //}
            //File.WriteAllText(DataCache.ErrorReport1.ToString(), DataCache.ErrorReport.ToString());
            /////////////////////////////////
        }

        //更新每列高度（用于快速下落时计算下落高度）
        private static void UpdateGamePanelTop(int iLength, int jLength)
        {
            for (int i = iLength - 1; i >= 0; i--)
            {
                for (int j = 0; j < jLength; j++)
                {
                    if (DataCache.CurrentCube.Component[i, j] != 0)
                    {
                        DataCache.GamePanelTop[DataCache.CurrentCube.X + j] = DataCache.CurrentCube.Y + i;
                    }
                }
            }
        }

        //检测下落合法性
        private static bool CheckDownValidity()
        {
            bool valid = true;
            int iLength = DataCache.CurrentCube.Component.GetLength(0);
            int jLength = DataCache.CurrentCube.Component.GetLength(1);

            for (int i = 0; i < iLength; i++)
            {
                for (int j = 0; j < jLength; j++)
                {
                    if (DataCache.CurrentCube.Component[i, j] != 0)
                    {
                        if (i < iLength - 1 && DataCache.CurrentCube.Component[i + 1, j] == 0)
                        {
                            if (DataCache.GamePanel[i + DataCache.CurrentCube.Y + 1, j + DataCache.CurrentCube.X] == 1)
                            {
                                valid = false;
                                return valid;
                            }
                        }
                        else if (i == iLength - 1 && DataCache.GamePanel[i + DataCache.CurrentCube.Y + 1, j + DataCache.CurrentCube.X] == 1)
                        {
                            valid = false;
                            return valid;
                        }
                    }
                }
            }
            return valid;
        }

        //方块快速下落
        public static void BlockQuickDown()
        {
            int iLength = DataCache.CurrentCube.Component.GetLength(0);
            int jLength = DataCache.CurrentCube.Component.GetLength(1);
            List<int> distancelist = new List<int>();

            for (int i = 0; i < iLength; i++)
            {
                for (int j = 0; j < jLength; j++)
                {
                    if (DataCache.CurrentCube.Component[i, j] != 0)
                    {
                        if (i == iLength - 1)
                        {
                            distancelist.Add(DataCache.GamePanelTop[DataCache.CurrentCube.X + j] - DataCache.CurrentCube.Y - i - 1);
                        }
                        else if (i < iLength - 1 && DataCache.CurrentCube.Component[i + 1, j] == 0)
                        {
                            distancelist.Add(DataCache.GamePanelTop[DataCache.CurrentCube.X + j] - DataCache.CurrentCube.Y - i - 1);
                        }
                    }
                }
            }

            BlockRegularlyDown(true, distancelist.Min());
        }

        //向左移动方块
        public static void BlockMoveLeft()
        {
            if (CheckMoveLeftValidity())
            {
                int iLength = DataCache.CurrentCube.Component.GetLength(0);
                int jLength = DataCache.CurrentCube.Component.GetLength(1);

                for (int j = 0; j < jLength; j++)
                {
                    for (int i = 0; i < iLength; i++)
                    {
                        if (DataCache.CurrentCube.Component[i, j] != 0)
                        {
                            DataCache.GamePanel[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j - 1] = DataCache.CurrentCube.Component[i, j];
                            DataCache.GamePanel[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = 0;
                        }
                        if (DataCache.CurrentCube.X + j != 12)
                        {
                            if (DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] != null)
                            {
                                DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j - 1] = DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j];
                                DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = null;
                            }
                        }
                    }
                }
                DataCache.CurrentCube.X--;
                DataCache.MainWindowHandler.CubeBrush();
            }
        }

        //检查向左移动合法性
        private static bool CheckMoveLeftValidity()
        {
            bool valid = true;

            for (int i = 0; i < DataCache.CurrentCube.Component.GetLength(0); i++)
            {
                if (valid)
                {
                    for (int j = 0; j < DataCache.CurrentCube.Component.GetLength(1); j++)
                    {
                        if (DataCache.CurrentCube.Component[i, j] != 0 && j == 0)
                        {
                            if (DataCache.CurrentCube.X + j == 0 || (DataCache.GamePanel[i + DataCache.CurrentCube.Y, j + DataCache.CurrentCube.X - 1] == 1))
                            {
                                valid = false;
                                return valid;
                            }
                        }
                        else if (DataCache.CurrentCube.Component[i, j] != 0 && DataCache.CurrentCube.Component[i, j - 1] == 0)
                        {
                            if (DataCache.GamePanel[i + DataCache.CurrentCube.Y, j + DataCache.CurrentCube.X - 1] == 1)
                            {
                                valid = false;
                                return valid;
                            }
                        }
                    }
                }
                else break;
            }
            return valid;
        }

        //向右移动方块
        public static void BlockMoveRight()
        {
            if (CheckMoveRightValidity())
            {
                int iLength = DataCache.CurrentCube.Component.GetLength(0);
                int jLength = DataCache.CurrentCube.Component.GetLength(1);

                for (int j = jLength - 1; j >= 0; j--)
                {
                    for (int i = iLength - 1; i >= 0; i--)
                    {
                        if (DataCache.CurrentCube.Component[i, j] != 0)
                        {
                            DataCache.GamePanel[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j + 1] = DataCache.CurrentCube.Component[i, j];
                            DataCache.GamePanel[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = 0;
                        }
                        if (DataCache.CurrentCube.X + j != 12 && DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] != null)
                        {
                            DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j + 1] = DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j];
                            DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = null;
                        }
                    }
                }
                DataCache.CurrentCube.X++;
                DataCache.MainWindowHandler.CubeBrush();
            }
        }

        //检查向右移动合法性
        private static bool CheckMoveRightValidity()
        {
            bool valid = true;

            for (int i = 0; i < DataCache.CurrentCube.Component.GetLength(0); i++)
            {
                if (valid)
                {
                    for (int j = DataCache.CurrentCube.Component.GetLength(1) - 1; j >= 0; j--)
                    {
                        if (DataCache.CurrentCube.Component[i, j] != 0 && j == DataCache.CurrentCube.Component.GetLength(1) - 1)
                        {
                            if (DataCache.CurrentCube.X + j == 11 || (DataCache.GamePanel[i + DataCache.CurrentCube.Y, j + DataCache.CurrentCube.X + 1] == 1))
                            {
                                valid = false;
                                return valid;
                            }
                        }
                        else if (DataCache.CurrentCube.Component[i, j] != 0 && DataCache.CurrentCube.Component[i, j + 1] == 0)
                        {
                            if ((DataCache.CurrentCube.X + j == 11) || (DataCache.GamePanel[i + DataCache.CurrentCube.Y, j + DataCache.CurrentCube.X + 1] == 1))
                            {
                                valid = false;
                                return valid;
                            }
                        }
                    }
                }
                else break;
            }
            return valid;
        }

        //清除需要消除的方块
        private static void ClearBlock()
        {
            RemoveBlockAccordingToClearLineList(ReturnClearLine());
            //DataCache.HasLineNeedToClear = false;
            //RemoveRectangleAccordingToClearLineList(ReturnClearLine());
        }

        //检测并返回需要清楚的行
        private static List<int> ReturnClearLine()
        {
            int iLength = DataCache.GamePanel.GetLength(0);
            int jLength = DataCache.GamePanel.GetLength(1);

            bool IsFull = true;
            List<int> _list = new List<int>();

            for (int i = iLength - 2; i >= 0; i--)
            {
                for (int j = 0; j < jLength; j++)
                {
                    if (DataCache.GamePanel[i, j] == 0)
                    {
                        IsFull = false;
                        break;
                    }
                }
                if (IsFull)
                {
                    _list.Add(i);
                }
                IsFull = true;
            }

            return _list;
        }

        //清除需要消除的行（高度更新有BUG）
        private static void RemoveBlockAccordingToClearLineList(List<int> _list)
        {
            int iLength = DataCache.GamePanel.GetLength(0);
            int jLength = DataCache.GamePanel.GetLength(1);
            int m;

            for (int i = 0; i < _list.Count(); i++)
            {
                m = _list[i] + i;
                for (int j = 0; j < jLength; j++)
                {
                    DataCache.GamePanel[m, j] = 0;
                    DataCache.MainWindowHandler.GamePanelGrid.Children.Remove(DataCache.CurrentRectangleHeap[m, j]);
                    DataCache.CurrentRectangleHeap[m, j] = null;
                }

                for (int n = m; n >= 1; n--)
                {
                    for (int j = 0; j < jLength; j++)
                    {
                        DataCache.GamePanel[n, j] = DataCache.GamePanel[n - 1, j];
                        DataCache.GamePanel[n - 1, j] = 0;

                        if (DataCache.CurrentRectangleHeap[n - 1, j] != null)
                        {
                            DataCache.CurrentRectangleHeap[n, j] = DataCache.CurrentRectangleHeap[n - 1, j];
                            DataCache.CurrentRectangleHeap[n - 1, j] = null;
                            //DataCache.MainWindowHandler.GamePanelGrid.Children.Remove(DataCache.CurrentRectangleHeap[n - 1, j]);
                            System.Windows.Controls.Grid.SetRow(DataCache.CurrentRectangleHeap[n, j], n);
                            System.Windows.Controls.Grid.SetColumn(DataCache.CurrentRectangleHeap[n, j], j);
                        }
                    }
                }
            }
            if (_list.Count() != 0)
            {
                DataCache.HasLineNeedToClear = true;
                int _temp;
                for (int j = 0; j < jLength; j++)
                {
                    _temp = 19;
                    for (int i = iLength - 1; i >= 0; i--)
                    {
                        if (DataCache.GamePanel[i, j] != 0 && i < _temp)
                            _temp = i;
                    }
                    DataCache.GamePanelTop[j] = _temp;
                    Console.Write(DataCache.GamePanelTop[j] + " ");
                }
                Console.WriteLine();
                //DataCache.ErrorReport.Append("Clear!");
            }

            UpdateScore(_list);
        }

        //旋转方块
        public static void RotateBlock()
        {
            Cube _cube = GetRotationResult();
            if (CheckRotateValidity(_cube))
            {
                DataCache.CurrentCube = _cube;
                DataCache.MainWindowHandler.CubeBrush();
            }
        }

        //检测方块旋转合法性
        private static bool CheckRotateValidity(Cube _cube)
        {
            bool valid = true;

            //清除DataCache.CurrentCube在GamePanel中的占位
            for (int i = 0; i < DataCache.CurrentCube.Component.GetLength(0); i++)
            {
                for (int j = 0; j < DataCache.CurrentCube.Component.GetLength(1); j++)
                {
                    if (DataCache.CurrentCube.Component[i, j] != 0)
                        DataCache.GamePanel[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = 0;
                }
            }

            //比较旋转结果是否可行
            if (_cube.X + _cube.Component.GetLength(1) >= 12 && _cube.TurnTimes != DataCache.CurrentCube.TurnTimes) _cube.X = 12 - _cube.Component.GetLength(1);
            if (_cube.Y + _cube.Component.GetLength(0) >= 19) _cube.Y = 19 - _cube.Component.GetLength(0);
            for (int i = 0; i < _cube.Component.GetLength(0); i++)
            {
                for (int j = 0; j < _cube.Component.GetLength(1); j++)
                {
                    if (_cube.Component[i, j] != 0 && DataCache.GamePanel[_cube.Y + i, _cube.X + j] != 0)
                        valid = false;
                }
            }

            Cube _temp = valid ? _cube : DataCache.CurrentCube;

            for (int i = 0; i < _temp.Component.GetLength(0); i++)
            {
                for (int j = 0; j < _temp.Component.GetLength(1); j++)
                {
                    //if (_temp.X + j >= 12) _temp.X = 11 - j;
                    if (_temp.Component[i, j] != 0)
                        DataCache.GamePanel[_temp.Y + i, _temp.X + j] = 1;
                }
            }

            if (valid)
            {
                for (int i = 0; i < DataCache.CurrentCube.Component.GetLength(0); i++)
                {
                    for (int j = 0; j < DataCache.CurrentCube.Component.GetLength(1); j++)
                    {
                        if (DataCache.CurrentCube.Component[i, j] != 0)
                            DataCache.CurrentRectangleHeap[DataCache.CurrentCube.Y + i, DataCache.CurrentCube.X + j] = null;
                    }
                }
            }

            return valid;
        }

        //获取旋转测试结果
        private static Cube GetRotationResult()
        {
            int[,] _component = new int[4, 4];
            Cube _cube = new Cube(DataCache.CurrentCube);

            switch (DataCache.CurrentCube.CubeType)
            {
                case 0:
                    {
                        if (DataCache.CurrentCube.Orientation == 1)
                        {
                            switch (DataCache.CurrentCube.TurnTimes)
                            {
                                case 0:
                                    _cube.TurnTimes = 1;
                                    _component = new int[,]{
                                {1,0},
                                {1,1},
                                {0,1},
                                }; break;
                                case 1:
                                    _cube.TurnTimes = 0;
                                    _component = new int[,]{
                                {0,1,1},
                                {1,1,0},
                                };
                                    break;
                            }
                        }
                        else
                        {
                            switch (DataCache.CurrentCube.TurnTimes)
                            {
                                case 0:
                                    _cube.TurnTimes = 1;
                                    _component = new int[,]{
                                    {0,1},
                                    {1,1},
                                    {1,0},
                                    };
                                    break;

                                case 1:
                                    _cube.TurnTimes = 0;
                                    _component = new int[,]{
                                    {1,1,0},
                                    {0,1,1},
                                    };
                                    break;
                            }
                        }
                    }
                    break;

                case 1:
                    {
                        if (DataCache.CurrentCube.Orientation == 1)
                        {
                            switch (DataCache.CurrentCube.TurnTimes)
                            {
                                case 0:
                                    _cube.TurnTimes = 1;
                                    _component = new int[,]{
                                {1,1},
                                {0,1},
                                {0,1},
                                };
                                    break;

                                case 1:
                                    _cube.TurnTimes = 2;
                                    _component = new int[,]{
                                {0,0,1},
                                {1,1,1},
                                };
                                    break;

                                case 2:
                                    _cube.TurnTimes = 3;
                                    _component = new int[,]{
                                {1,0},
                                {1,0},
                                {1,1},
                                };
                                    break;

                                case 3:
                                    _cube.TurnTimes = 0;
                                    _component = new int[,]{
                                {1,1,1},
                                {1,0,0},
                                };
                                    break;
                            }
                        }
                        else
                        {
                            switch (DataCache.CurrentCube.TurnTimes)
                            {
                                case 0:
                                    _cube.TurnTimes = 1;
                                    _component = new int[,]{
                                {1,1},
                                {1,0},
                                {1,0},
                                }; break;
                                case 1:
                                    _cube.TurnTimes = 2;
                                    _component = new int[,]{
                                {1,1,1},
                                {0,0,1},
                                }; break;
                                case 2:
                                    _cube.TurnTimes = 3;
                                    _component = new int[,]{
                                {0,1},
                                {0,1},
                                {1,1},
                                }; break;
                                case 3:
                                    _cube.TurnTimes = 0;
                                    _component = new int[,]{
                                {1,0,0},
                                {1,1,1},
                                }; break;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (DataCache.CurrentCube.TurnTimes)
                        {
                            case 0:
                                _cube.TurnTimes = 1;
                                _component = new int[,]{
                                    {1},
                                    {1},
                                    {1},
                                    {1},
                                    }; break;
                            case 1:
                                _cube.TurnTimes = 0;
                                _component = new int[,]{
                                {1,1,1,1},
                                }; break;
                        }
                        break;
                    }
                case 3:
                    {
                        _component = DataCache.CurrentCube.Component;
                        _cube.TurnTimes = DataCache.CurrentCube.TurnTimes;
                        _cube.CubeType = DataCache.CurrentCube.CubeType;
                        break;
                    }
                case 4:
                    {
                        switch (DataCache.CurrentCube.TurnTimes)
                        {
                            case 0: _cube.TurnTimes = 1; _component = new int[,]{
                            {0,1},
                            {1,1},
                            {0,1},
                            }; break;
                            case 1: _cube.TurnTimes = 2; _component = new int[,]{
                            {0,1,0},
                            {1,1,1},
                            }; break;
                            case 2: _cube.TurnTimes = 3; _component = new int[,]{
                            {1,0},
                            {1,1},
                            {1,0},
                            }; break;
                            case 3: _cube.TurnTimes = 0; _component = new int[,]{
                            {1,1,1},
                            {0,1,0},
                            }; break;
                        }
                        break;
                    }

                default: _component = null; break;
            }
            _cube.Component = _component;
            return _cube;
        }

        //随机产生新方块
        private static Cube GetCubeType(int _rand)
        {
            Cube _cube = new Cube();
            _cube.Color = _rand;
            Random rand = new Random();
            int orientation = rand.Next(0, 2);
            _cube.Orientation = orientation;
            _cube.TurnTimes = 0;

            switch (_rand)
            {
                case 0:
                    {
                        _cube.CubeType = 0;
                        if (orientation == 1)
                        {

                            _cube.Component = new int[,] {
                        {0,1,1},
                        {1,1,0},
                    };
                        }
                        else
                        {
                            _cube.Component = new int[,]{
                            {1,1,0},
                            {0,1,1},
                            };
                        }
                        return _cube;
                    }
                case 1:
                    {
                        _cube.CubeType = 1;
                        if (orientation == 1)
                        {
                            _cube.Component = new int[,] {
                        {1,1,1},
                        {1,0,0},
                    };
                        }
                        else
                        {
                            _cube.Component = new int[,] {
                        {1,0,0},
                        {1,1,1},
                    };
                        }
                        return _cube;
                    }
                case 2:
                    {
                        _cube.CubeType = 2;
                        _cube.Component = new int[,] {
                        {1,1,1,1},
                        };
                        return _cube;

                    }
                case 3:
                    {
                        _cube.CubeType = 3;
                        _cube.Component = new int[,]{
                        {1,1,0},
                        {1,1,0},
                        };
                        return _cube;
                    }
                case 4:
                    {
                        _cube.CubeType = 4;
                        _cube.Component = new int[,]{
                        {1,1,1},
                        {0,1,0},
                        };
                        return _cube;
                    }
                default: return null;
            }
        }

        private static void UpdateScore(List<int> _list)
        {
            DataCache.Scorer += DataCache.BaseScore * _list.Count() * _list.Count();
            DataCache.MainWindowHandler.ScorerTextBlock.Text = DataCache.Scorer.ToString();
        }
    }
}
