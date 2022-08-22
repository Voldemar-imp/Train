using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isContinue = true;
            Station station = new Station();

            while (isContinue)
            {
                Console.Clear();
                station.ShowInfo();
                Console.WriteLine("1) Создать новый поезд \n2) Показать архив \n3) выход");
                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.Clear();

                switch (key.KeyChar)
                {
                    case '1':
                        station.CreateNewTrain();
                        break;
                    case '2':
                        station.ShowArhive();
                        break;
                    case '3':
                        isContinue = false;
                        break;
                    default:
                        Console.WriteLine("Неверно выбрана команда. Для продолжения нажмите любую клавишу...");
                        break;
                }
            }

        }

    }

    class Arhive
    {
        private List<Train> _trains = new List<Train>();
        private List<Ticket> _ticketsList = new List<Ticket>();

        public Arhive()
        {
            _trains = new List<Train>();
            _ticketsList = new List<Ticket>();
        }

        public void AddTrain(Train train)
        {
            _trains.Add(train);
        }

        public void AddTicketsList(List<Ticket> ticketsList)
        {
            _ticketsList.AddRange(ticketsList);
        }

        public void ShowTrains()
        {
            foreach (Train train in _trains)
            {
               train.ShowArhive();
            }

            Console.ReadKey(true);
        }

        public void ShowticketsList()
        {
            foreach (Ticket ticket in _ticketsList)
            {
                ticket.ShowInfo();
            }

            Console.ReadKey(true);
        }
    }

    class Station
    {
        private int _passengers;
        private Direction _direction;
        private Arhive _arhive;
        private Train _train;
        private List<Ticket> _ticketsList = new List<Ticket>();

        public Station()
        {
            _passengers = 0;
            _train = null;
            _arhive = new Arhive();
        }

        public void CreateNewTrain()
        {
            ShowInfo();
            CreateDirection();
            AddTrain();
            GetPassengers();
            AddTicket();
            ShowTickets();
            SendTrain();
            SendToArhive();
        }

        public void ShowArhive()
        {
            ShowInfo();
            Console.WriteLine("1) Показать отвравленные поезда \n2) показать проданные билеты");
            ConsoleKeyInfo key = Console.ReadKey(true);
            ShowInfo();

            switch (key.KeyChar)
            {
                case '1':
                    _arhive.ShowTrains();
                    break;
                case '2':
                    _arhive.ShowticketsList();
                    break;

                default:
                    Console.WriteLine("Неверно выбрана команда. Для продолжения нажмите любую клавишу...");
                    break;
            }
        }
        
        public void ShowInfo()
        {
            Console.Clear();
            if (_train == null)
            {
                Console.Write($"Направление: < Не создано || \n" +
                    $"Поезд не сформирован\n\n");
            }
            else
            {
                _train.ShowStatus();
            }
        }

        private void CreateDirection()
        {
            ShowInfo();
            Console.WriteLine("Введите название пункта отправления:");
            string station1 = Console.ReadLine();
            Console.WriteLine("Введите название пункта назначения:");
            string station2 = Console.ReadLine();
            _direction = new Direction(station1, station2);
        }

        private void AddTrain()
        {
            _train = new Train(_direction);
        }

        private void GetPassengers()
        {
            Random random = new Random();
            _passengers = random.Next(100, 501);
            ShowInfo();
            Console.WriteLine("Желающих купить билеты: " + _passengers + " человек");
            Console.ReadKey(true);
        }

        private void AddTicket()
        {
            int seatsTaken = 0;
            int wagonNumber = 1;
            _train.AddWagon();

            while (seatsTaken != _passengers)
            {
                Wagon wagon = _train.GetWagon(wagonNumber);

                if (wagon.AddPassengers())
                {
                    _ticketsList.Add(new Ticket(_direction, wagonNumber, wagon.SeatsTaken));
                    seatsTaken++;
                }
                else
                {
                    _train.AddWagon();
                    wagonNumber++;
                }
            }
        }

        private void SendTrain()
        {
            ShowInfo();
            Console.WriteLine("\nЧтобы отправить поезд нажмите любую клавишу");
            Console.ReadKey(true);
        }

        private void SendToArhive()
        {
            _arhive.AddTrain(_train);
            _train = null;
            _arhive.AddTicketsList(_ticketsList);
            _ticketsList = new List<Ticket>();
        }

        private void ShowTickets()
        {
            ShowInfo();
            Console.WriteLine("Проданные билеты: \n");

            foreach (Ticket ticket in _ticketsList)
            {
                ticket.ShowInfo();
            }

            Console.ReadKey(true);
        }
  
    }


    class Direction
    {       
        public string Station1 { get; private set; }
        public string Station2 { get; private set; }

        public Direction(string station1, string station2)
        {
            Station1 = station1;
            Station2 = station2;           
        }
    }

    class Train
    {
        private List<Wagon> _wagons = new List<Wagon>();
        private Direction _direction;
        private Random _random = new Random(); 

        public Train(Direction direction)
        {
            _direction = direction; 
        }

        public void AddWagon()
        {
            _wagons.Add(new Wagon(_random.Next(40, 61)));
        }

        public Wagon GetWagon(int number)
        {
            return _wagons[number - 1];
        }

        public void ShowStatus()
        {
            Console.Clear();
            if(_wagons.Count == 0)
            {
                Console.Write($"Направление: < {_direction.Station1} - {_direction.Station2} || \n" +
                    $"Поезд не сформирован\n\n");
            }
            else
            {
                ShowInfo();
                Console.WriteLine("\nПоезд готов к отправлению \n\n");
            }

            Console.WriteLine();
        }

        public void ShowArhive()
        {
            ShowInfo();
            Console.WriteLine(" - Поезд отправлен");
        }

        private void ShowInfo()
        {
            Console.Write($"Направление: < {_direction.Station1} - {_direction.Station2} || ");
            for (int i = 0; i < _wagons.Count; i++)
            {
                Console.Write($" | {i + 1}");
                _wagons[i].ShowInfo();
            }
        }
    }

    class Wagon
    {        
        public int Seats { get; }
        public int SeatsTaken { get; private set; }
        
        public Wagon(int seats)
        {
            Seats = seats; 
            SeatsTaken = 0;
        }

        public bool AddPassengers()
        {
            if (Seats > SeatsTaken)
            {
                SeatsTaken++;
                return true;
            }

            return false;
        }

        public void ShowInfo()
        {
            Console.Write($" : {SeatsTaken}/{Seats}| ");
        }
    }

    class Ticket
    {
        private static int _numbers = 10000;
        private int _wagonNumber;
        private int _seatNumber;
        private int _number;
        private Direction _direction;

        public Ticket(Direction direction, int wagonNumber, int seatNumber)
        {
            _number = ++_numbers;
            _wagonNumber = wagonNumber;
            _seatNumber = seatNumber;
            _direction = direction;
        }

        public void ShowInfo()
        {
            Console.WriteLine($" №{_number}. Откуда: {_direction.Station1}. Куда: {_direction.Station2}. Вагон: {_wagonNumber} / место: {_seatNumber}");
        }
    }
}
