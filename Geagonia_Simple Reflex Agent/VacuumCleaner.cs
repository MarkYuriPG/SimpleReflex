using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geagonia_Simple_Reflex_Agent
{
    enum Action
    {
        Left, Right, Up, Down, Clean, NoOp
    }

    internal class VacuumCleaner
    {
        public Room currentRoom { get; set; }
        public Timer NoOpTimer;
        public Timer MoveTimer;
        public Timer CleanTimer;
        public Point Position;
        public Action CurrentAction;
        public List<Room> rooms;
        public int MoveSpeed = 10;
        int time = 0;


        public VacuumCleaner(List<Room> rooms,Room currentRoom) 
        {
            this.currentRoom = currentRoom;
            Position = currentRoom.roomPoint;
            this.rooms = rooms;

            NoOpTimer = new Timer();
            NoOpTimer.Interval = 100;
            NoOpTimer.Tick += AnimateSleep;
            NoOpTimer.Enabled = false;

            MoveTimer = new Timer();
            MoveTimer.Interval = 100;
            MoveTimer.Tick += AnimateMove;
            MoveTimer.Enabled = false;

            CleanTimer = new Timer();
            CleanTimer.Interval = 100;
            CleanTimer.Tick += AnimateClean;
            CleanTimer.Enabled = false;
        }

        Random random = new Random();

        public Action Act(List<Room> rooms)
        {
            // Phase 1 choose what act to start first
            // Phase 2 then run/animate the chosen act

            // Phase 1
            bool hasTimerEnabled =  MoveTimer.Enabled || 
                                    NoOpTimer.Enabled || 
                                    CleanTimer.Enabled;
            if (!hasTimerEnabled)
            {
				if (currentRoom.roomState == State.Dirty)
				{
					StartClean();
				}
				else if (currentRoom.roomState == State.Clean)
				{
					if(random.Next(0, 2) == 0)
                    {
                        
						StartMove();
                    }
					else
						StartNoOp();
				}
            }

            return Action.NoOp;
        }

        public Action Move(List<Room> rooms, int randomNumber)
        {
            if(randomNumber >= 0 && randomNumber <= 5)
            {
                return GoLeft(rooms);
            }
            else if(randomNumber >= 6 && randomNumber <= 10)
            {
                return GoUp(rooms);
            }
            else if (randomNumber >= 11 && randomNumber <= 15)
            {
                return GoRight(rooms);
            }
            else if (randomNumber >= 16 && randomNumber <= 20)
            {
                return GoDown(rooms);
            }
            else if (randomNumber == 21)
            {
                time = 0;
                NoOpTimer.Start();
                Console.WriteLine("START SLEEP");
                return Action.NoOp;
            }
            return Action.NoOp;
        }

        private Action GoUp(List<Room> rooms)
        {
            if (currentRoom == rooms[(int)Location.C])
                currentRoom = rooms[(int)Location.A];
            else if (currentRoom == rooms[(int)Location.D])
                currentRoom = rooms[(int)Location.B];
            return Action.Up;
        }

        private Action GoLeft(List<Room> rooms)
        {
            if (currentRoom == rooms[(int)Location.B])
                currentRoom = rooms[(int)Location.A];
            else if (currentRoom == rooms[(int)Location.D])
                currentRoom = rooms[(int)Location.C];
            return Action.Left;
        }

        private Action GoDown(List<Room> rooms)
        {
            if (currentRoom == rooms[(int)Location.A])
                currentRoom = rooms[(int)Location.C];
            else if (currentRoom == rooms[(int)Location.B])
                currentRoom = rooms[(int)Location.D];
            return Action.Down;
        }

        private Action GoRight(List<Room> rooms)
        {
            if (currentRoom == rooms[(int)Location.A])
                currentRoom = rooms[(int)Location.B];
            else if (currentRoom == rooms[(int)Location.C])
                currentRoom = rooms[(int)Location.D];
            return Action.Right;
        }

        private Action Clean()
        {
            currentRoom.roomState = State.Clean;
            currentRoom.DirtTimer.Start();
            currentRoom.time = 0;

            return Action.Clean;
        }

        // Clean ---------------------------------
        private void StartClean()
        {
            time = 0;
            CleanTimer.Start();
            CleanTimer.Enabled = true;
        }
        private void AnimateClean(object sender, EventArgs e)
        {
            time++;
            if (time == 4)
            {
                CleanTimer.Stop();
                CleanTimer.Enabled = false;
                Clean();
                Console.WriteLine("Stop cleaning");
            }
            // TODO: ANIMATE CLEAN HERE
            Console.WriteLine("CLEANING");
        }

        // Sleep ---------------------------------
        // starter for NoOpTimer 
        private void StartNoOp()
        {
            time = 0;
            NoOpTimer.Start();
            NoOpTimer.Enabled = true;
        }

        // callback for NoOpTimer
        private void AnimateSleep(object o, EventArgs a)
        {
            time++;
            if (time == 8)
            {
                NoOpTimer.Stop();
                NoOpTimer.Enabled = false;
                Console.WriteLine("Stop Sleeping");
            }
            // TODO: ANIMATE SLEEP HERE
            Console.WriteLine("SLEEPING");
        }

        // Move ---------------------------------
        // starter for MoveTimer
        private void StartMove()
        {
            // choose direction

            time = 0;
            MoveTimer.Start();
            MoveTimer.Enabled = true;

            if (currentRoom.roomName == Location.A)
            {
                if (random.Next(0,2) == 0)
                {
                    CurrentAction = Action.Right;
                    currentRoom = rooms[(int)Location.B];
                }
                else
                {
                    CurrentAction = Action.Down;
                    currentRoom = rooms[(int)Location.C];
                }
            }
            else if (currentRoom.roomName == Location.B)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Left;
                    currentRoom = rooms[(int)Location.A];
                }
                else
                {
                    CurrentAction = Action.Down;
                    currentRoom = rooms[(int)Location.D];
                }
            }
            else if (currentRoom.roomName == Location.C)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Up;
                    currentRoom = rooms[(int)Location.A];
                }
                else
                {
                    CurrentAction = Action.Right;
                    currentRoom = rooms[(int)Location.D];
                }
            }
            else if (currentRoom.roomName == Location.D)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Up;
                    currentRoom = rooms[(int)Location.B];
                }
                else
                {
                    CurrentAction = Action.Left;
                    currentRoom = rooms[(int)Location.C];
                }
            }
        }

        // callback for MoveTimer
        private void AnimateMove(object sender, EventArgs e)
        {
            time++;
            if (time == 10)
            {
                MoveTimer.Stop();
                MoveTimer.Enabled = false;
                Console.WriteLine("Stop Move");
            }

            if (CurrentAction == Action.Left)
            {
                AnimateLeft();
            }
            else if (CurrentAction == Action.Right)
            {
                AnimateRight();
            }
            else if (CurrentAction == Action.Down)
            {
                AnimateDown();
            }
            else if (CurrentAction == Action.Up)
            {
                AnimateUp();
            }
        }

        private void AnimateDown()
        {
            int distance = rooms[2].roomPoint.Y - rooms[0].roomPoint.Y;
            Position = new Point(Position.X, Position.Y + (int)(distance * 0.1));
            Console.WriteLine("Down");
        }

        private void AnimateUp()
        {
            int distance = rooms[2].roomPoint.Y - rooms[0].roomPoint.Y;
            Position = new Point(Position.X, Position.Y - (int) (distance * 0.1));
            Console.WriteLine("Up");
        }

        private void AnimateRight()
        {
            int distance = rooms[1].roomPoint.X - rooms[0].roomPoint.X;
            Position = new Point(Position.X + (int)(distance * 0.1), Position.Y);
            Console.WriteLine("Right");
        }

        private void AnimateLeft()
        {
            int distance = rooms[1].roomPoint.X - rooms[0].roomPoint.X;
            Position = new Point(Position.X - (int)(distance * 0.1), Position.Y);
            Console.WriteLine("Left");
        }

    }
}
