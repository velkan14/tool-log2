using GAF;
using GAF.Extensions;
using GAF.Operators;
using GAF.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EditorBuddyMonster
{
    class CustomCrossover : IGeneticOperator
    {

        private const int MAP_WIDTH = 32;
        private const int MAP_HEIGHT = 32;

        public enum Crossover2DShape
        {
            TwoByTwoSquare,
            ThreeByThreeSquare,
            FourByFourSquare,
            Triangle,
            FourByFourCircle,
            Dispersion
        }

        private readonly object _syncLock = new object();
        private double _crossoverProbabilityS = 1.0;
        private FitnessFunction _fitnessFunctionDelegate;
        private int _evaluations;
        private bool _allowDuplicatesS;
        private Crossover2DShape _crossoverTypeS;
        private ReplacementMethod _replacementMethodS;
        private Population _currentPopulation;
        private Population _newPopulation;
        private int _currentPopulationSize;
        private int _numberOfChildrenToGenerate;

        public event Crossover.CrossoverCompleteHandler OnCrossoverComplete;

        public bool AllowDuplicates
        {
            get
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    return this._allowDuplicatesS;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
            set
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    this._allowDuplicatesS = value;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
        }

        public double CrossoverProbability
        {
            get
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    return this._crossoverProbabilityS;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
            set
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    this._crossoverProbabilityS = value;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
        }

        public Crossover2DShape CrossoverShape
        {
            get
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    return this._crossoverTypeS;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
            set
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    this._crossoverTypeS = value;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
        }

        public ReplacementMethod ReplacementMethod
        {
            get
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    return this._replacementMethodS;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
            set
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    this._replacementMethodS = value;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
        }

        public bool Enabled { get; set; }


        internal CustomCrossover()
            : this(1.0)
        {
        }

        public CustomCrossover(double crossOverProbability)
            : this(crossOverProbability, true)
        {
        }

        public CustomCrossover(double crossOverProbability, bool allowDuplicates)
            : this(crossOverProbability, allowDuplicates, Crossover2DShape.TwoByTwoSquare)
        {
        }

        public CustomCrossover(double crossOverProbability, bool allowDuplicates, Crossover2DShape crossoverShape)
            : this(crossOverProbability, allowDuplicates, crossoverShape, ReplacementMethod.GenerationalReplacement)
        {
        }

        public CustomCrossover(double crossOverProbability, bool allowDuplicates, Crossover2DShape crossoverShape, ReplacementMethod replacementMethod)
        {
            this.CrossoverProbability = crossOverProbability;
            this.AllowDuplicates = allowDuplicates;
            this.ReplacementMethod = replacementMethod;
            this.CrossoverShape = crossoverShape;
            this.Enabled = true;
        }


        public void Invoke(Population currentPopulation,
                            ref Population newPopulation,
                            FitnessFunction fitnessFunctionDelegate)
        {

            if (newPopulation == null)
                newPopulation = new Population(0, 0, currentPopulation.ReEvaluateAll, currentPopulation.LinearlyNormalised, currentPopulation.ParentSelectionMethod);
            if (!this.Enabled)
                return;
            this._fitnessFunctionDelegate = fitnessFunctionDelegate;
            this._currentPopulation = currentPopulation;
            this._newPopulation = newPopulation;
            this.Process();
            newPopulation = this._newPopulation;

        }

        //para cada avaliação, incrementar um contador?
        public int GetOperatorInvokedEvaluations()
        {
            //throw new NotImplementedException();
            return this._evaluations;
        }

        //handles the crossover procedure, taking into account the crossover probability. uses random selection? roulette wheel selection?
        protected void Process()
        {

            int num1 = 100;
            int num2 = 0;
            this._evaluations = 0;
            if (this._replacementMethodS == ReplacementMethod.DeleteLast)
            {
                this._newPopulation.Solutions.Clear();
                this._newPopulation.Solutions.AddRange((IEnumerable<Chromosome>)this._currentPopulation.Solutions);
            }
            else
            {
                List<Chromosome> elites = this._currentPopulation.GetElites();
                num2 = Enumerable.Count<Chromosome>((IEnumerable<Chromosome>)elites);
                if (elites != null && num2 > 0)
                    this._newPopulation.Solutions.AddRange((IEnumerable<Chromosome>)elites);
            }
            this._currentPopulationSize = this._currentPopulation.Solutions.Count;
            this._numberOfChildrenToGenerate = this._currentPopulationSize - num2;
            while (this._numberOfChildrenToGenerate > 0)
            {
                --num1;
                if (num1 <= 0)
                    break;
                Chromosome c1 = (Chromosome)null;
                Chromosome c2 = (Chromosome)null;
                List<Chromosome> list = this._currentPopulation.SelectParents();
                Chromosome p1 = list[0];
                Chromosome p2 = list[1];
                CrossoverData crossoverData = this.CreateCrossoverData(p1.Genes.Count, this.CrossoverShape);
                CrossoverData crossoverResult = this.PerformCrossover(p1, p2, this.CrossoverProbability, this.CrossoverShape, crossoverData, out c1, out c2);
                if (this.OnCrossoverComplete != null)
                    this.OnCrossoverComplete((object)this, new CrossoverEventArgs(crossoverResult));
                if (this.AddChild(c1))
                    --this._numberOfChildrenToGenerate;
                if (this._numberOfChildrenToGenerate > 0 && this.AddChild(c2))
                    --this._numberOfChildrenToGenerate;
            }

        }

        internal List<Gene> ListClone(List<Gene> list)
        {
            List<Gene> nl = new List<Gene>();
            foreach (var g in list)
                nl.Add(g.DeepClone());
            return nl;
        }

        internal CrossoverData PerformCrossover(Chromosome p1, Chromosome p2, double crossoverProbability, Crossover2DShape crossoverShape, CrossoverData crossoverData, out Chromosome c1, out Chromosome c2)
        {
            CrossoverData crossoverData1 = new CrossoverData();
            int count1 = p1.Genes.Count;
            if (count1 != p2.Genes.Count)
                throw new ArgumentException("Parent chromosomes are not the same length.");
            if (crossoverData == null)
                throw new ArgumentException("The CrossoverData parameter is null.");
            List<Gene> source1 = ListClone(p1.Genes);// new List<Gene>();
            List<Gene> source2 = ListClone(p2.Genes);// new List<Gene>();

            if (RandomProvider.GetThreadRandom().NextDouble() <= crossoverProbability)
            {
                //iterate all the replacement points
                foreach (int point in crossoverData.Points){

                    var tmpG = source1[point].DeepClone();
                    source1[point] = source2[point].DeepClone();
                    source2[point] = tmpG;

                }

                //APIClass.CountNeighbours(source1);
                //APIClass.CountNeighbours(source2);

            }

            if (source1.Count != count1 || source1.Count != count1)
                throw new ChromosomeCorruptException("Chromosome is corrupt!");
            c1 = new Chromosome((IEnumerable<Gene>)source1);
            c2 = new Chromosome((IEnumerable<Gene>)source2);
            return crossoverData1;
        }


        /// <summary>
        /// Crossover data represents the points (int) where the crossover will be performed, in this case, we want to switch several points between parents
        /// </summary>
        /// <param name="chromosomeLength"></param>
        /// <param name="crossoverShape"></param>
        /// <returns></returns>
        internal CrossoverData CreateCrossoverData(int chromosomeLength, Crossover2DShape crossoverShape)
        {
            CrossoverData crossoverData = new CrossoverData();

            switch (crossoverShape)
            {
                case Crossover2DShape.TwoByTwoSquare:
                    {
                        int rx, ry;
                        //do
                        //{
                            rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 2);
                            ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 2);
                        //}
                        //while (rx > 30 || ry > 30);

                        for (int yOffset = 0; yOffset < 2; yOffset++)
                            for (int xOffset = 0; xOffset < 2; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case Crossover2DShape.ThreeByThreeSquare:
                    {
                        int rx, ry;
                        //do
                        //{
                            rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 3);
                            ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 3);
                        //}
                        //while (rx > 29 || ry > 29);

                        for (int yOffset = 0; yOffset < 3; yOffset++)
                            for (int xOffset = 0; xOffset < 3; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case Crossover2DShape.FourByFourSquare:
                    {
                        int rx, ry;
                        //do
                        //{
                            rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 4);
                            ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 4);
                        //}
                        //while (rx > 28 || ry > 28);

                        for (int yOffset = 0; yOffset < 4; yOffset++)
                            for (int xOffset = 0; xOffset < 4; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case Crossover2DShape.FourByFourCircle:
                    {
                        int rx, ry;
                        do
                        {
                            rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1);
                            ry = RandomProvider.GetThreadRandom().Next(1, MAP_HEIGHT - 1);
                        }
                        while (rx > 28 || ry > 28);

                        for (int yOffset = 0; yOffset < 4; yOffset++)
                            for (int xOffset = 0; xOffset < 4; xOffset++)
                                if ((yOffset == 0 && xOffset == 0) || (yOffset == 0 && xOffset == 3) || (yOffset == 3 && xOffset == 0) || (yOffset == 3 && xOffset == 3))
                                    continue;
                                else
                                    crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case Crossover2DShape.Triangle: //size??

                    break;

                case Crossover2DShape.Dispersion:

                    break;

                default:
                    {
                        int rx, ry;
                        //do
                        //{
                        rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 4);
                        ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 4);
                        //}
                        //while (rx > 28 || ry > 28);

                        for (int yOffset = 0; yOffset < 4; yOffset++)
                            for (int xOffset = 0; xOffset < 4; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;
            }


            return crossoverData;
        }


        /// <summary>
        /// Add a child to the new population
        /// </summary>
        /// <param name="child"></param>
        /// <returns> True if successful, False otherwise </returns>
        private bool AddChild(Chromosome child)
        {
            bool flag = false;
            if (this._replacementMethodS == ReplacementMethod.DeleteLast)
            {
                child.Evaluate(this._fitnessFunctionDelegate);
                ++this._evaluations;
                if (child.Genes != null && child.Fitness > this._currentPopulation.MinimumFitness && (this.AllowDuplicates || !this._newPopulation.SolutionExists(child)))
                {
                    this._newPopulation.Solutions.Add(child);
                    if (this._newPopulation.Solutions.Count > this._currentPopulationSize)
                    {
                        this._newPopulation.Solutions.Sort();
                        this._newPopulation.Solutions.RemoveAt(this._currentPopulationSize - 1);
                        flag = true;
                    }
                    else
                        flag = true;
                }
            }
            else
            {
                if (this._newPopulation.Solutions.Count + this._numberOfChildrenToGenerate > this._currentPopulationSize)
                {
                    this._numberOfChildrenToGenerate = 0;
                    return false;
                }
                if (child.Genes != null && (this.AllowDuplicates || !this._newPopulation.SolutionExists(child)))
                {
                    this._newPopulation.Solutions.Add(child);
                    flag = true;
                }
            }
            return flag;
        }


        public delegate void CrossoverCompleteHandler(object sender, CrossoverEventArgs e);

    }
}
