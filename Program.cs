namespace _01_OOPGenericExam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Motorcycle:");
            MotorCycle m1 = new MotorCycle(model: "R15V5", yearMake: 2024, cc: 160, noOfGear: 6, vehicleType: VehicleType.Personal, startingMethod: "Self+Kick", maxPowerBHP: 250, maxTorqueNM: 22000, mileage: 45, cooling: "Air Cooled", frontBrake: "Disk", rearBrake: "ABS");
            m1.AddExteriorDesign("VIP Horn", "LED Display", "LED Fog Light");
            GenericDetailImplement<MotorCycle> genericMotor = new GenericDetailImplement<MotorCycle>();
            Console.WriteLine(genericMotor.GetDetail(m1));
            Console.WriteLine(m1.GetExteriorDesign());
            Console.WriteLine();

            Console.WriteLine("Car:");
            Car c1 = new Car(model: "Land Cruiser", yearMake: 2017, cc: 1500, noOfGear: 6, vehicleType: VehicleType.Racing, noOfSeat: 5, noOfDoor: 5);
            c1.AddInteriorDesign("GPS", "Auto Drive", "Music System");
            VehicleDetailsImplementement<Car> vehicleCar = new VehicleDetailsImplementement<Car>();
            Console.WriteLine(vehicleCar.GetDetail(c1));
            Console.WriteLine(c1.GetInteriorDesign());
            Console.WriteLine();

            Console.ReadKey();
        }
    }
    //Enum
    public enum VehicleType { Personal = 1, Racing }
    //Abstract
    public abstract class Vehicle
    {
        protected Vehicle(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType)
        {
            Model = model;
            YearMake = yearMake;
            CC = cc;
            NoOfGear = noOfGear;
            VehicleType = vehicleType;
        }
        public string Model { get; set; }
        public int YearMake { get; set; }
        public int CC { get; set; }
        public int NoOfGear { get; set; }
        public VehicleType VehicleType { get; set; }
        public abstract string Details();
    }
    //Interfaces for Design
    public interface IExteriorDesign
    {
        void AddExteriorDesign(params string[] designs);
        string GetExteriorDesign();
    }
    public interface IInteriorDesign
    {
        void AddInteriorDesign(params string[] designs);
        string GetInteriorDesign();
    }
    //TwoWheeler
    public class TwoWheeler : Vehicle, IExteriorDesign
    {
        private readonly List<string> exteriorDesigns = new List<string>();
        protected TwoWheeler(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, string startingMethod, int maxPowerBHP, int maxTorqueNM, int mileage, string cooling, string frontBrake, string rearBrake) : base(model, yearMake, cc, noOfGear, vehicleType)
        {
            StartingMethod = startingMethod;
            MaxPowerBHP = maxPowerBHP;
            MaxTorqueNM = maxTorqueNM;
            Mileage = mileage;
            Cooling = cooling;
            FrontBrake = frontBrake;
            RearBrake = rearBrake;
        }
        public string StartingMethod { get; set; }
        public int MaxPowerBHP { get; set; }
        public int MaxTorqueNM { get; set; }
        public int Mileage { get; set; }
        public string Cooling { get; set; }
        public string FrontBrake { get; set; }
        public string RearBrake { get; set; }
        public void AddExteriorDesign(params string[] designs)
        {
            this.exteriorDesigns.AddRange(designs);
        }
        public string GetExteriorDesign() => string.Join(", ", exteriorDesigns);
        public override string Details()
        {
            return $"{Model}, {CC}cc, Year: {YearMake}, Mileage: {Mileage} KMPL, Start: {StartingMethod}, Gear: {NoOfGear}, Cooling: {Cooling}, Brakes: {FrontBrake}/{RearBrake}, Type: {VehicleType}, Power: {MaxPowerBHP} BHP, Torque: {MaxTorqueNM} NM";
        }
    }
    // FourWheeler
    public class FourWheeler : Vehicle, IInteriorDesign
    {
        private readonly List<string> interiorDesigns = new List<string>();
        protected FourWheeler(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, int noOfSeat, int noOfDoor) : base(model, yearMake, cc, noOfGear, vehicleType)
        {
            NoOfSeat = noOfSeat;
            NoOfDoor = noOfDoor;
        }
        public int NoOfSeat { get; set; }
        public int NoOfDoor { get; set; }
        public void AddInteriorDesign(params string[] designs)
        {
            this.interiorDesigns.AddRange(designs);
        }
        public string GetInteriorDesign() => string.Join(", ", interiorDesigns);
        public override string Details()
        {
            return $"{Model}, {CC}cc, Year: {YearMake}, Gear: {NoOfGear}, Seats: {NoOfSeat}, Doors: {NoOfDoor}, Type: {VehicleType}";
        }
    }
    //Sealed Classes
    public sealed class MotorCycle : TwoWheeler
    {
        public MotorCycle(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, string startingMethod, int maxPowerBHP, int maxTorqueNM, int mileage, string cooling, string frontBrake, string rearBrake) : base(model, yearMake, cc, noOfGear, vehicleType, startingMethod, maxPowerBHP, maxTorqueNM, mileage, cooling, frontBrake, rearBrake)
        {
        }
    }
    public sealed class Car : FourWheeler
    {
        public Car(string model, int yearMake, int cc, int noOfGear, VehicleType vehicleType, int noOfSeat, int noOfDoor) : base(model, yearMake, cc, noOfGear, vehicleType, noOfSeat, noOfDoor)
        {
        }
    }
    //Generic Interfaces
    public interface IGenericDetail<T>
    {
        string GetDetail<T1>(T1 obj);
    }
    public class GenericDetailImplement<T> : IGenericDetail<T>
    {
        public string GetDetail<T1>(T1 obj)
        {
            return obj is Vehicle vehicle ? vehicle.Details() : "Not a Vehicle";
        }
    }
    public interface IVehicleDetail<T>
    {
        string GetDetail<T1>(T1 obj) where T1 : Vehicle;
    }
    public class VehicleDetailsImplementement<T> : IVehicleDetail<T>
    {
        public string GetDetail<T1>(T1 obj) where T1 : Vehicle
        {
            return obj.Details();
        }
    }
}








//namespace _2_LINQ
//{
//    public class Product
//    {
//        public int ProductID { get; set; }
//        public string Name { get; set; }
//        public string ProductNumber { get; set; }
//        public string Color { get; set; }
//        public double StandardCost { get; set; }
//        public double ListPrice { get; set; }
//        public int Size { get; set; }
//        public double Weight { get; set; }
//        public int ProductCategoryID { get; set; }
//        public int ProductModelID { get; set; }
//    }
//    public class ProductCategory
//    {
//        public int ProductCategoryID { get; set; }
//        public string Name { get; set; }
//    }
//    public class ProductModel
//    {
//        public int ProductModelID { get; set; }
//        public string Name { get; set; }
//    }
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            var products = new Product[]
//           {
//               new Product(){ ProductID = 1, Name = "LL Road Frame - Red, 44", ProductNumber = "FR-R38R-44", Color = "Red", StandardCost = 187.1571, ListPrice = 337.22, Size = 44, Weight = 1052.33, ProductCategoryID = 18, ProductModelID = 9           } ,
//               new Product(){ ProductID = 2, Name = "LL Road Frame - Red, 48", ProductNumber = "FR-R38R-48", Color = "Red", StandardCost = 187.1571, ListPrice = 337.22, Size = 48, Weight = 1070.47, ProductCategoryID = 18, ProductModelID = 9           } ,
//               new Product(){ ProductID = 3, Name = "LL Road Frame - Red, 52", ProductNumber = "FR-R38R-52", Color = "Red", StandardCost = 187.1571, ListPrice = 337.22, Size = 52, Weight = 1088.62, ProductCategoryID = 18, ProductModelID = 9           } ,
//               new Product(){ ProductID = 4, Name = "LL Road Frame - Red, 58", ProductNumber = "FR-R38R-58", Color = "Red", StandardCost = 187.1571, ListPrice = 337.22, Size = 58, Weight = 1115.83, ProductCategoryID = 18, ProductModelID = 9           } ,
//               new Product(){ ProductID = 5, Name = "LL Road Frame - Red, 60", ProductNumber = "FR-R38R-60", Color = "Red", StandardCost = 187.1571, ListPrice = 337.22, Size = 60, Weight = 1124.9, ProductCategoryID = 18, ProductModelID = 9            } ,
//               new Product(){ ProductID = 6, Name = "LL Road Frame - Red, 62", ProductNumber = "FR-R38R-62", Color = "Red", StandardCost = 187.1571, ListPrice = 337.22, Size = 62, Weight = 1133.98, ProductCategoryID = 18, ProductModelID = 9           } ,
//               new Product(){ ProductID = 7, Name = "ML Road Frame - Red, 44", ProductNumber = "FR-R72R-44", Color = "Red", StandardCost = 352.1394, ListPrice = 594.83, Size = 44, Weight = 1006.97, ProductCategoryID = 18, ProductModelID = 16          } ,
//               new Product(){ ProductID = 8, Name = "ML Road Frame - Red, 48", ProductNumber = "FR-R72R-48", Color = "Red", StandardCost = 352.1394, ListPrice = 594.83, Size = 48, Weight = 1025.11, ProductCategoryID = 18, ProductModelID = 16          } ,
//               new Product(){ ProductID = 9, Name = "ML Road Frame - Red, 52", ProductNumber = "FR-R72R-52", Color = "Red", StandardCost = 352.1394, ListPrice = 594.83, Size = 52, Weight = 1043.26, ProductCategoryID = 18, ProductModelID = 16          } ,
//               new Product(){ ProductID = 10, Name = "ML Road Frame - Red, 58", ProductNumber = "FR-R72R-58", Color = "Red", StandardCost = 352.1394, ListPrice = 594.83, Size = 58, Weight = 1070.47, ProductCategoryID = 18, ProductModelID = 16         } ,
//               new Product(){ ProductID = 11, Name = "ML Road Frame - Red, 60", ProductNumber = "FR-R72R-60", Color = "Red", StandardCost = 352.1394, ListPrice = 594.83, Size = 60, Weight = 1079.54, ProductCategoryID = 18, ProductModelID = 16         } ,
//               new Product(){ ProductID = 12, Name = "HL Mountain Frame - Silver, 44", ProductNumber = "FR-M94S-44", Color = "Silver", StandardCost = 706.811, ListPrice = 1364.5, Size = 44, Weight = 1251.91, ProductCategoryID = 16, ProductModelID = 5 } ,
//               new Product(){ ProductID = 13, Name = "HL Mountain Frame - Silver, 48", ProductNumber = "FR-M94S-52", Color = "Silver", StandardCost = 706.811, ListPrice = 1364.5, Size = 48, Weight = 1270.05, ProductCategoryID = 16, ProductModelID = 5 } ,
//               new Product(){ ProductID = 14, Name = "HL Mountain Frame - Black, 44", ProductNumber = "FR-M94B-44", Color = "Black", StandardCost = 699.0928, ListPrice = 1349.6, Size = 44, Weight = 1251.91, ProductCategoryID = 16, ProductModelID = 5  } ,
//               new Product(){ ProductID = 15, Name = "HL Mountain Frame - Black, 48", ProductNumber = "FR-M94B-48", Color = "Black", StandardCost = 699.0928, ListPrice = 1349.6, Size = 48, Weight = 1270.05, ProductCategoryID = 16, ProductModelID = 5  } ,
//               new Product(){ ProductID = 16, Name = "Road-150 Red, 62", ProductNumber = "BK-R93R-62", Color = "Red", StandardCost = 2171.2942, ListPrice = 3578.27, Size = 62, Weight = 6803.85, ProductCategoryID = 6, ProductModelID = 25               } ,
//               new Product(){ ProductID = 17, Name = "Road-150 Red, 44", ProductNumber = "BK-R93R-44", Color = "Red", StandardCost = 2171.2942, ListPrice = 3578.27, Size = 44, Weight = 6245.93, ProductCategoryID = 6, ProductModelID = 25               } ,
//               new Product(){ ProductID = 18, Name = "Road-150 Red, 48", ProductNumber = "BK-R93R-48", Color = "Red", StandardCost = 2171.2942, ListPrice = 3578.27, Size = 48, Weight = 6409.23, ProductCategoryID = 6, ProductModelID = 25               } ,
//               new Product(){ ProductID = 19, Name = "Road-150 Red, 52", ProductNumber = "BK-R93R-52", Color = "Red", StandardCost = 2171.2942, ListPrice = 3578.27, Size = 52, Weight = 6540.77, ProductCategoryID = 6, ProductModelID = 25               } ,
//               new Product(){ ProductID = 20, Name = "Road-150 Red, 56", ProductNumber = "BK-R93R-56", Color = "Red", StandardCost = 2171.2942, ListPrice = 3578.27, Size = 56, Weight = 6658.7, ProductCategoryID = 6, ProductModelID = 25                } ,
//               new Product(){ ProductID = 21, Name = "Road-450 Red, 58", ProductNumber = "BK-R68R-58", Color = "Red", StandardCost = 884.7083, ListPrice = 1457.99, Size = 58, Weight = 8069.37, ProductCategoryID = 6, ProductModelID = 28                } ,
//               new Product(){ ProductID = 22, Name = "Road-450 Red, 60", ProductNumber = "BK-R68R-60", Color = "Red", StandardCost = 884.7083, ListPrice = 1457.99, Size = 60, Weight = 8119.26, ProductCategoryID = 6, ProductModelID = 28                } ,
//               new Product(){ ProductID = 23, Name = "Road-450 Red, 44", ProductNumber = "BK-R68R-44", Color = "Red", StandardCost = 884.7083, ListPrice = 1457.99, Size = 44, Weight = 7606.7, ProductCategoryID = 6, ProductModelID = 28                 } ,
//               new Product(){ ProductID = 24, Name = "Road-450 Red, 48", ProductNumber = "BK-R68R-48", Color = "Red", StandardCost = 884.7083, ListPrice = 1457.99, Size = 48, Weight = 7770, ProductCategoryID = 6, ProductModelID = 28                   } ,
//               new Product(){ ProductID = 25, Name = "Road-450 Red, 52", ProductNumber = "BK-R68R-52", Color = "Red", StandardCost = 884.7083, ListPrice = 1457.99, Size = 52, Weight = 7901.54, ProductCategoryID = 6, ProductModelID = 28                } ,
//               new Product(){ ProductID = 26, Name = "Road-650 Red, 58", ProductNumber = "BK-R50R-58", Color = "Red", StandardCost = 486.7066, ListPrice = 782.99, Size = 58, Weight = 8976.55, ProductCategoryID = 6, ProductModelID = 30                 } ,
//               new Product(){ ProductID = 27, Name = "Road-650 Red, 60", ProductNumber = "BK-R50R-60", Color = "Red", StandardCost = 486.7066, ListPrice = 782.99, Size = 60, Weight = 9026.44, ProductCategoryID = 6, ProductModelID = 30                 } ,
//               new Product(){ ProductID = 28, Name = "Road-650 Red, 62", ProductNumber = "BK-R50R-62", Color = "Red", StandardCost = 486.7066, ListPrice = 782.99, Size = 62, Weight = 9071.8, ProductCategoryID = 6, ProductModelID = 30                  } ,
//               new Product(){ ProductID = 29, Name = "Road-650 Red, 44", ProductNumber = "BK-R50R-44", Color = "Red", StandardCost = 486.7066, ListPrice = 782.99, Size = 44, Weight = 8513.88, ProductCategoryID = 6, ProductModelID = 30                 } ,
//               new Product(){ ProductID = 30, Name = "Road-650 Red, 48", ProductNumber = "BK-R50R-48", Color = "Red", StandardCost = 486.7066, ListPrice = 782.99, Size = 48, Weight = 8677.18, ProductCategoryID = 6, ProductModelID = 30                 } ,
//               new Product(){ ProductID = 31, Name = "Road-650 Red, 52", ProductNumber = "BK-R50R-52", Color = "Red", StandardCost = 486.7066, ListPrice = 782.99, Size = 52, Weight = 8808.72, ProductCategoryID = 6, ProductModelID = 30                 } ,
//               new Product(){ ProductID = 32, Name = "Road-650 Black, 58", ProductNumber = "BK-R50B-58", Color = "Black", StandardCost = 486.7066, ListPrice = 782.99, Size = 58, Weight = 8976.55, ProductCategoryID = 6, ProductModelID = 30             } ,
//               new Product(){ ProductID = 33, Name = "Road-650 Black, 60", ProductNumber = "BK-R50B-60", Color = "Black", StandardCost = 486.7066, ListPrice = 782.99, Size = 60, Weight = 9026.44, ProductCategoryID = 6, ProductModelID = 30             } ,
//               new Product(){ ProductID = 34, Name = "Road-650 Black, 62", ProductNumber = "BK-R50B-62", Color = "Black", StandardCost = 486.7066, ListPrice = 782.99, Size = 62, Weight = 9071.8, ProductCategoryID = 6, ProductModelID = 30              } ,
//               new Product(){ ProductID = 35, Name = "Road-650 Black, 44", ProductNumber = "BK-R50B-44", Color = "Black", StandardCost = 486.7066, ListPrice = 782.99, Size = 44, Weight = 8513.88, ProductCategoryID = 6, ProductModelID = 30             } ,
//               new Product(){ ProductID = 36, Name = "Road-650 Black, 48", ProductNumber = "BK-R50B-48", Color = "Black", StandardCost = 486.7066, ListPrice = 782.99, Size = 48, Weight = 8677.18, ProductCategoryID = 6, ProductModelID = 30             } ,
//               new Product(){ ProductID = 37, Name = "Road-650 Black, 52", ProductNumber = "BK-R50B-52", Color = "Black", StandardCost = 486.7066, ListPrice = 782.99, Size = 52, Weight = 8808.72, ProductCategoryID = 6, ProductModelID = 30             } ,
//               new Product(){ ProductID = 38, Name = "Mountain-100 Silver, 38", ProductNumber = "BK-M82S-38", Color = "Silver", StandardCost = 1912.1544, ListPrice = 3399.99, Size = 38, Weight = 9230.56, ProductCategoryID = 5, ProductModelID = 19     } ,
//               new Product(){ ProductID = 39, Name = "Mountain-100 Silver, 42", ProductNumber = "BK-M82S-42", Color = "Silver", StandardCost = 1912.1544, ListPrice = 3399.99, Size = 42, Weight = 9421.06, ProductCategoryID = 5, ProductModelID = 19     } ,
//               new Product(){ ProductID = 40, Name = "Mountain-100 Silver, 44", ProductNumber = "BK-M82S-44", Color = "Silver", StandardCost = 1912.1544, ListPrice = 3399.99, Size = 44, Weight = 9584.36, ProductCategoryID = 5, ProductModelID = 19     } ,
//               new Product(){ ProductID = 41, Name = "Mountain-100 Silver, 48", ProductNumber = "BK-M82S-48", Color = "Silver", StandardCost = 1912.1544, ListPrice = 3399.99, Size = 48, Weight = 9715.9, ProductCategoryID = 5, ProductModelID = 19      } ,
//               new Product(){ ProductID = 42, Name = "Mountain-100 Black, 38", ProductNumber = "BK-M82B-38", Color = "Black", StandardCost = 1898.0944, ListPrice = 3374.99, Size = 38, Weight = 9230.56, ProductCategoryID = 5, ProductModelID = 19       } ,
//               new Product(){ ProductID = 43, Name = "Mountain-100 Black, 42", ProductNumber = "BK-M82B-42", Color = "Black", StandardCost = 1898.0944, ListPrice = 3374.99, Size = 42, Weight = 9421.06, ProductCategoryID = 5, ProductModelID = 19       } ,
//               new Product(){ ProductID = 44, Name = "Mountain-100 Black, 44", ProductNumber = "BK-M82B-44", Color = "Black", StandardCost = 1898.0944, ListPrice = 3374.99, Size = 44, Weight = 9584.36, ProductCategoryID = 5, ProductModelID = 19       } ,
//               new Product(){ ProductID = 45, Name = "Mountain-100 Black, 48", ProductNumber = "BK-M82B-48", Color = "Black", StandardCost = 1898.0944, ListPrice = 3374.99, Size = 48, Weight = 9715.9, ProductCategoryID = 5, ProductModelID = 19        } ,
//               new Product(){ ProductID = 46, Name = "Mountain-300 Black, 38", ProductNumber = "BK-M47B-38", Color = "Black", StandardCost = 598.4354, ListPrice = 1079.99, Size = 38, Weight = 11498.51, ProductCategoryID = 5, ProductModelID = 21       } ,
//               new Product(){ ProductID = 47, Name = "Mountain-300 Black, 40", ProductNumber = "BK-M47B-40", Color = "Black", StandardCost = 598.4354, ListPrice = 1079.99, Size = 40, Weight = 11689.01, ProductCategoryID = 5, ProductModelID = 21       } ,
//               new Product(){ ProductID = 48, Name = "Mountain-300 Black, 44", ProductNumber = "BK-M47B-44", Color = "Black", StandardCost = 598.4354, ListPrice = 1079.99, Size = 44, Weight = 11852.31, ProductCategoryID = 5, ProductModelID = 21       } ,
//               new Product(){ ProductID = 49, Name = "Mountain-300 Black, 48", ProductNumber = "BK-M47B-48", Color = "Black", StandardCost = 598.4354, ListPrice = 1079.99, Size = 48, Weight = 11983.85, ProductCategoryID = 5, ProductModelID = 21       } ,
//               new Product(){ ProductID = 50, Name = "Road-250 Red, 44", ProductNumber = "BK-R89R-44", Color = "Red", StandardCost = 1518.7864, ListPrice = 2443.35, Size = 44, Weight = 6699.52, ProductCategoryID = 6, ProductModelID = 26               } ,
//               new Product(){ ProductID = 51, Name = "Road-250 Red, 48", ProductNumber = "BK-R89R-48", Color = "Red", StandardCost = 1518.7864, ListPrice = 2443.35, Size = 48, Weight = 6862.82, ProductCategoryID = 6, ProductModelID = 26               } ,
//               new Product(){ ProductID = 52, Name = "Road-250 Red, 52", ProductNumber = "BK-R89R-52", Color = "Red", StandardCost = 1518.7864, ListPrice = 2443.35, Size = 52, Weight = 6994.36, ProductCategoryID = 6, ProductModelID = 26               } ,
//               new Product(){ ProductID = 53, Name = "ML Mountain Frame - Black, 38", ProductNumber = "FR-M63B-38", Color = "Black", StandardCost = 185.8193, ListPrice = 348.76, Size = 38, Weight = 1238.3, ProductCategoryID = 16, ProductModelID = 15  } ,
//               new Product(){ ProductID = 54, Name = "ML Mountain Frame - Black, 40", ProductNumber = "FR-M63B-40", Color = "Black", StandardCost = 185.8193, ListPrice = 348.76, Size = 40, Weight = 1256.44, ProductCategoryID = 16, ProductModelID = 14 } ,
//               new Product(){ ProductID = 55, Name = "ML Mountain Frame - Black, 44", ProductNumber = "FR-M63B-44", Color = "Black", StandardCost = 185.8193, ListPrice = 348.76, Size = 44, Weight = 1274.59, ProductCategoryID = 16, ProductModelID = 14 } ,
//               new Product(){ ProductID = 56, Name = "ML Mountain Frame - Black, 48", ProductNumber = "FR-M63B-48", Color = "Black", StandardCost = 185.8193, ListPrice = 348.76, Size = 48, Weight = 1292.73, ProductCategoryID = 16, ProductModelID = 14 }
//              ,
//           };
//            var categories = new ProductCategory[]
//            {
//              new ProductCategory(){   ProductCategoryID = 1, Name = "Bikes"                        },
//              new ProductCategory(){  ProductCategoryID = 2, Name = "Components"                    },
//              new ProductCategory(){  ProductCategoryID = 3, Name = "Clothing"                      },
//              new ProductCategory(){  ProductCategoryID = 4, Name = "Accessories"                   },
//              new ProductCategory(){  ProductCategoryID = 5, Name = "Mountain Bikes"                },
//              new ProductCategory(){  ProductCategoryID = 6, Name = "Road Bikes"                    },
//              new ProductCategory(){  ProductCategoryID = 7, Name = "Touring Bikes"                 },
//              new ProductCategory(){  ProductCategoryID = 8, Name = "Handlebars"                    },
//              new ProductCategory(){  ProductCategoryID = 9, Name = "Bottom Brackets"               },
//              new ProductCategory(){  ProductCategoryID = 10, Name = "Brakes"                       },
//              new ProductCategory(){  ProductCategoryID = 11, Name = "Chains"                       },
//              new ProductCategory(){  ProductCategoryID = 12, Name = "Cranksets"                    },
//              new ProductCategory(){  ProductCategoryID = 13, Name = "Derailleurs"                  },
//              new ProductCategory(){  ProductCategoryID = 14, Name = "Forks"                        },
//              new ProductCategory(){  ProductCategoryID = 15, Name = "Headsets"                     },
//              new ProductCategory(){  ProductCategoryID = 16, Name = "Mountain Frames"              },
//              new ProductCategory(){  ProductCategoryID = 17, Name = "Pedals"                       },
//              new ProductCategory(){  ProductCategoryID = 18, Name = "Road Frames"                  },
//              new ProductCategory(){  ProductCategoryID = 19, Name = "Saddles"                      },
//              new ProductCategory(){  ProductCategoryID = 20, Name = "Touring Frames"               },
//              new ProductCategory(){  ProductCategoryID = 21, Name = "Wheels"                       },
//              new ProductCategory(){  ProductCategoryID = 22, Name = "Bib-Shorts"                   },
//              new ProductCategory(){  ProductCategoryID = 23, Name = "Caps"                         },
//              new ProductCategory(){  ProductCategoryID = 24, Name = "Gloves"                       },
//              new ProductCategory(){  ProductCategoryID = 25, Name = "Jerseys"                      },
//              new ProductCategory(){  ProductCategoryID = 26, Name = "Shorts"                       },
//              new ProductCategory(){  ProductCategoryID = 27, Name = "Socks"                        },
//              new ProductCategory(){  ProductCategoryID = 28, Name = "Tights"                       },
//              new ProductCategory(){  ProductCategoryID = 29, Name = "Vests"                        },
//              new ProductCategory(){  ProductCategoryID = 30, Name = "Bike Racks"                   },
//              new ProductCategory(){  ProductCategoryID = 31, Name = "Bike Stands"                  },
//              new ProductCategory(){  ProductCategoryID = 32, Name = "Bottles and Cages"            },
//              new ProductCategory(){  ProductCategoryID = 33, Name = "Cleaners"                     },
//              new ProductCategory(){  ProductCategoryID = 34, Name = "Fenders"                      },
//              new ProductCategory(){  ProductCategoryID = 35, Name = "Helmets"                      },
//              new ProductCategory(){  ProductCategoryID = 36, Name = "Hydration Packs"              },
//              new ProductCategory(){  ProductCategoryID = 37, Name = "Lights"                       },
//              new ProductCategory(){  ProductCategoryID = 38, Name = "Locks"                        },
//              new ProductCategory(){  ProductCategoryID = 39, Name = "Panniers"                     },
//              new ProductCategory(){  ProductCategoryID = 40, Name = "Pumps"                        },
//              new ProductCategory(){  ProductCategoryID = 41, Name = "Tires and Tubes" }

//            };
//            var models = new ProductModel[]
//            {
//            new ProductModel(){    ProductModelID = 1, Name = "Classic Vest"                } ,
//            new ProductModel(){    ProductModelID = 2, Name = "Cycling Cap"                 } ,
//            new ProductModel(){    ProductModelID = 3, Name = "Full-Finger Gloves"          } ,
//            new ProductModel(){    ProductModelID = 4, Name = "Half-Finger Gloves"          } ,
//            new ProductModel(){    ProductModelID = 5, Name = "HL Mountain Frame"           } ,
//            new ProductModel(){    ProductModelID = 6, Name = "HL Road Frame"               } ,
//            new ProductModel(){    ProductModelID = 7, Name = "HL Touring Frame"            } ,
//            new ProductModel(){    ProductModelID = 8, Name = "LL Mountain Frame"           } ,
//            new ProductModel(){    ProductModelID = 9, Name = "LL Road Frame"               } ,
//            new ProductModel(){    ProductModelID = 10, Name = "LL Touring Frame"           } ,
//            new ProductModel(){    ProductModelID = 11, Name = "Long-Sleeve Logo Jersey"    } ,
//            new ProductModel(){    ProductModelID = 12, Name = "Men's Bib-Shorts"           } ,
//            new ProductModel(){    ProductModelID = 13, Name = "Men's Sports Shorts"        } ,
//            new ProductModel(){    ProductModelID = 14, Name = "ML Mountain Frame"          } ,
//            new ProductModel(){    ProductModelID = 15, Name = "ML Mountain Frame-W"        } ,
//            new ProductModel(){    ProductModelID = 16, Name = "ML Road Frame"              } ,
//            new ProductModel(){    ProductModelID = 17, Name = "ML Road Frame-W"            } ,
//            new ProductModel(){    ProductModelID = 18, Name = "Mountain Bike Socks"        } ,
//            new ProductModel(){    ProductModelID = 19, Name = "Mountain-100" }
//            };

//            Console.WriteLine("===================================Result===========================================");
//            (
//                from p in products
//                join c in categories on p.ProductCategoryID equals c.ProductCategoryID
//                join m in models on p.ProductModelID equals m.ProductModelID
//                select new                      //anonymous object
//                {
//                    p.Name,
//                    cat = c.Name,               //Projection in LINQ.
//                    mod = m.Name,
//                    p.ListPrice,
//                    p.StandardCost,
//                })
//                .ToList()
//                .ForEach(x => Console.WriteLine($"{x.Name}, {x.cat}, {x.mod}, {x.ListPrice}"));

//            Console.ReadKey();
//        }
//    }
//}




//namespace _03_multitasking
//{
//    internal class Program
//    {
//        static async Task Main(string[] args)
//        {
//            Console.WriteLine("Multitasking App\n");
//            Task T1 = ShowTimeAsync();
//            Task T2 = ProcessDataAsync();
//            await Task.WhenAll(T1, T2);         //Runs multiple tasks in parallel and waits for all to complete.

//            Console.WriteLine("\nAll tasks completed.");
//            Console.ReadKey();
//        }
//        static async Task ShowTimeAsync()
//        {
//            for (int i = 1; i <= 15; i++)
//            {
//                Console.WriteLine($"[{i}] Current Time: {DateTime.Now.ToLongTimeString()}");
//                await Task.Delay(1000);
//            }
//        }
//        static async Task ProcessDataAsync()
//        {
//            Console.WriteLine("Data processing started...");
//            await Task.Delay(3000);
//            Console.WriteLine("Data processing completed.");
//        }
//    }
//}
