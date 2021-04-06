using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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

namespace IrredComplNumFinder02
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, iProgressIndicator
    {
        long size, selsize, maxfactors, minfactors;
        ProdGrid Factors;
        Ellipse[,] FoundProducts, FoundFactors;
        bool[,] CorrectFactorNum;
        bool SearchLEq = false; //initial setting: search for complex numbers with *exactly* the selected number of factor pairs 
        const double smallmargfract = 0.1; // small fraction of the space available for one result indicator (for boundaries and padding)
        const double bigmargfract = 0.3; // not so small fraction of the space available for one result indicator (for the smaller circles)
        ComplNum SelectedProd = null;//initially no number is selected as product to show its factor pairs

        private delegate void NoArgDelegate();

        public MainWindow()
        {
            InitializeComponent();
            size = 10;
            selsize = 10;
            maxfactors = 0;
            GridSize.Text = size.ToString();
        }

        private void SizePlus_Click(object sender, RoutedEventArgs e)
        {
            selsize++;
            GridSize.Text = selsize.ToString();
            StartSearch.IsEnabled = true;
            if (selsize > 2)
                SizeMinus.IsEnabled = true;
            StartSearch.Content = "Start Search";
        }

        private void SizeMinus_Click(object sender, RoutedEventArgs e)
        {
            if (selsize > 2)
            {
                selsize--;
                GridSize.Text = selsize.ToString();
                StartSearch.IsEnabled = true;
            }
            if (selsize <= 2)
                SizeMinus.IsEnabled = false;
            StartSearch.Content = "Start Search";
        }

        private void StartSearch_Click(object sender, RoutedEventArgs e)
        {
            size = selsize;
            StartSearch.IsEnabled = false;
            StartSearch.Content = "Searching...";
            SizeMinus.IsEnabled = false;
            SizePlus.IsEnabled = false;
            FactNumMinus.IsEnabled = false;
            FactNumPlus.IsEnabled = false;
            ShowFactors.IsEnabled = false;
            //ensure changes are visible
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                (NoArgDelegate)delegate { });
            Factors = new ProdGrid(size, this);
            if (Factors != null)
            {
                FactNumPlus.IsEnabled = true;
                minfactors = Factors.GetMinFactNum();
                maxfactors = minfactors;
                FactorNum.Text = maxfactors.ToString();
                ShowFactors.IsEnabled = true;
            }
            SizeMinus.IsEnabled = true;
            SizePlus.IsEnabled = true;
            StartSearch.Content = "Finished.";
            /*SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = Color.FromRgb(150, 255, 150);
            StartSearch.Background = myBrush;*/

        }

        public void UpdateProgress(double progress)
        {
            //MessageBox.Show("Progress update: " + progress.ToString());
            if (progress < 0)
            {
                SolidColorBrush myBrush = new SolidColorBrush();
                myBrush.Color = Color.FromRgb(255, 150, 150);
                StartSearch.Background = myBrush;
                StartSearch.Content = "Failure!";
                //ensure changes are visible:
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                    (NoArgDelegate)delegate { });
            }
            else if((0<=progress)&&(1>=progress))
            {
                
                LinearGradientBrush myBrush = new LinearGradientBrush();
                Point UpLeft = StartSearch.PointToScreen(new Point(0, 0));
                Point CenterLeft = new Point(UpLeft.X, UpLeft.Y + (StartSearch.ActualHeight / 2));
                Point CenterRight = new Point(CenterLeft.X + StartSearch.ActualWidth, CenterLeft.Y);
                myBrush.StartPoint = new Point(0,0);
                myBrush.EndPoint = new Point(1,0);
                GradientStop GreenStart = new GradientStop();
                GreenStart.Color = Color.FromRgb(100, 255, 100);
                GreenStart.Offset = 0.0;
                GradientStop GreenEnd = new GradientStop();
                GreenEnd.Color = Color.FromRgb(100, 255, 100);
                GreenEnd.Offset = Math.Max(progress-0.1,0);
                GradientStop GreyStart = new GradientStop();
                GreyStart.Color = Color.FromRgb(221, 221, 221);
                GreyStart.Offset = progress;
                GradientStop GreyEnd = new GradientStop();
                GreyEnd.Color = Color.FromRgb(221, 221, 221);
                GreyEnd.Offset = 1.0;
                myBrush.GradientStops.Add(GreenStart);
                myBrush.GradientStops.Add(GreenEnd);
                myBrush.GradientStops.Add(GreyStart);
                myBrush.GradientStops.Add(GreyEnd);
                /*SolidColorBrush myBrush = new SolidColorBrush();
                double RedVal = Math.Round(((1 - progress) * 221) + (progress * 100));
                double GreenVal = Math.Round(((1 - progress) * 221) + (progress * 255));
                double BlueVal = Math.Round(((1 - progress) * 221) + (progress * 100));
                myBrush.Color = Color.FromRgb((byte)RedVal, (byte)GreenVal, (byte)BlueVal);
                //myBrush.Color = Color.FromRgb(100, 255, 100);*/
                StartSearch.Background = myBrush;
                //ensure changes are visible:
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                    (NoArgDelegate)delegate { });
            }
            else
            {
                SolidColorBrush myBrush = new SolidColorBrush();
                myBrush.Color = Color.FromRgb(221, 221, 221);
                StartSearch.Background = myBrush;
            }
            //ensure changes are visible:
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                (NoArgDelegate)delegate { });
        }

        private void FactNumPlus_Click(object sender, RoutedEventArgs e)
        {
            maxfactors++;
            ShowFactors.IsEnabled = true;
            FactorNum.Text = maxfactors.ToString();
            if (maxfactors > minfactors)
                FactNumMinus.IsEnabled = true;
        }

        private void FactNumMinus_Click(object sender, RoutedEventArgs e)
        {
            if (maxfactors > minfactors)
            {
                maxfactors--;
                FactorNum.Text = maxfactors.ToString();
                ShowFactors.IsEnabled = true;
            }
            if (maxfactors <= minfactors)
                FactNumMinus.IsEnabled = false;
        }

        private ComplNum FindIndicator(Ellipse SearchTarget, Ellipse[,] SearchSpace)
        {
            ComplNum SearchRes = null;
            if (SearchSpace != null)
            {
                long rows = SearchSpace.GetLength(0);
                long cols = SearchSpace.GetLength(1);
                for (long i = 0; i < rows; i++)
                {
                    for (long j = 0; j < cols; j++)
                    {
                        if (SearchSpace[i, j] == SearchTarget)
                        {
                            SearchRes = new ComplNum(j - size, i - size);
                        }
                    }
                }
            }
            return SearchRes;
        }

        private SolidColorBrush[] Colors(long NumElements)
        {
            double[] RedVals = new double[17]{      1,  1,      1,      1,  0.66,   0.33,   0,  0,      0,      0,  0,      0,      0,  0.33,   0.66,   1,      1};
            double[] GreenVals = new double[17] {   0,  0.33,   0.66,   1,  1,      1,      1,  1,      1,      1,  0.66,   0.33,   0,  0,      0,      0,      0};
            double[] BlueVals = new double[17] {    0,  0,      0,      0,  0,      0,      0,  0.33,   0.66,   1,  1,      1,      1,  1,      1,      0.66,   0.33};
            //byte[] ColPerm = new byte[17] { 13, 7, 6, 2, 1, 10, 11, 17, 4, 3, 15, 16, 5, 12, 9, 14, 8 };
            byte[] ColPerm = new byte[17] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17 };
            byte maxcols = 17;
            //string message = "";
            byte ColPhase = 0;
            byte BrPhase = 0;
            /*double ColDelta = ((double)6 / (double)NumElements)*0.95;
            double RedVal = 1;
            double GreenVal = 0;
            double BlueVal = 0;*/
            double BriVal = 1;
            //message += ColDelta.ToString();
            //message += " ";
            SolidColorBrush[] List = new SolidColorBrush[NumElements];
            for (long i=0; i<NumElements; i++)
            {
                //message += "(" + RedVal.ToString() + "/" + GreenVal.ToString() + "/" + BlueVal.ToString() + "/" + BriVal.ToString() + "/"+ColPhase.ToString()+ ") ";
                // creating a new brush with the next color
                List[i] = new SolidColorBrush();
                List[i].Color = Color.FromRgb(
                    (byte)Math.Round(255 * RedVals[ColPerm[ColPhase]-1] * BriVal),
                    (byte)Math.Round(255 * GreenVals[ColPerm[ColPhase] - 1] * BriVal), 
                    (byte)Math.Round(255 * BlueVals[ColPerm[ColPhase] - 1] * BriVal));

                // do the color progression
                ColPhase++;
                ColPhase %= maxcols;
                /*switch (ColPhase){
                    case 0:
                        if (GreenVal + ColDelta <= 1)
                        {
                            GreenVal += ColDelta;
                        }
                        else 
                        {
                            ColPhase = 1;
                            GreenVal = 1;
                            RedVal -= ColDelta;
                        }
                        break;
                    case 1:
                        if (RedVal - ColDelta >= 0)
                        {
                            RedVal -= ColDelta;
                        }
                        else
                        {
                            ColPhase = 2;
                            RedVal = 0;
                            BlueVal = ColDelta;
                        }
                        break;
                    case 2:
                        if (BlueVal + ColDelta <= 1)
                        {
                            BlueVal += ColDelta;
                        }
                        else
                        {
                            ColPhase = 3;
                            BlueVal = 1;
                            GreenVal -= ColDelta;
                        }
                        break;
                    case 3:
                        if (GreenVal - ColDelta >=0)
                        {
                            GreenVal -= ColDelta;
                        }
                        else
                        {
                            ColPhase = 4;
                            GreenVal = 0;
                            RedVal = ColDelta;
                        }
                        break;
                    case 4:
                        if (RedVal + ColDelta <= 1)
                        {
                            RedVal += ColDelta;
                        }
                        else
                        {
                            ColPhase = 5;
                            RedVal = 1;
                            BlueVal = BlueVal - ColDelta;
                        }
                        break;
                    case 5:
                        if (BlueVal - ColDelta >= 0)
                        {
                            BlueVal -= ColDelta;
                        }
                        else
                        {
                            ColPhase = 0;
                            BlueVal = 0;
                            GreenVal = ColDelta;
                        }
                        break;
                }*/
                switch (BrPhase)
                {
                    case 0:
                        BrPhase = 1;
                        BriVal = 0.8;
                        break;
                    case 1:
                        BrPhase = 2;
                        BriVal = 0.6;
                        break;
                    case 2:
                        BrPhase = 0;
                        BriVal = 1;
                        break;
                }
            }
            //MessageBox.Show(message);
            return List;
        }

        private void StandardColorFactors()
        {
            if (FoundFactors != null)
            {
                long ffrows = FoundFactors.GetLength(0);
                long ffcols = FoundFactors.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundFactors[i, j] != null)
                        {
                            SolidColorBrush myBrush = new SolidColorBrush();
                            myBrush.Color = Color.FromRgb(100, 100, 100);
                            FoundFactors[i, j].Fill=myBrush;
                            FoundFactors[i, j].StrokeThickness = 0;
                        }
                    }
                }
            }
        }

        private void StandardToolTipFactors()
        {
            if (FoundFactors != null)
            {
                long ffrows = FoundFactors.GetLength(0);
                long ffcols = FoundFactors.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundFactors[i, j] != null)
                        {
                            FoundFactors[i, j].ToolTip = null; ;
                        }
                    }
                }
            }
        }
        private void StandardColorProducts()
        {
            if (FoundProducts != null)
            {
                long ffrows = FoundProducts.GetLength(0);
                long ffcols = FoundProducts.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundProducts[i, j] != null)
                        {
                            SolidColorBrush myBrush = new SolidColorBrush();
                            myBrush.Color = Color.FromRgb(150, 150, 150);
                            FoundProducts[i, j].Fill = myBrush;
                            FoundProducts[i, j].StrokeThickness = 0;
                        }
                    }
                }
            }
        }
        private void StandardOutlineProducts()
        {
            if (FoundProducts != null)
            {
                long ffrows = FoundProducts.GetLength(0);
                long ffcols = FoundProducts.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundProducts[i, j] != null)
                        {
                            SolidColorBrush LineBrush = new SolidColorBrush();
                            LineBrush.Color = Color.FromRgb(255, 255, 255);
                            FoundProducts[i, j].Stroke = LineBrush;
                            FoundProducts[i, j].StrokeThickness = 0;
                        }
                    }
                }
            }
        }
        private void StandardOutlineFactors()
        {
            if (FoundFactors != null)
            {
                long ffrows = FoundFactors.GetLength(0);
                long ffcols = FoundFactors.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundFactors[i, j] != null)
                        {
                            SolidColorBrush LineBrush = new SolidColorBrush();
                            LineBrush.Color = Color.FromRgb(255, 255, 255);
                            FoundFactors[i, j].Stroke = LineBrush;
                            FoundFactors[i, j].StrokeThickness = 0;
                        }
                    }
                }
            }
        }
        private void RecolorFactors(ComplNum Prod, ComplFactors Factors)
        {
            if (FoundFactors != null)
            {
                // cleanup before special coloring
                StandardColorFactors();
                StandardOutlineFactors();
                StandardOutlineProducts();
                StandardToolTipFactors();

                //actual re-color action
                double diameter = GetCurrentDiameter();
                // mark the selected product
                if (FoundProducts != null)
                {
                    FoundProducts[Prod.Im + size, Prod.Re + size].StrokeThickness = diameter*smallmargfract;
                }
                //mark factors
                long FactNum = Factors.NumFactors;
                SolidColorBrush[] BrushList = Colors(FactNum);
                long ColInd = 0;
                FactorList CurFactPair = Factors.First;
                while (CurFactPair != null)
                {
                    if (CurFactPair.val1.Equal(CurFactPair.val2))
                    {
                        long i = CurFactPair.val1.Im + size;
                        long j = CurFactPair.val1.Re + size;
                        if (FoundFactors[i, j] != null)
                        {
                            FoundFactors[i, j].StrokeThickness = diameter * smallmargfract;
                            FoundFactors[i, j].Fill = BrushList[ColInd];
                            FoundFactors[i, j].ToolTip = "Factor "+CurFactPair.val1.ToString() +
                                " (both factors of " + Prod.ToString() + " identical)";
                        }
                    }
                    else
                    {
                        long i = CurFactPair.val1.Im + size;
                        long j = CurFactPair.val1.Re + size;
                        if (FoundFactors[i,j] != null)
                        {
                            FoundFactors[i, j].Fill = BrushList[ColInd];
                        }
                        FoundFactors[i, j].ToolTip = "Factor " + CurFactPair.val1.ToString() +
                                " (corresponding factor of " + Prod.ToString() + ": "
                                + CurFactPair.val2.ToString() + ").\n Click to show corresponding factor.";
                        i = CurFactPair.val2.Im + size;
                        j = CurFactPair.val2.Re + size;
                        if (FoundFactors[i, j] != null)
                        {
                            FoundFactors[i, j].Fill = BrushList[ColInd];
                        }
                        FoundFactors[i, j].ToolTip = "Factor " + CurFactPair.val2.ToString() +
                                " (corresponding factor of " + Prod.ToString() + ": "
                                + CurFactPair.val1.ToString() + ").\n Click to show corresponding factor.";

                    }
                    ColInd++;
                    CurFactPair = CurFactPair.next;
                }
            }
        }
        private void ProdIndicator_Click(object sender, MouseButtonEventArgs e)
        {
            // search for clicked ellipse in array to get the corresponding number
            if (FoundProducts != null)
            {
                SelectedProd = FindIndicator((Ellipse)sender, FoundProducts);
                if (SelectedProd != null) //found?
                {
                    // get all factor pairs
                    if (Factors != null)
                    {
                        ComplFactors CurFactors = Factors.GetFactors(SelectedProd);
                        if (CurFactors != null)
                        {// color all factors w/ equal colors for factor pairs and special markings for "double" factors
                            RecolorFactors(SelectedProd, CurFactors);
                        }
                    }
                }
            }

        }

        private void FactIndicator_Click(object sender, MouseButtonEventArgs e)
        {
            if (SelectedProd != null)// react only if previously some number has been selected as product
            {
                // search for clicked ellipse in array to get the corresponding number
                if (FoundFactors != null)
                {

                    ComplNum SelectedFactor = FindIndicator((Ellipse)sender, FoundFactors);
                    if (SelectedFactor != null)//found? 
                    {
                        // get all factor pairs
                        if (Factors != null)
                        {
                            ComplFactors CurFactors = Factors.GetFactors(SelectedProd);
                            if (CurFactors != null)
                            {
                                //look if the currently selected number is actually a factor of the previously selected product
                                FactorList CurFactPair = CurFactors.First;
                                //get current necessary thickness of edge to be seen and not be too big.
                                double EdgeThickness = GetCurrentDiameter() * smallmargfract;
                                while (CurFactPair != null) //go through *all* factor pairs to clean up edges from possible previous factor selection
                                {
                                    //clean up any previous black edges from non-double factors
                                    if (!CurFactPair.val1.Equal(CurFactPair.val2))
                                    {
                                        FoundFactors[CurFactPair.val1.Im + size, CurFactPair.val1.Re + size].StrokeThickness = 0;
                                        FoundFactors[CurFactPair.val2.Im + size, CurFactPair.val2.Re + size].StrokeThickness = 0;
                                    }
                                    // is the lastly selected number part of the current factor pair?
                                    if (CurFactPair.val1.Equal(SelectedFactor))
                                    {
                                        // only need to do sth if no "double factor"
                                        if (!CurFactPair.val2.Equal(SelectedFactor))
                                        {
                                            // recolor current and corresponding factor's inner edge
                                            SolidColorBrush myBrush = new SolidColorBrush();
                                            myBrush.Color = Color.FromRgb(0, 0, 0);
                                            ((Ellipse)sender).Stroke = myBrush;
                                            ((Ellipse)sender).StrokeThickness = EdgeThickness;
                                            FoundFactors[CurFactPair.val2.Im + size, CurFactPair.val2.Re + size].Stroke = myBrush;
                                            FoundFactors[CurFactPair.val2.Im + size, CurFactPair.val2.Re + size].StrokeThickness = EdgeThickness;
                                        }
                                    } else if (CurFactPair.val2.Equal(SelectedFactor))
                                    {
                                        //possibility of "double factor" already excluded
                                        // recolor current and corresponding factor's inner edge
                                        SolidColorBrush myBrush = new SolidColorBrush();
                                        myBrush.Color = Color.FromRgb(0, 0, 0);
                                        ((Ellipse)sender).Stroke = myBrush;
                                        ((Ellipse)sender).StrokeThickness = EdgeThickness;
                                        FoundFactors[CurFactPair.val1.Im + size, CurFactPair.val1.Re + size].Stroke = myBrush;
                                        FoundFactors[CurFactPair.val1.Im + size, CurFactPair.val1.Re + size].StrokeThickness = EdgeThickness;
                                    }
                                    else
                                    {
                                        //if current selected factor is not part 
                                    }
                                    CurFactPair = CurFactPair.next;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeResultIndicators();
        }

        private void ResizeResultIndicators()
        {
            // re-size and re-position result indicators when size of window
            // and thereby possibly also size of canvas has changed.
            double diameter;

            diameter = GetCurrentDiameter();
            if (FoundProducts != null)
            {
                long fprows = FoundProducts.GetLength(0);
                long fpcols = FoundProducts.GetLength(1);
                for (long i = 0; i < fprows; i++)
                {
                    for (long j = 0; j < fpcols; j++)
                    {
                        if (FoundProducts[i, j] != null)
                        {
                            FoundProducts[i, j].Width = diameter * (1 - 2 * smallmargfract);
                            FoundProducts[i, j].Height = diameter * (1 - 2 * smallmargfract);
                            Canvas.SetBottom(FoundProducts[i, j], (i * diameter) + (diameter * smallmargfract));
                            Canvas.SetLeft(FoundProducts[i, j], (j * diameter) + (diameter * smallmargfract));
                            if (FoundProducts[i, j].StrokeThickness > 0)
                            {
                                FoundProducts[i, j].StrokeThickness = diameter * smallmargfract;
                            }
                        }
                    }
                }
            }
            if (FoundFactors != null)
            {
                long ffrows = FoundFactors.GetLength(0);
                long ffcols = FoundFactors.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundFactors[i, j] != null)
                        {
                            FoundFactors[i, j].Width = diameter * (1 - 2 * bigmargfract);
                            FoundFactors[i, j].Height = diameter * (1 - 2 * bigmargfract);
                            Canvas.SetBottom(FoundFactors[i, j], (i * diameter) + (diameter * bigmargfract));
                            Canvas.SetLeft(FoundFactors[i, j], (j * diameter) + (diameter * bigmargfract));
                            if (FoundFactors[i, j].StrokeThickness > 0)
                            {
                                FoundFactors[i, j].StrokeThickness = diameter * smallmargfract;
                            }
                        }
                    }
                }
            }
        }

        private void ComparatorSel_Click(object sender, RoutedEventArgs e)
        {
            SearchLEq = !SearchLEq;
            if (SearchLEq)
            {
                ComparatorSel.Content = "≤";
            }
            else
            {
                ComparatorSel.Content = "=";
            }
            if ((FactNumMinus.IsEnabled) || (FactNumPlus.IsEnabled))
            {
                ShowFactors.IsEnabled = true;
            }
        }

        private Ellipse ResultIndicator(double space, double marginfract, long frombottom, long fromleft, 
            byte r, byte g, byte b, bool isproduct, Canvas DrawGrid)
        {
            Ellipse Indicator = new Ellipse();
            Indicator.Width = space * (1 - 2 * marginfract);
            Indicator.Height = space * (1 - 2 * marginfract);
            SolidColorBrush LineBrush = new SolidColorBrush();
            LineBrush.Color = Color.FromRgb(255, 255, 255);
            Indicator.Stroke = LineBrush;
            Indicator.StrokeThickness = 0;
            SolidColorBrush MyBrush = new SolidColorBrush();
            MyBrush.Color = Color.FromRgb(r, g, b);
            Indicator.Fill = MyBrush;
            Canvas.SetBottom(Indicator, (frombottom * space) + (space * marginfract));
            Canvas.SetLeft(Indicator, (fromleft * space) + (space * marginfract));
            DrawGrid.Children.Add(Indicator);
            if (isproduct)
            {
                Indicator.MouseLeftButtonDown += new MouseButtonEventHandler(this.ProdIndicator_Click);
            }
            else
            {
                Indicator.MouseLeftButtonDown += new MouseButtonEventHandler(this.FactIndicator_Click);
            }
            
            return Indicator;
        }

        private void CleanUpDrawingElements()
        {
            CorrectFactorNum = null;
            if (FoundProducts != null)
            {
                long fprows = FoundProducts.GetLength(0);
                long fpcols = FoundProducts.GetLength(1);
                for (long i = 0; i < fprows; i++)
                {
                    for (long j = 0; j < fpcols; j++)
                    {
                        if (FoundProducts[i, j] != null)
                        {
                            NumGrid.Children.Remove(FoundProducts[i, j]);
                            FoundProducts[i, j] = null;
                        }
                    }
                }
            }
            FoundProducts = null;
            if (FoundFactors != null)
            {
                long ffrows = FoundFactors.GetLength(0);
                long ffcols = FoundFactors.GetLength(1);
                for (long i = 0; i < ffrows; i++)
                {
                    for (long j = 0; j < ffcols; j++)
                    {
                        if (FoundFactors[i, j] != null)
                        {
                            NumGrid.Children.Remove(FoundFactors[i, j]);
                            FoundFactors[i, j] = null;
                        }
                    }
                }
            }
            FoundFactors = null;
        }

        private double GetCurrentDiameter()
        {
            //actual new drawing
            double mincanvascoord = Math.Min(NumGrid.ActualWidth, NumGrid.ActualHeight);
            double diameter = mincanvascoord / (2 * size + 1);
            return diameter;
        }

        private void ShowFactors_Click(object sender, RoutedEventArgs e)
        {
            double diameter;
            bool critfulfilled;
            ComplFactors CurrentFactors;
            //make button unavailable
            ShowFactors.IsEnabled = false;

            //clean up from possible previous drawing
            CleanUpDrawingElements();
            SelectedProd = null;

            //actual new drawing
            diameter = GetCurrentDiameter();


            //actual show action
            FoundProducts = new Ellipse[2 * size + 1, 2 * size + 1];
            FoundFactors = new Ellipse[2 * size + 1, 2 * size + 1];
            CorrectFactorNum = new bool[2 * size + 1, 2 * size + 1];
            for (long i = 0; i < ((2 * size) + 1); i++)
            {
                for (long j = 0; j < ((2 * size) + 1); j++)
                {
                    CorrectFactorNum[i, j] = false; // general assumption: current number does *not* fulfill the search criterion for the number of factor pairs.
                    if ((j - size == 0) && (i - size == 0))
                    {
                        // special marking for origin
                        FoundProducts[i, j] = ResultIndicator(diameter, smallmargfract, i, j, 100, 255, 100, true, NumGrid);
                        FoundProducts[i, j].ToolTip = "0+0i (origin, #of factor pairs infinite)";
                        FoundFactors[i, j] = ResultIndicator(diameter, bigmargfract, i, j, 50, 255, 50, false, NumGrid);
                    }
                    else
                    {
                        ComplNum CurProd = new ComplNum(j - size, i - size);
                        if (this.Factors != null)
                        {
                            CurrentFactors = Factors.GetFactors(CurProd);
                            if (SearchLEq)
                            {
                                critfulfilled = ((CurrentFactors.NumFactors >= minfactors) && (CurrentFactors.NumFactors <= maxfactors));
                            }
                            else
                            {
                                critfulfilled = (CurrentFactors.NumFactors == maxfactors);
                            }
                            //if ((CurrentFactors.NumFactors>=minfactors)&& (CurrentFactors.NumFactors <= maxfactors))
                            if (critfulfilled)
                            {
                                // actions if current number turns out to fulfill the search criterion
                                FoundProducts[i, j] = ResultIndicator(diameter, smallmargfract, i, j, 100, 100, 255, true, NumGrid);
                                CorrectFactorNum[i, j] = true;
                            }
                            else
                            {
                                FoundProducts[i, j] = ResultIndicator(diameter, smallmargfract, i, j, 150, 150, 150, true, NumGrid);
                            }
                            FoundFactors[i, j] = ResultIndicator(diameter, bigmargfract, i, j, 100, 100, 100, false, NumGrid);
                            FoundProducts[i, j].ToolTip = CurProd.ToString()+" # Factor Pairs: "+ CurrentFactors.NumFactors+".\nClick to show factor pairs.";
                        }
                        else
                        {
                            // fall-back if something is wrong with the data base
                            FoundProducts[i, j] = ResultIndicator(diameter, smallmargfract, i, j, 255, 100, 100, true, NumGrid);
                            FoundProducts[i, j].ToolTip = CurProd.ToString() + " (No Info on Factors found)";
                            FoundFactors[i, j] = ResultIndicator(diameter, bigmargfract, i, j, 255, 50, 50, false, NumGrid);
                        }
                    }
                }

            }
        }
    }
}

