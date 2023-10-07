using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geagonia_Simple_Reflex_Agent
{
    public partial class Form1 : Form
    {
        List<Room> rooms = new List<Room>();
        List<Point> roomPoints = new List<Point>();

        VacuumCleaner vacuumCleaner;

        int centerX;
        int centerY;

        Random random = new Random();
        int randomNumber;

        public Form1()
        {
            InitializeComponent();
            SetUp();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Black);
            Brush labelBrush = new SolidBrush(Color.Black);
            Brush greenBrush = new SolidBrush(Color.Green);
            Brush redBrush = new SolidBrush(Color.Red);

            centerX = this.ClientSize.Width / 2;
            centerY = this.ClientSize.Height / 2;

            g.DrawLine(myPen, centerX, 0, centerX, this.ClientSize.Height);
            g.DrawLine(myPen, 0, centerY, this.ClientSize.Width, centerY);

            Font font = new Font("Arial", 12);


            roomPoints[0] = new Point(centerX / 2, centerY / 2);
            roomPoints[1] = new Point(this.ClientSize.Width - this.ClientSize.Width / 4,
                    centerY / 2);
            roomPoints[2] = new Point(centerX / 2,
                    this.ClientSize.Height - this.ClientSize.Height / 4);
            roomPoints[3] = new Point(this.ClientSize.Width - this.ClientSize.Width / 4,
                    this.ClientSize.Height - this.ClientSize.Height / 4);
  
            if(vacuumCleaner.NoOpTimer.Enabled)
                g.FillEllipse(redBrush, vacuumCleaner.Position.X, vacuumCleaner.Position.Y, 100, 100);
            else
                g.FillEllipse(greenBrush, vacuumCleaner.Position.X, vacuumCleaner.Position.Y, 100, 100);

            for (int i = 0; i < rooms.Count; i++)
            {
                rooms[i].roomPoint = roomPoints[i];
                g.DrawString(rooms[i].roomName + "\n" + rooms[i].roomState,
                    font, labelBrush, rooms[i].roomPoint);
            }
        }

        // Original callback 
        private void Update(object sender, EventArgs e)
        {
            vacuumCleaner.Act(rooms);
            this.Refresh();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void SetUp()
        {
            centerX = this.ClientSize.Width / 2;
            centerY = this.ClientSize.Height / 2;

            roomPoints.Add(new Point(centerX / 2, centerY / 2));

            roomPoints.Add
            (
                new Point
                (
                    this.ClientSize.Width - this.ClientSize.Width / 4,
                    centerY / 2
                )
            );

            roomPoints.Add
            (
                new Point
                (
                    centerX / 2,
                    this.ClientSize.Height - this.ClientSize.Height / 4
                )
            );

            roomPoints.Add
            (
                new Point
                (
                    this.ClientSize.Width - this.ClientSize.Width / 4,
                    this.ClientSize.Height - this.ClientSize.Height / 4
                )
            );

            for (int i = 0; i<4; i++)
            {
                rooms.Add(new Room((Location)i));
                rooms[i].roomPoint = roomPoints[i];
            }

            foreach(Room room in rooms)
            {
                room.roomState = (State)random.Next(0, 2);
                if(room.roomState == State.Clean)
                    room.DirtTimer.Start();
            }           
        
            randomNumber = new Random().Next(0, 4);
            vacuumCleaner = new VacuumCleaner(rooms, rooms[randomNumber]);
        }
    }
    
}
