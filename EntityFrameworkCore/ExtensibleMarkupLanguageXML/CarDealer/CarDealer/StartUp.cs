namespace CarDealer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Data;
    using Dtos.Export;
    using Dtos.Import;
    using Models;
    using ProductShop.Data;

    public class StartUp
    {
        private static void CreateDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Database was deleted!");

            Console.WriteLine(context.Database.EnsureCreated()
                ? $"Database {DbContextConfiguration.DatabaseName} is created!"
                : $"Database {DbContextConfiguration.DatabaseName} is exist!");
        }

        private static void WriteFile(string file, string contents)
        {
            var exportDirectory = @"ExportData\";
            if (!Directory.Exists(exportDirectory))
                Directory.CreateDirectory(exportDirectory);

            var writeDirectory = $"{exportDirectory}{file}";
            File.WriteAllText(writeDirectory, contents);
        }

        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //CreateDatabase(db);

            #region Import data to database

            //var sb = new StringBuilder();
            //var suppliersRead = File.ReadAllText(@"Datasets\suppliers.xml");
            //var partsRead = File.ReadAllText(@"Datasets\parts.xml");
            //var carsRead = File.ReadAllText(@"Datasets\cars.xml");
            //var customersRead = File.ReadAllText(@"Datasets\customers.xml");
            //var salesRead = File.ReadAllText(@"Datasets\sales.xml");

            //sb.AppendLine(ImportSuppliers(db, suppliersRead));
            //sb.AppendLine(ImportParts(db, partsRead));
            //sb.AppendLine(ImportCars(db, carsRead));
            //sb.AppendLine(ImportCustomers(db, customersRead));
            //sb.AppendLine(ImportSales(db, salesRead));

            //Console.WriteLine(sb.ToString().Trim());

            #endregion

            #region Export data from database

            //var getCarsWithDistance = GetCarsWithDistance(db);
            //var getCarsFromMakeBmw = GetCarsFromMakeBmw(db);
            //var getLocalSuppliers = GetLocalSuppliers(db);
            //var getCarsWithTheirListOfParts = GetCarsWithTheirListOfParts(db);
            //var getTotalSalesByCustomer = GetTotalSalesByCustomer(db);
            var getSalesWithAppliedDiscount = GetSalesWithAppliedDiscount(db);

            //var writeCarsWithDistanceFile = "cars.xml";
            //var writeCarsFromMakeBmwFile = "bmw-cars.xml";
            //var writeLocalSuppliersFile = "local-suppliers.xml";
            //var writeCarsWithTheirListOfPartsFile = "cars-and-parts.xml";
            //var writeTotalSalesByCustomerFile = "customers-total-sales.xml";
            var writeSalesWithAppliedDiscountFile = "sales-discounts.xml";

            //WriteFile(writeCarsWithDistanceFile, getCarsWithDistance);
            //WriteFile(writeCarsFromMakeBmwFile, getCarsFromMakeBmw);
            //WriteFile(writeLocalSuppliersFile, getLocalSuppliers);
            //WriteFile(writeCarsWithTheirListOfPartsFile, getCarsWithTheirListOfParts);
            //WriteFile(writeTotalSalesByCustomerFile, getTotalSalesByCustomer);
            WriteFile(writeSalesWithAppliedDiscountFile, getSalesWithAppliedDiscount);

            #endregion

        }

        #region 2. Import Data

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var suppliersDto = XMLConverter.Deserializer<ImportSuplierDto>(inputXml, "Suppliers");
            var suppliers = suppliersDto
                .Select(x => new Supplier
                {
                    Name = x.Name,
                    IsImporter = x.IsImporter
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var partsDto = XMLConverter.Deserializer<ImportPartsDto>(inputXml, "Parts");
            var supliers = context.Suppliers.Select(x => x.Id).ToList();
            var parts = partsDto
                .Where(x => supliers.Contains(x.SupplierId))
                .Select(x => new Part
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SupplierId = x.SupplierId
                })
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsDto = XMLConverter.Deserializer<ImportCarsDto>(inputXml, "Cars");
            var cars = new List<Car>();

            foreach (var dto in carsDto)
            {
                var uniquParts = dto.ImportPartCar.Select(p => p.Id).Distinct().ToArray();
                var parts = uniquParts.Where(x => context.Parts.Any(c => c.Id == x));
                var car = new Car
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TravelledDistance = dto.TravelledDistance,
                    PartCars = parts
                        .Select(id => new PartCar
                        {
                            PartId = id
                        })
                        .ToArray()
                };

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersDto = XMLConverter.Deserializer<ImportCustomersDto>(inputXml, "Customers");
            var customers = customersDto
                .Select(x => new Customer
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver
                })
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var salesDto = XMLConverter.Deserializer<ImportSalesDto>(inputXml, "Sales");
            var sales = salesDto
                .Where(x => context.Cars.Any(c => c.Id == x.CarId))
                .Select(x => new Sale
                {
                    CarId = x.CarId,
                    CustomerId = x.CustomerId,
                    Discount = x.Discount
                })
                .ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }

        #endregion

        #region 3. Query and Export Data

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var rootName = "cars";
            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .Select(x => new GetCarsWithDistanceDto
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToArray();

            var result = XMLConverter.Serializer(cars, rootName);
            return result;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var rootName = "cars";
            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new GetCarsFromMakeBmwDto
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            var result = XMLConverter.Serializer(cars, rootName);
            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var rootName = "suppliers";
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new GetLocalSuppliersDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            var result = XMLConverter.Serializer(suppliers, rootName);
            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var rootName = "cars";
            var cars = context.Cars
                .Select(car => new GetCarsWithTheirListOfPartsDto
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                    Parts = new GetPart
                    {
                        GetParts = car.PartCars
                            .Select(parts => new Parts
                            {
                                Name = parts.Part.Name,
                                Price = parts.Part.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var result = XMLConverter.Serializer(cars, rootName);
            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var rootName = "customers";
            var customers = context.Customers
                .Where(x => x.Sales.Any(s => s.CustomerId == x.Id))
                .Select(x => new GetTotalSalesByCustomerDto
                {
                    Name = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = context.PartCars
                        .Where(pc => x.Sales.Any(s => pc.Car.Id == s.CarId))
                        .Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            var result = XMLConverter.Serializer(customers, rootName);
            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var rootName = "sales";
            var sales = context.Sales
                //.ToArray()
                .Select(sale => new GetSalesWithAppliedDiscountDto
                {
                    Car = context.Cars
                        .Where(car => car.Id == sale.CarId)
                        .Select(c => new CarDto
                        {
                            Make = c.Make,
                            Model = c.Model,
                            TravelledDistance = c.TravelledDistance
                        }).First(),
                    Discount = sale.Discount,
                    CustomerName = sale.Customer.Name,
                    Price = decimal.Parse(sale.Car.PartCars.Sum(pc => pc.Part.Price).ToString("G29")),
                    PriceWithDiscount = decimal.Parse(
                        (sale.Car.PartCars.Sum(pc => pc.Part.Price) -
                         ((sale.Discount / 100) * sale.Car.PartCars.Sum(p => p.Part.Price))).ToString("G29"))
                })
                .ToArray();
            ;

            var result = XMLConverter.Serializer(sales, rootName);
            return result;
        }
        #endregion
    }
}