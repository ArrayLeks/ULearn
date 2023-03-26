using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class Risovatel
    {
        static float x, y;
        static Graphics graphic;

        public static void Initialize(Graphics newGraphic)
        {
            graphic = newGraphic;
            graphic.SmoothingMode = SmoothingMode.None;
            graphic.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        { x = x0; y = y0; }

        public static void MakeIt(Pen pen, double length, double angle)
        {
            //Делает шаг длиной dlina в направлении ugol и рисует пройденную траекторию
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            graphic.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Change(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle));
            y = (float)(y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int hight, double turnAngle, Graphics graphic)
        {
            // ugolPovorota пока не используется, но будет использоваться в будущем
            Risovatel.Initialization(graphic);

            var size = Math.Min(width, hight);
            float SX = 0.375f;

            var diagonal_length = Math.Sqrt(2) * (size * 0.375f + size * 0.04f) / 2;
            var x0 = (float)(diagonal_length * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonal_length * Math.Sin(Math.PI / 4 + Math.PI)) + hight / 2f;

            Risovatel.set_position(x0, y0);

            //Рисуем 1-ую сторону
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, 0);
            Risovatel.makeIt(Pens.Yellow, size * 0.04f * Math.Sqrt(2), Math.PI / 4);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, Math.PI);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f - size * 0.04f, Math.PI / 2);

            Risovatel.Change(size * 0.04f, -Math.PI);
            Risovatel.Change(size * 0.04f * Math.Sqrt(2), 3 * Math.PI / 4);

            //Рисуем 2-ую сторону
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, -Math.PI / 2);
            Risovatel.makeIt(Pens.Yellow, size * 0.04f * Math.Sqrt(2), -Math.PI / 2 + Math.PI / 4);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, -Math.PI / 2 + Math.PI);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f - size * 0.04f, -Math.PI / 2 + Math.PI / 2);

            Risovatel.Change(size * 0.04f, -Math.PI / 2 - Math.PI);
            Risovatel.Change(size * 0.04f * Math.Sqrt(2), -Math.PI / 2 + 3 * Math.PI / 4);

            //Рисуем 3-ю сторону
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, Math.PI);
            Risovatel.makeIt(Pens.Yellow, size * 0.04f * Math.Sqrt(2), Math.PI + Math.PI / 4);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, Math.PI + Math.PI);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f - size * 0.04f, Math.PI + Math.PI / 2);

            Risovatel.Change(size * 0.04f, Math.PI - Math.PI);
            Risovatel.Change(size * 0.04f * Math.Sqrt(2), Math.PI + 3 * Math.PI / 4);

            //Рисуем 4-ую сторону
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, Math.PI / 2);
            Risovatel.makeIt(Pens.Yellow, size * 0.04f * Math.Sqrt(2), Math.PI / 2 + Math.PI / 4);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f, Math.PI / 2 + Math.PI);
            Risovatel.makeIt(Pens.Yellow, size * 0.375f - size * 0.04f, Math.PI / 2 + Math.PI / 2);

            Risovatel.Change(size * 0.04f, Math.PI / 2 - Math.PI);
            Risovatel.Change(size * 0.04f * Math.Sqrt(2), Math.PI / 2 + 3 * Math.PI / 4);
        }
    }
}