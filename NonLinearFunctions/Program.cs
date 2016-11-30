using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Differentiation;
using MathNet.Numerics.LinearRegression;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;
using Eval = MathNet.Symbolics.Evaluate;
using MathNet.Numerics.LinearAlgebra.Double;

namespace NonLinearFunctions
{
    class Program
    {
        public static void CalculateEquation( ref Dictionary<string,FloatingPoint> initialVector,int i)
        {
            var df1x = Calculus.Differentiate(x, f1);
            var df1y = Calculus.Differentiate(y, f1);
            var df2x = Calculus.Differentiate(x, f2);
            var df2y = Calculus.Differentiate(y, f2);
            
         
            double j11 = Eval.Evaluate(initialVector, df1x).RealValue;
            double j12 = Eval.Evaluate(initialVector, df1y).RealValue;
            double j21 = Eval.Evaluate(initialVector, df2x).RealValue;
            double j22 = Eval.Evaluate(initialVector, df2y).RealValue;
            //Console.WriteLine(Infix.FormatStrict(df1x) + " " + Infix.Format(df1y) + " " + Infix.Format(df2x) + " " + Infix.Format(df2y));

            double[] m = { j11, j21, j12, j22 };
            Matrix<double> jacobian = new DenseMatrix(2, 2, m);
            Matrix<double> initial = new DenseMatrix(2, 1, new double[] { Eval.Evaluate(initialVector, f1).RealValue, Eval.Evaluate(initialVector, f2).RealValue });

            jacobian = jacobian.Inverse();

            var solution = jacobian * (-initial);

            var xn = x0 + solution[0,0];
            var yn = y0 + solution[1,0];

            var outVal = new Dictionary<string, FloatingPoint> {
                {"x",xn}, {"y",yn}
            };
            Console.WriteLine("x" + i.ToString() + " = " + xn + Environment.NewLine + "y"+i.ToString()+" = " + yn);
            Console.WriteLine("f1(x"+i+",y"+i+")="+Math.Round( Eval.Evaluate(initialVector,f1).RealValue,3));
            Console.WriteLine("f2(x" + i+",y" + i + ")=" + Eval.Evaluate(initialVector, f2).RealValue);
            
          
            initialVector = outVal;
            x0 = xn;
            y0 = yn;
        }
        public static void initFunctions()
        {
            x0 = 1;
            y0 = 0;
            x = Expr.Symbol("x");
            y = Expr.Symbol("y");
            //Functions should be declared before compilation
            f1 = Expr.Sin(x) + y - 2;
            f2 = 2 * Expr.Pow(x, 2) * +y - 4;

        }
        public static double x0;
        public static double y0;
        public static Expr f1;
        public static Expr f2;
        public static Expr x;
        public static Expr y;
        static void Main(string[] args)
        {
            initFunctions();
            var initialVector = new Dictionary<string, FloatingPoint>
            {
                {"x",x0 },
                {"y",y0 }
            };
            for (int i = 1; i < 6; i++)
            {
                CalculateEquation(ref initialVector, i);

            }
                
            Console.ReadKey();
        }
    }
}
