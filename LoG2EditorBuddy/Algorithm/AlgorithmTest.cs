﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Extensions;
using GAF.Operators;
using System.Collections;
using Log2CyclePrototype.Algorithm;

namespace Log2CyclePrototype
{
     class AlgorithmTest
    {

         public AlgorithmTest() { }


        public void GO()
        {
            const int populationSize = 100;

            //get our cities
            //var cities = CreateCities();
            var cells = CreateCells();

            //Each city is an object the chromosome is a special case as it needs 
            //to contain each city only once. Therefore, our chromosome will contain 
            //all the cities with no duplicates

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population();
			
            //create the chromosomes
            for (var p = 0; p < populationSize; p++)
            {

                var chromosome = new Chromosome();
                foreach (var cell in cells)
                {
                    chromosome.Genes.Add(new Gene(cell));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }
			
            //create the elite operator
            var elite = new Elite(5);
			
            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = GAF.Operators.CrossoverType.DoublePointOrdered
            };
			
            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;
			
            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);
			
            //run the GA
            ga.Run(Terminate);

            Console.Read();
        }

        void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            foreach (var gene in fittest.Genes)
            {
                Console.WriteLine(((Cell)gene.ObjectValue).CellType);
                Logger.AppendText(((Cell)gene.ObjectValue).CellType.ToString());
            }
            //foreach(Cell c in })
        }

        private void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var distanceToTravel = CalculateType(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Distance: {2}", e.Generation, fittest.Fitness, distanceToTravel);

        }

        private static List<City> CreateCities()
        {
            var cities = new List<City>
            {
                new City("Birmingham", 52.486125, -1.890507),
                new City("Bristol", 51.460852, -2.588139),
                new City("London", 51.512161, -0.116215),
                new City("Leeds", 53.803895, -1.549931),
                new City("Manchester", 53.478239, -2.258549),
                new City("Liverpool", 53.409532, -3.000126),
                new City("Hull", 53.751959, -0.335941),
                new City("Newcastle", 54.980766, -1.615849),
                new City("Carlisle", 54.892406, -2.923222),
                new City("Edinburgh", 55.958426, -3.186893),
                new City("Glasgow", 55.862982, -4.263554),
                new City("Cardiff", 51.488224, -3.186893),
                new City("Swansea", 51.624837, -3.94495),
                new City("Exeter", 50.726024, -3.543949),
                new City("Falmouth", 50.152266, -5.065556),
                new City("Canterbury", 51.289406, 1.075802)
            };

            return cities;
        }

        private static List<Cell> CreateCells()
        {
            var cells = new List<Cell>
            {
                new Cell(0,0,1),
                new Cell(0,1,2),
                new Cell(1,0,3),
                new Cell(1,1,4),
            };

            return cells;
        }


        public static double CalculateFitness(Chromosome chromosome)
        {
            var distanceToTravel = CalculateType(chromosome);
            return 1 - distanceToTravel / 10;
        }


        private static double CalculateType(Chromosome chromosome)
        {
            double distanceToTravel = 0.0;
            Cell previousCell = null;

            //run through each city in the order specified in the chromosome
            foreach (var gene in chromosome.Genes)
            {
                var currentCell = (Cell)gene.ObjectValue;

                if (previousCell != null)
                {
                    var distance = previousCell.DistanceTo(currentCell);
                    distanceToTravel += distance;
                }

                previousCell = currentCell;
            }

            return distanceToTravel;
        }

        private static double CalculateDistance(Chromosome chromosome)
        {
            var distanceToTravel = 0.0;
            City previousCity = null;

            //run through each city in the order specified in the chromosome
            foreach (var gene in chromosome.Genes)
            {
                var currentCity = (City)gene.ObjectValue;

                if (previousCity != null)
                {
                    var distance = previousCity.GetDistanceFromPosition(currentCity.Latitude,
                                                                        currentCity.Longitude);
                    distanceToTravel += distance;
                 }

                previousCity = currentCity;
            }

            return distanceToTravel;
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 400;
        }

    }
}


namespace Log2CyclePrototype.Algorithm
{
    [Serializable]
    public class City
    {
        public City(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Name { set; get; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double GetDistanceFromPosition(double latitude, double longitude)
        {
            var R = 6371; // radius of the earth in km
            var dLat = DegreesToRadians(latitude - Latitude);
            var dLon = DegreesToRadians(longitude - Longitude);
            var a =
                System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2) +
                System.Math.Cos(DegreesToRadians(Latitude)) * System.Math.Cos(DegreesToRadians(latitude)) *
                System.Math.Sin(dLon / 2) * System.Math.Sin(dLon / 2)
                ;
            var c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
            var d = R * c; // distance in km
            return d;
        }

        private static double DegreesToRadians(double deg)
        {
            return deg * (System.Math.PI / 180);
        }

        public byte[] ToBinaryString()
        {
            var result = new byte[6];

            return result;
        }

        public override bool Equals(object obj)
        {
            var item = obj as City;
            return Equals(item);
        }

        protected bool Equals(City other)
        {
            return string.Equals(Name, other.Name) && Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                return hashCode;
            }
        }
    }
}




namespace Log2CyclePrototype.Algorithm
{
    [Serializable]
    public class Cell
    {

        public int CellType
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _x, _y;
        private int _type;
        private Hashtable _mapElements;


        public Cell(int x, int y, int type)
        {
            _mapElements = new Hashtable();
            _x = x;
            _y = y;
            _type = type;
            //Enum.TryParse(type, true, out _type);
        }

        public void AddElement(MapElement element)
        {
            _mapElements.Add(element.uniqueID, element);
        }

        /// <summary>
        /// Distance between this Cell and another, useful for heuristics calculation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceTo(Cell other)
        {
            return (double)System.Math.Abs(_type - other._type);
        }

    }
}