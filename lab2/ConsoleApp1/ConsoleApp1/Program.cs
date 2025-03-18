using System;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Rectangle myRectangle = new Rectangle();

            myRectangle.x = -5;                     
            myRectangle.y = 4;
            myRectangle.width = 3;
            myRectangle.height = 2;
            Console.WriteLine("исходный прямоугольник " + myRectangle.GetRectangleDescription());
            //переместим прямоугольник с помощью метода
            myRectangle.MoveToNewPosition(1, 1);
            Console.WriteLine("перемещенный прямоугольник " + myRectangle.GetRectangleDescription());
            myRectangle.Resize(5, 6);
            Console.WriteLine("прямоугольник с новым размером " + myRectangle.GetRectangleDescription());
            //найдем прямоугольник включающий в себя два других прямоугольнкиа 
            Rectangle rectangle1 = new Rectangle(1,5,2,1);
            Rectangle rectangle2 = new Rectangle(4,2,2,1);
            Rectangle container = Rectangle.ContainerRectangle(rectangle1, rectangle2);
            Console.WriteLine(("описаный прямоугольник " + container.GetRectangleDescription()));
            Rectangle rectangle3 = new Rectangle(1, 5, 3, 2);            
            Rectangle rectangle4 = new Rectangle(2, 4, 3, 2);
            Rectangle intersection = Rectangle.IntersectionOfRectangle(rectangle3, rectangle4);
            if(intersection == null)
            {
                Console.WriteLine("прямоугольники не пересекаються");
            }
            else
            {
                Console.WriteLine(("пересечение прямоугольников " + intersection.GetRectangleDescription()));
            }
            

        }
    }
    public class Rectangle 
    {
        //прямоугольник задаеться координатами левого верхнего угла, шириной и высотой 
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        //координаты остальных 3-х углов 
        public int positionRightTopX { get { return x + width; } }
        public int positionRightTopY { get { return y; } }
        public int positionLeftBottomX { get { return x ; } }
        public int positionLeftBottomY { get { return y - height; } }
        
        public int positionRightBottomX { get { return x + width; } }
        public int positionRightBottomY { get { return y - height; } }

        //конструктор без аргументов 
        public Rectangle()
        {
        }

        //задаем конструктор со всеми координатами прямоугольника
        public Rectangle(int x, int y, int width, int height)
        {

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        // переместить прямоугольник в заданные координаты
        public void MoveToNewPosition(int newX, int newY)
        {
            this.x = newX;
            this.y = newY;

        }

        //переместить прямоугольник задава смещение по осям х у
        public void MoveWithOffset(int offsetX, int offsetY)
        {
            this.x += offsetX;
            this.y += offsetY;
        }

        public void Resize ( int newWidth, int newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
        }
        
        public static Rectangle ContainerRectangle(Rectangle rectangle1, Rectangle rectangle2)
        {
            //вычеслим координаты углов нового прямоугольнька вмещающего 2 заданых
            //находим левый верхний угол 
            int topLeftX = Math.Min(rectangle1.x, rectangle2.x);
            int topLeftY = Math.Max(rectangle1.y, rectangle2.y);
            //находим правый нижний угол
            int bottomRightX = Math.Max(rectangle1.positionRightBottomX, rectangle2.positionRightBottomX);
            int bottomRightY = Math.Min(rectangle1.positionRightBottomY, rectangle2.positionRightBottomY);

            Rectangle newRectangle = new Rectangle();
            newRectangle.x = topLeftX;
            newRectangle.y = topLeftY;
            newRectangle.width = bottomRightX - topLeftX;
            newRectangle.height = topLeftY - bottomRightY;
            return newRectangle;


        }
        public static Rectangle IntersectionOfRectangle(Rectangle rectangle1, Rectangle rectangle2)
        {
            //находим левую границу правее которой присутсвуют оба прямоугольника
            int x1 = Math.Max(rectangle1.x, rectangle2.x);

            //находим верхнюю границу ниже которой пристутсвуют оба прямоугольника
            int y1 = Math.Min(rectangle1.y, rectangle2.y);

            //находим правую границу левее которой находяться оба прямоугольника
            int x2 = Math.Min(rectangle1.positionRightTopX , rectangle2.positionRightTopX);
            
            //находим нижнюю границу выше которой находяться оба прямоугольника 
            int y2 = Math.Max(rectangle1.positionRightBottomY, rectangle2.positionRightBottomY);
            //если найденая левая граница левее правой граници,а верхняя выше нижней границы то прямоугольники пересекаються
            if (x1 < x2 && y1 > y2)
            {
                return new Rectangle(x1, y1, x2 - x1, y1 - y2);
            }
            else
            {
                //прямоугольники не пересекаются
                return null;
            }
        }
        //метод для вывода описания прямоугольника 
        public string GetRectangleDescription()
        {
            return string.Format("Rectangle left top corner is ({0},{1}),width {2},height {3}", this.x, this.y, this.width, this.height);
        }
            
    }
}


