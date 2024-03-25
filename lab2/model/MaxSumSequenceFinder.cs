using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.model
{
    public class MaxSumSequenceFinder
    {
        private List<double> _numbers { get; set; }
        private List<double> _maxSumSequence { get; set; }
        private double _maxSum;

        public List<double> Numbers 
        {
            get {  return _numbers; } 
        }

        public List<double> MaxSumSequence 
        { 
            get { return _maxSumSequence; } 
        }

        public double MaxSum
        {
            get { return _maxSum; }
        }

        public bool isResultEmpty
        {
            get { return _maxSumSequence.Count == 0; }
        }

        public string GetStartAndEndIndexes()
        {
            return $"Последовательность с максимальной суммой: {_maxSumSequence.First()}, ..., {_maxSumSequence.Last()}";
        }

        public MaxSumSequenceFinder(List<double> numbers) { 
            this._numbers = numbers;
            this._maxSumSequence = new List<double>();
            this._maxSum = 0;
        }


        public void FindMaxSumSequence()
        {
            double currentSum = 0;
            double maxSumTemp = 0;
            int start = 0;
            int end = 0;
            int tempStart = 0;
            bool sequenceFound = false; // Флаг для обозначения нахождения последовательности

            for (int i = 0; i < _numbers.Count; i++)
            {
                currentSum += _numbers[i];

                if (currentSum < 0)
                {
                    currentSum = 0;
                    tempStart = i + 1;
                }
                else
                {
                    if (!sequenceFound)
                    {
                        // Если еще не найдена последовательность, проверяем, состоит ли текущая сумма из минимум двух элементов
                        if (currentSum > _numbers[i] && currentSum > maxSumTemp)
                        {
                            sequenceFound = true;
                            maxSumTemp = currentSum;
                            start = tempStart;
                            end = i;
                        }
                    }
                    else
                    {
                        // Если последовательность уже найдена, проверяем, является ли текущая сумма больше максимальной
                        if (currentSum > maxSumTemp)
                        {
                            maxSumTemp = currentSum;
                            end = i;
                        }
                    }
                }
            }

            if (sequenceFound)
            {
                _maxSum = maxSumTemp;
                _maxSumSequence.Clear();
                for (int j = start; j <= end; j++)
                {
                    _maxSumSequence.Add(_numbers[j]);
                }
            }
        }

    }

}
