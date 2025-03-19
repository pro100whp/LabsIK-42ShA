using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Text.Json.Serialization;

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
            Console.WriteLine("почтаковий прямокутник  " + myRectangle.GetRectangleDescription());
            
            
            myRectangle.MoveToNewPosition(1, 1);
            Console.WriteLine("переміщенний прямокутник " + myRectangle.GetRectangleDescription());
            
            myRectangle.MoveWithOffset(2, 1);
            Console.WriteLine("переміщенний прямокутник на 2 по х та 1 по у" + myRectangle.GetRectangleDescription());
            
            myRectangle.Resize(5, 6);
            Console.WriteLine("прямокутник з новим розміром " + myRectangle.GetRectangleDescription());
            
           
            Rectangle rectangle1 = new Rectangle(1,5,2,1);
            Rectangle rectangle2 = new Rectangle(4,2,2,1);
            Rectangle container = Rectangle.ContainerRectangle(rectangle1, rectangle2);
            Console.WriteLine(("описаный прямокутник " + container.GetRectangleDescription()));

            Rectangle rectangle3 = new Rectangle(1, 5, 3, 2);            
            Rectangle rectangle4 = new Rectangle(2, 4, 3, 2);
            Rectangle intersection = Rectangle.IntersectionOfRectangle(rectangle3, rectangle4);
            if(intersection == null)
            {
                Console.WriteLine("прямокутники не перетинаються");
            }
            else
            {
                Console.WriteLine(("перетин прямокутників " + intersection.GetRectangleDescription()));
            }
            
            string serializedObject = JsonSerializer.Serialize(myRectangle);
            File.WriteAllText(@"C:\Users\Public\Documents\myRectangle.Json", serializedObject);

           
            string jsonContent = File.ReadAllText(@"C:\Users\Public\Documents\myRectangle.Json");
            
            Rectangle MyDeserializedRectangle = JsonSerializer.Deserialize<Rectangle>(jsonContent);
            Console.WriteLine(("десеріалізованый обьект прямокуктника " + MyDeserializedRectangle.GetRectangleDescription()));






        }
    }
    public class Rectangle
    {
        
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

       
        [JsonIgnore]
        public int positionRightTopX { get { return x + width; } }
        [JsonIgnore]
        public int positionRightTopY { get { return y; } }
        [JsonIgnore]
        public int positionLeftBottomX { get { return x ; } }
        [JsonIgnore]
        public int positionLeftBottomY { get { return y - height; } }
        [JsonIgnore]
        public int positionRightBottomX { get { return x + width; } }
        [JsonIgnore]
        public int positionRightBottomY { get { return y - height; } }

        
        public Rectangle()
        {
        }

       
        public Rectangle(int x, int y, int width, int height)
        {

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        
        public void MoveToNewPosition(int newX, int newY)
        {
            this.x = newX;
            this.y = newY;

        }

        
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
            
            // левый верхний угол 
            int topLeftX = Math.Min(rectangle1.x, rectangle2.x);
            int topLeftY = Math.Max(rectangle1.y, rectangle2.y);
            //правый нижний угол
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
            
            int x1 = Math.Max(rectangle1.x, rectangle2.x);

            
            int y1 = Math.Min(rectangle1.y, rectangle2.y);

            
            int x2 = Math.Min(rectangle1.positionRightTopX , rectangle2.positionRightTopX);
            
            
            int y2 = Math.Max(rectangle1.positionRightBottomY, rectangle2.positionRightBottomY);
            
            if (x1 < x2 && y1 > y2)
            {
                return new Rectangle(x1, y1, x2 - x1, y1 - y2);
            }
            else
            {
                
                return null;
            }
        }
        
        public string GetRectangleDescription()
        {
            return string.Format("Лівий верхній кут  ({0},{1}),ширина {2},висота {3}", this.x, this.y, this.width, this.height);
        }

       
       
        
    }
}


