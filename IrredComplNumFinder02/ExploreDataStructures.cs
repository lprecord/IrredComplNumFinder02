using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IrredComplNumFinder02
{
    public class FactorList
    {
        public ComplNum val1, val2;
        public FactorList next;

        public FactorList()
        {
            val1 = new ComplNum();
            val2 = new ComplNum();
            next = null;
        }
    }
    public class ComplFactors
    {
        public FactorList First, Last;
        public long NumFactors;

        public ComplFactors()
        {
            First = null;
            Last = null;
            NumFactors = 0;
        }

        public void Append(ComplNum fact1, ComplNum fact2)
        {
            NumFactors++;
            if (First == null)
            {
                First = new FactorList();
                Last = First;
            }
            else
            {
                Last.next = new FactorList();
                Last = Last.next;
            }
            try
            {
                Last.val1.Assign(fact1);
                Last.val2.Assign(fact2);
            }
            catch { MessageBox.Show("NullException in ComplFactors.Append"); }
        }

        public bool AppendNoRedundancy(ComplNum fact1, ComplNum fact2)
        {
            bool success = false;
            bool notfound = false;
            FactorList current = First;

            try
            {
                notfound = true;
                while (notfound && (current != null))
                {
                    if ((current.val1.Equal(fact1) && current.val2.Equal(fact2)) || (current.val1.Equal(fact2) && current.val2.Equal(fact1)))
                        notfound = false;
                    current = current.next;
                }
            }
            catch { MessageBox.Show("NullException in ComplFactors.AppendNoRedundancy during check"); }
            if (notfound)
            {
                try
                {
                    NumFactors++;
                    if (First == null)
                    {
                        First = new FactorList();
                        Last = First;
                    }
                    else
                    {
                        Last.next = new FactorList();
                        Last = Last.next;
                    }
                    Last.val1.Assign(fact1);
                    Last.val2.Assign(fact2);
                    success = true;
                }
                catch { MessageBox.Show("NullException in ComplFactors.AppendNoRedundancy during assignment"); }
            }
            return success;
        }


        public ComplFactors(ComplNum InputProd)
        {
            double AbsVal;
            uint OuterPhase;
            long Size1, Size2, Size3, OuterSize;
            long SearchReAbs1, SearchImAbs1, SearchReAbs2, SearchImAbs2;
            ComplNum Fact1, Fact2, Prod;

            AbsVal = 0;
            Size1 = 0;
            Size2 = 0;
            Size3 = 0;
            Fact1 = null;
            Fact2 = null;
            Prod = new ComplNum();

            try
            {
                AbsVal = InputProd.Absolute();
                Size3 = (long)Math.Ceiling(AbsVal);
                Size2 = (long)Math.Floor(Math.Sqrt(AbsVal)* Math.Sqrt(0.5));
                Prod.Assign(InputProd);
            }
            catch { MessageBox.Show("NullException in ComplFactors constructor"); }
            First = null;
            Last = null;
            NumFactors = 0;

            for (OuterSize = Size2; OuterSize <= Size3; OuterSize++)
            {
                OuterPhase = 0;
                SearchReAbs1 = -OuterSize;
                SearchImAbs1 = -OuterSize;
                Size1 = (long)Math.Ceiling(AbsVal/((double)OuterSize));
                while (OuterPhase<4)
                {
                    for (SearchReAbs2 = 0; SearchReAbs2 <= Size1; SearchReAbs2++)
                    {
                        for (SearchImAbs2 = 0; SearchImAbs2 <= Size1; SearchImAbs2++)
                        {
                            //Test with all sign combinations
                            Fact1 = new ComplNum(SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1,Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }
                            Fact1 = new ComplNum(-SearchReAbs1, -SearchImAbs1);
                            Fact2 = new ComplNum(-SearchReAbs2, -SearchImAbs2);
                            if (Prod.Equal(ComplNumCalc.Mul(Fact1, Fact2)))
                            {
                                this.AppendNoRedundancy(Fact1, Fact2);
                            }

                        }

                    }
                    if (OuterPhase == 0)
                    {
                        if (SearchImAbs1 < OuterSize)
                            SearchImAbs1++;
                        else
                            OuterPhase++;
                    }
                    else if (OuterPhase==1)
                    {
                        if (SearchReAbs1 < OuterSize)
                            SearchReAbs1++;
                        else
                            OuterPhase++;
                    }
                    else if (OuterPhase == 2)
                    {
                        if (SearchImAbs1 > -OuterSize)
                            SearchImAbs1--;
                        else
                            OuterPhase++;
                    }
                    else if (OuterPhase == 3)
                    {
                        if (SearchReAbs1 > -OuterSize)
                            SearchReAbs1--;
                        else
                            OuterPhase++;
                    }

                }

            }

        }

    }

    public class ProdGrid
    {
        ComplFactors[,] FactorSets;
        long GridSize;
        ComplNum LowerLeftCorner;
        private long minFactNum;

        public long GetMinFactNum()
        {
            return minFactNum;
        }

        public ProdGrid(long size, iProgressIndicator ProgressReceptor)
        {
            long RIndex = 0;
            long IIndex = 0;
            minFactNum = long.MaxValue;
            GridSize = size;
            LowerLeftCorner = new ComplNum(-size, -size);
            long ArrSize = (2 * size + 1);
            FactorSets = new ComplFactors[ArrSize, ArrSize] ;
            double ItemsToAnalyze = ArrSize * ArrSize;
            for (RIndex = 0; RIndex < ArrSize; RIndex++)
            {
                for (IIndex = 0; IIndex < ArrSize; IIndex++)
                {
                    FactorSets[RIndex, IIndex] = new ComplFactors(new ComplNum(LowerLeftCorner.Re + RIndex, LowerLeftCorner.Im + IIndex));
                    try
                    {
                        minFactNum = Math.Min(minFactNum, FactorSets[RIndex, IIndex].NumFactors);
                    }
                    catch 
                    { 
                        MessageBox.Show("NullException in ProdGrid Constructor after call to ComplFactors constructor");
                        //indicate problem during search
                        ProgressReceptor.UpdateProgress(-1);
                    }
                }
                double CurrentProgress = ((RIndex * ArrSize) + IIndex) / ItemsToAnalyze;
                ProgressReceptor.UpdateProgress(CurrentProgress);
            }
            //indicate that searcn is not running anymore
            ProgressReceptor.UpdateProgress(2);
        }

        public ComplFactors GetFactors(ComplNum Prod)
        {
            if(Math.Abs(Prod.Re)>GridSize|| Math.Abs(Prod.Re) > GridSize)
            {
                return null;
            }
            else
            {
                long RIndex = Prod.Re - LowerLeftCorner.Re;
                long IIndex = Prod.Im - LowerLeftCorner.Im;
                return FactorSets[RIndex, IIndex];
            }
        }
    }
}
