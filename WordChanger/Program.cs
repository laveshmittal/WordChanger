using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;

namespace WordChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            //Input File Path
            const string inputFilePath = @"..\..\..\data.txt";

            //Temporary Outfile Path to write result and rename it later
            var outputFilePath = Directory.GetParent(inputFilePath) + "\\tempOutput.txt";


            Console.WriteLine("\tWelcome to Word Changer!");
            
            Console.Write("Please enter a word you wish to replace:"); 
            var wordToReplace = Console.ReadLine();
            
            Console.Write($"Please enter a word you wish to replace \"{wordToReplace}\" by:"); 
            var replaceBy = Console.ReadLine();

             Console.Write($"How many occurences of \"{wordToReplace}\" you wish to replace by \"{replaceBy}\" (inputType:number):");
             var inputCount = Console.ReadLine();

             int count;

             //Parsing input count to integer32
             if (!int.TryParse(inputCount, out count))
             {
                 Console.WriteLine("Error:Invalid Count. Please provide a valid number");
                 return;
             }

            

            //Getting List of Words from text file
            var inputFileWordList = GetWordListFromFile(inputFilePath);

            //creating a copy of word list for processing
            var outputFileWordList = new List<string>(inputFileWordList);

            var wordsReplacedCount = ReplaceListContents(outputFileWordList, wordToReplace, replaceBy, count);

            if (wordsReplacedCount < count)
            {
                Console.WriteLine($"File contains only {wordsReplacedCount} occurences out of {count} of \"{wordToReplace}\". Do you still wish to replace " +
                                  $"words? (press yes to continue or any key to abort) ");
                var decision = Console.ReadLine();
                if (decision != null &&decision.ToLower() == "yes")
                {
                    WriteWordListToFile(outputFilePath, outputFileWordList);
                }
                else
                {
                    Console.WriteLine("Word Replacement Aborted");
                    return;
                }
            }
            else
            {
                WriteWordListToFile(outputFilePath, outputFileWordList);
            }

            Console.WriteLine("Word Replacement Process Completed.");
            //Close the output file and rename it to input file
            File.Delete(inputFilePath);
            File.Move(outputFilePath, inputFilePath);
        }

        public static void WriteWordListToFile(String outputFilePath, List<string> wordList)
        {
            var outputFile = new FileInfo(outputFilePath).OpenWrite();
            var outputString = String.Join(" ", wordList);
            outputFile.Write(Encoding.ASCII.GetBytes(outputString));
            outputFile.Close();

        }

        public static List<string> GetWordListFromFile(string path)
        {
            var inputFileText = File.ReadAllText(path);
            return inputFileText.Split(" ").ToList();
        }

        public static int ReplaceListContents(List<string> list, string inputWord, string outputWord, int count)
        {
            var wordsReplaced = 0;
            for (var i = 0; i < list.Count; i++)
            { 
                if (wordsReplaced < count)
                {
                    if (list[i] == inputWord)
                    {
                        list[i] = outputWord;
                        wordsReplaced++;
                    }
                }
                else break;
            }

            return wordsReplaced;
        }
    }
}
