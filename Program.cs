using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace CarDealership
{
    internal class Program
    {
        static List<Car> cars = new List<Car>();
        static string antetka = "";
        static string filePath = "..\\..\\cars.txt";

        static void Main(string[] args)
        {
            ReadCarsFromFile(filePath);

            while (true)
            {

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1) Покажи всички налични коли в склада");
                Console.WriteLine("2) Покажи всички продадени коли");
                Console.WriteLine("3) Продай кола от склада");
                Console.WriteLine("4) Добави нова кола в склада");
                Console.ForegroundColor = ConsoleColor.White;

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1: ShowCarsInSquad(); break;
                    case 2: ShowCarsSold(); break;
                    case 3: SellCar(); break;
                    case 4: AddNewCar(); break;
                    default: System.Environment.Exit(0); break;
                }

                Console.WriteLine(); //new line
            }
        }

        static void AddNewCar()
        {

        }

        static void SellCar()
        {
            Console.WriteLine("Въведете номера на колата, която искате да продадете");
            
            Car[] listCars = new Car[cars.Count];
            int i = 0;
            foreach (Car car in cars)
            {
                if (car.Availability)
                {
                    i++;
                    Console.WriteLine($"{i}) {car.Brand} {car.Model} {car.Color}");
                    listCars[i] = car;
                }
            }

            int choice = int.Parse(Console.ReadLine());
            if (choice > i || choice < 1) SellCar();

            Car sCar = listCars[choice];
            if (sCar.Sell())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Колата {sCar.Brand} {sCar.Model} е успешно продадена!");
                Console.ForegroundColor = ConsoleColor.White;
                WriteCarsToFile(filePath);
                ShowCarsSold();
            }
        }

        static void ShowCarsSold()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nСписък на продадените коли и собствениците им");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Car car in cars)
            {
                if (!car.Availability)
                {
                    Console.WriteLine($"{car.Brand} {car.Model} {car.Color} продадена на => {car.NewОwner}");
                }
            }
        }

        static void ShowCarsInSquad()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Списък на всички коли в склада");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Car car in cars)
            {
                if (car.Availability)
                {
                    Console.WriteLine($"{car.Brand} {car.Model} {car.Color} ");
                }
            }
        }

        static void ReadCarsFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8")))
            {
                string line;
                int row = 0;

                while ((line = sr.ReadLine()) != null)
                {

                    string[] carData = line.Split(',');

                    if (carData.Length == 10 && row != 0)
                    {
                        int ID = int.Parse(carData[0]);
                        string Brand = carData[1];
                        string Model = carData[2];
                        int YearOfManufacture = int.Parse(carData[3]);
                        int Price = int.Parse(carData[4]);
                        string Color = carData[5];
                        bool IsNew = bool.Parse(carData[6]);
                        int Mileage = int.Parse(carData[7]);
                        bool Availability = bool.Parse(carData[8]);
                        string NewОwner = carData[9];

                        Car newCar = new Car(ID, Brand, Model, YearOfManufacture,
                                    Price, Color, IsNew, Mileage, Availability, NewОwner);
                        cars.Add(newCar);
                    }
                    else
                    {
                        if (row == 0)
                        {
                            antetka = line;
                        }

                        if (carData.Length != 10)
                        {
                            Console.WriteLine($"Некоректни данни в ред {row}");
                        }
                    }
                    row++;
                }
            }
        }

        static void WriteCarsToFile(string filePath)
        {

            using (StreamWriter sw = new StreamWriter(filePath, false,
                    Encoding.GetEncoding("utf-8")))
            {
                sw.WriteLine(antetka);

                foreach (Car car in cars)
                {
                    string line = $"{car.ID},{car.Brand},{car.Model},{car.YearOfManufacture},";
                    line += $"{car.Price},{car.Color},{car.IsNew},{car.Mileage}," +
                        $"{car.Availability},{car.NewОwner}";
                    sw.WriteLine(line);
                }
            }
        }

        class Car
        {
            public int ID;
            public string Brand;
            public string Model;
            public int YearOfManufacture;
            public int Price;
            public string Color;
            public bool IsNew;
            public int Mileage;
            public bool Availability;
            public string NewОwner;

            public Car(int id, string brand, string model, int yearOfManufacture,
                int price, string color, bool isNew, int mileage, bool availability, string newOwner)
            {
                ID = id; Brand = brand; Model = model; 
                YearOfManufacture = yearOfManufacture;
                Price = price; Color = color; IsNew = isNew; 
                Mileage = mileage; Availability = availability;
                NewОwner = newOwner;
            }

            public bool Sell()
            {
                if (Availability)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("Въведете име на купувача:");
                    Console.ForegroundColor = ConsoleColor.White;

                    string name = Console.ReadLine();
                    if (checkName(name))
                    {
                        Availability = false;
                        NewОwner = name;
                        return true;  //Успешно продаване
                    }
                }

                return false; //Неуспешно продаване
            }

            private bool checkName(string name)
            {
                foreach (char c in name)
                {
                    if (c == ',') return false;
                }
                return true;    
            }

        }
    }
}
