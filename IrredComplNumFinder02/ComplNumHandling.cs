using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IrredComplNumFinder02
{
    public class ComplNum
    {
        public long Re, Im;

        public ComplNum()
        {
            Re = 0;
            Im = 0;
        }

        public ComplNum(long Real, long Imaginary)
        {
            Re = Real;
            Im = Imaginary;
        }

        public void Assign(ComplNum val)
        {
            try
            {
                Re = val.Re;
                Im = val.Im;
            }
            catch
            { MessageBox.Show("NullException in ComplNum.Assign"); }
        }

        public double Absolute()
        {
            return Math.Sqrt((double)((Re * Re) + (Im * Im)));
        }

        public bool Equal(ComplNum Comparator)
        {
            bool Result = false;
            try
            {
                if ((Comparator.Re == Re) && (Comparator.Im == Im))
                    Result = true;
            }
            catch {
                MessageBox.Show("NullException in ComplNum.Equal");
            }
            return Result;
        }

        public override string ToString()
        {
            string result = Re.ToString();
            if (Im < 0)
            {
                result += (Im.ToString() + "i");
            }
            else
            {
                result += ("+" + Im.ToString() + "i");
            }
            return result;
        }
    }

    public static class ComplNumCalc
    {
        
        public static ComplNum Add(ComplNum a, ComplNum b)
        {
            ComplNum Result=new ComplNum();
            try
            {
                Result.Re = a.Re + b.Re;
                Result.Im = a.Im + b.Im;
            }
            catch
            { MessageBox.Show("NullException in ComplNumCalc.Add"); }
            return Result;
        }

        public static ComplNum Sub(ComplNum a, ComplNum b)
        {
            ComplNum Result = new ComplNum();
            try
            {
                Result.Re = a.Re - b.Re;
                Result.Im = a.Im - b.Im;
            }
            catch { MessageBox.Show("NullException in ComplNumCalc.Sub"); }
            return Result;
        }

        public static ComplNum Mul(ComplNum a, ComplNum b)
        {
            ComplNum Result = new ComplNum();
            try
            {
                Result.Re = (a.Re * b.Re) - (a.Im * b.Im);
                Result.Im = (a.Im * b.Re) + (b.Im * a.Re);
            }
            catch { MessageBox.Show("NullException in ComplNumCalc.Mul"); }
            return Result;
        }
    }
}
