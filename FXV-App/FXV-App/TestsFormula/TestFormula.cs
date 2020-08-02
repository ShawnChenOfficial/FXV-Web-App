using System;
namespace FXV.TestsFormula
{
    public class TestFormula
    {
        private double _x1 { get; set; } // lowerest result
        private double _x2 { get; set; }// highest result
        private int _y1 { get; set; }// lowerest score
        private int _y2 { get; set; }//highest score
        private double _RealResult { get; set; }

        public TestFormula(double x1, double x2, int y1, int y2, double RealResult)
        {
            this._x1 = x1;
            this._x2 = x2;
            this._y1 = y1;
            this._y2 = y2;
            this._RealResult = RealResult;
        }


        public int GetFinalScore()
        {
            return (int)Math.Round(GetScore());
        }

        public double GetFactorA()
        {
            var top = (_y1 + _y2) * (_x1 * _x1 + _x2 * _x2) - (_x1 + _x2) * (_x1 * _y1 + _x2 * _y2);
            var bottom = 2 * (_x1 * _x1 + _x2 * _x2) - (_x1 + _x2) * (_x1 + _x2);

            return top / bottom;
        }
        public double GetFactorB()
        {
            var top = 2 * (_x1 * _y1 + _x2 * _y2) - (_x1 + _x2) * (_y1 + _y2);
            var bottom = 2 * (_x1 * _x1 + _x2 * _x2) - (_x1 + _x2) * (_x1 + _x2);

            return top / bottom;
        }
        private double GetScore()
        {
            var a = GetFactorA();
            var b = GetFactorB();
            return a + b * _RealResult;
        }
    }
}
