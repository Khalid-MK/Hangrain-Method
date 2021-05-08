using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangrain_Method
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            ////////////////////////////      Inputs      /////////////////////////////////////////

            //int[,] input = {{ 14, 5 , 8 , 7 },
            //                { 2 , 12, 6 , 5  },
            //                { 7 , 8 , 3 , 9  },
            //                { 2 , 4 , 6 , 10 }}; // Main Array

            //int[,] input = {{ 5, 3, 2, 8 },
            //                { 7, 9, 2, 6 },
            //                { 6, 4, 5, 7 },
            //                { 5, 7, 7, 8 }};

            //int[,] input = {{ 82, 83, 69, 92 },
            //                { 77, 37, 49, 92 },
            //                { 11, 69, 5 , 86 },
            //                { 8 , 9 , 98, 23 }};


            //int[,] input = {{ 68, 85, 40, 49 },
            //                { 74, 18, 79, 1  },
            //                { 3 , 85, 59, 53 },
            //                { 86, 74, 61, 28 }};

            //int[,] input = {{ 9 , 11, 14, 11, 7  },
            //                { 6 , 15, 13, 13, 10 },
            //                { 12, 13, 6 , 8 , 8  },
            //                { 11, 9 , 10, 12, 9  },
            //                { 7 , 12, 14, 10, 14 }};

            //int[,] input = {{ 42, 57, 74, 85, 34, 66 },
            //                { 54, 33, 27, 30, 80, 88 },
            //                { 3 , 39, 46, 50, 48, 89 },
            //                { 76, 74, 27, 49, 68, 45 },
            //                { 69, 3 , 65, 30, 44, 46 },
            //                { 36, 71, 25, 37, 99, 17 }};

            //int[,] input = {{ 21,  46,  73,   16,  38,   7 ,  21 , 85 },
            //                { 61,  38,   3 ,  56,  71,  41 ,  88 , 39 },
            //                { 91,  89,  49 ,  38,  6 ,  13 ,  98 , 47 },
            //                { 48,  31,  22 ,  65,  31,  63 ,  79 , 89 },
            //                { 98,  77,   1 ,  48,  27,  79 ,  91 , 9  },
            //                { 73,  91,  46 ,  4 ,  97,  10 ,  19 , 63 },
            //                { 11,  65,   9 ,  73,  1 ,  58 ,  33 , 40 },
            //                { 71 , 62,  42 ,  53,  21,  96 ,  14 , 59 }};

            int[,] input = {{ 73,  79,  87,  53,  42,  73,  57,  77,  55,  68 },
                            { 55,  65,  47,  5 ,  75,  88,  72,  71,  82,  43 },
                            { 95,  35,  48,  66,  94,  51,  68,  84,  88,  11 },
                            { 78,  26,  41,  10,  96,  5 ,  46,  89,  24,  35 },
                            { 84,  47,  42,  6 ,  33,  85,  94,  60,  82,  47 },
                            { 16,  71,  92,  18,  91,  44,  20,  68,  48,  94 },
                            { 25,  88,  68,  70,  38,  11,  19,  57,  41,  40 },
                            { 53,  99,  62,  96,  71,  98,  53,  36,  15,  43 },
                            { 94,  93,  36,  42,  83,  77,  42,  86,  80,  35 },
                            { 6 ,  4 ,  26,  77,  69,  34,  55,  60,  96,  89 } };



            ////////////////////////////      Get data Ready      ////////////////////////////////
            int[,] array =(int[,]) input.Clone();   // use the original to calc. "Z"
            DisplayArray(array);
            int[] rowMin = new int[array.GetLength(0)];  // Rows min. values
            for (int i = 0; i < rowMin.Length; i++)
            {
                int min = array[i, 0];
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[i, j] < min)
                    {
                        min = array[i, j];  // Get min. value
                    }
                }
                rowMin[i] = min;  // Fill rowMin with rows  min. values
            }

            for (int i = 0; i < array.GetLength(1); i++)
            {
                int min = array[0, i];
                int j;
                for (j = 0; j < array.GetLength(0); j++)
                {
                    array[j, i] = array[j, i] - rowMin[j]; // Row value - Min. row value
                    if (array[j, i] < min)
                    {
                        min = array[j, i];   // Get Col. min value 
                    }
                }
                if (min != 0)
                {
                    for (int l = 0; l < array.GetLength(0); l++)
                    {
                        array[l, i] = array[l, i] - min; // Col. vlue - min. col. value
                    }
                }
            }
            DisplayArray(array);
            ////////////////////////////      Scanning      ////////////////////////////////

            int[] taskAssigned = new int[array.GetLength(0)]; // array for : assignee --> task 
            int  noOfLines =0;

            ///////// Check if number of lines equal number of jobs or not  //////////////
            do
            {
                // 0 --> Not Marked
                // 1 --> Marked  
                
                int[] markedCol;
                int[] markedRow;
                noOfLines = Mark(array, out markedCol, out markedRow, ref taskAssigned); 
                if (noOfLines < array.GetLength(0))
                {
                    int minValue = GetMinimumValueFromArray(array, markedCol, markedRow);  // Get minimum value in non-marked places
                    SubtractMinimumValueFromArray(array, markedCol, markedRow, minValue);
                }

            } while (noOfLines < array.GetLength(0));// No of lines < No of tasks

            ////////////////////////////      Assign Tasks      ////////////////////////////////
            DisplayArray(array);
            AssignTasks(input, array, taskAssigned);

            Console.ReadKey();
        }

        /// <summary>
        /// Row Scan for get row that have  --1--  item with value "0" and mark its col
        /// </summary>
        /// <param name="array"></param>
        /// <returns>
        /// int[] markedCol = {0 , 1 , 1 , 0 , .... }  
        /// 1 --> Marked
        /// 0 --> Not marked
        /// </returns>
        private static int RowScan(int[,] array, out int[] markedCol, out int[] taskAssigned)
        {
            taskAssigned = new int[array.GetLength(1)];
            markedCol = new int[array.GetLength(1)];
            int count;  // Count Number of zeros in the single row
            int noOfLines = 0;
            for (int i = 0; i < array.GetLength(1); i++)  // i --> counter of rows 
            {
                count = 0;
                for (int j = 0; j < array.GetLength(1); j++)  // j --> counter of cols
                {
                    if (array[i, j] == 0 && markedCol[j] == 0)
                    {
                        count++;
                    }
                }
                if (count == 1)
                {
                    for (int l = 0; l < array.GetLength(1); l++)
                    {
                        if (array[i, l] == 0 && markedCol[l] == 0)
                        {
                            markedCol[l] = 1; // Marked Col.
                            noOfLines++;
                            taskAssigned[i] = l;
                            break;
                        }
                    }
                }
            }
            return noOfLines;
        }

        /// <summary>
        /// Col Scan for get Col that have  --1--  item with value "0" and mark its row
        /// </summary>
        /// <param name="array"></param>
        /// <returns>
        /// int[] markedrow = {0 , 1 , 1 , 0 , .... }  
        /// 1 --> Marked
        /// 0 --> Not marked
        /// </returns>
        private static int ColScan(int[,] array, int[] markedCol, int noOfLines, out int[] markedRow, ref int[] taskAssigned)
        {
            markedRow = new int[array.GetLength(0)];
            int count;  // Count Number of zeros in the single col
            for (int j = 0; j < array.GetLength(0); j++)
            {
                if (markedCol[j] == 0) //Check not marked col
                {
                    count = 0;
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        if (array[i, j] == 0 && markedRow[i] == 0)
                        {
                            count++;
                        }
                    }
                    if (count == 1)
                    {
                        for (int l = 0; l < array.GetLength(1); l++)
                        {
                            if (array[l, j] == 0 && markedRow[l] == 0)
                            {
                                markedRow[l] = 1; // Marked Row
                                noOfLines++;
                                taskAssigned[l] = j;
                                break;
                            }
                        }
                    }
                }
            }
            return noOfLines;
        }

        public static int Mark(int[,] array, out int[] markedCol, out int[] markedRow, ref int[] taskAssigned)
        {
            int colLine = RowScan(array, out markedCol, out taskAssigned);
            int markedLines = ColScan(array, markedCol, colLine, out markedRow, ref taskAssigned);
            bool covered = CheckZerosCovered(array, markedCol, markedRow);
            ///////// Check if lines Covered All zeros //////////////
            if (!covered)
            {
                markedLines = RowScanMultiZeros(array, markedCol, markedRow, markedLines, taskAssigned);
            }
            return markedLines;
        }

        public static bool CheckZerosCovered(int[,] array, int[] markedCol, int[] markedRow)
        {
            bool Covered = true;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (markedCol[j] == 0 && markedRow[i] == 0 && array[i, j] == 0)
                    {
                        Covered = false;
                        return Covered;
                    }
                }
            }
            return Covered;
        }

        public static int GetMinimumValueFromArray(int[,] array, int[] markedCol, int[] markedRow)
        {
            int min = 10000000;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                if (markedRow[i] == 0)
                {
                    for (int j = 0; j < array.GetLength(0); j++)
                    {
                        if (markedCol[j] == 0 && array[i, j] < min)
                        {
                            min = array[i, j];
                        }
                    }
                }
            }

            return min;
        }

        public static void SubtractMinimumValueFromArray(int[,] array, int[] markedCol, int[] markedRow, int min)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (markedCol[j] == 0 && markedRow[i] == 0)
                    {
                        array[i, j] = array[i, j] - min;
                    }
                    else if (markedCol[j] == 1 && markedRow[i] == 1)
                    {
                        array[i, j] = array[i, j] + min;
                    }
                }
            }
        }

        private static int RowScanMultiZeros(int[,] array, int[] markedCol, int[] markedRows, int noOfLines, int[] taskAssigned)
        {
            for (int i = 0; i < array.GetLength(1); i++)  // i --> counter of rows 
            {
                for (int j = 0; j < array.GetLength(1); j++)  // j --> counter of cols
                {
                    if (array[i, j] == 0 && markedCol[j] == 0 && markedRows[i] == 0)
                    {
                        noOfLines++;
                        markedCol[j] = 1;
                        markedRows[i] = 1;
                        taskAssigned[i] = j;
                    }
                }
            }
            return noOfLines;
        }

        public static void AssignTasks(int[,] input, int[,] array, int[] taskAssigned)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            for (int i = 0; i < taskAssigned.Length; i++)
            {
                Console.WriteLine("\t Assignee #{0} ---> Task #{1} \n", i + 1, taskAssigned[i] + 1);
            }
            Console.WriteLine("\t\t");
            int z = 0;
            for (int i = 0; i < taskAssigned.Length; i++)
            {
                z += input[i, taskAssigned[i]];
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t Z = {0}", z);
            Console.ResetColor();
        }

        public static void DisplayArray(int[,] array)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write("\t {0}", array[i, j]);
                }
                Console.WriteLine("\n");
            }
            Console.ReadKey(true);
        }


    }
}
