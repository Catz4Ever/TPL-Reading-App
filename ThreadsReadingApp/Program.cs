using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ThreadsReadingApp
{
    internal class Program
    {
        static void returnNumberOfWords(object path)
        {
            string filePath = path.ToString();
            int wordCount = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    wordCount += words.Length;
                }
            }

            Console.WriteLine($"Number of words:{wordCount}");
        }

        static void returnLongestWord(object path)
        {
            string filePath = path.ToString();
            string longestWord = string.Empty;

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (word.Length > longestWord.Length)
                        {
                            longestWord = word;
                        }
                    }
                }
    
                Console.WriteLine($"Longest Word: {longestWord}");
            }
        }

        static void returnShortestWord(object path)
        {
           string filePath = path.ToString();
           string shortestWord = string.Empty;

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (word.Length >= 4 && (shortestWord == string.Empty || word.Length < shortestWord.Length))
                        {
                            shortestWord = word;
                        }
                    }
                }
            }

            Console.WriteLine($"Shortest Word: {shortestWord}");
        }

        static void returnCommonWords(object path)
        {
            string filePath = path.ToString();
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (!string.IsNullOrWhiteSpace(word) && word.Length >= 4)
                        {
                            string cleanedWord = word.ToLower();
                            if (wordFrequency.ContainsKey(cleanedWord))
                                wordFrequency[cleanedWord]++;
                            else
                                wordFrequency[cleanedWord] = 1;
                        }
                    }
                }
            }

            Dictionary<string, int> mostCommonWords = new Dictionary<string, int>();
            int count = 0;

            foreach (var entry in wordFrequency.OrderByDescending(x => x.Value))
            {
                mostCommonWords[entry.Key] = entry.Value;
                count++;

                if (count >= 5)
                    break;
            }

            Console.Write("Five most common words: ");
            foreach (var entry in mostCommonWords)
            {
                Console.Write($"{entry.Key}: {entry.Value}, ");
            }
            Console.WriteLine();
        }

        static void returnUncommonWords(object path)
        {
            string filePath = path.ToString();
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (!string.IsNullOrWhiteSpace(word) && word.Length >= 4)
                        {
                            string cleanedWord = word.ToLower();
                            if (wordFrequency.ContainsKey(cleanedWord))
                                wordFrequency[cleanedWord]++;
                            else
                                wordFrequency[cleanedWord] = 1;
                        }
                    }
                }
            }

            Dictionary<string, int> mostUncommonWords = new Dictionary<string, int>();
            int count = 0;
            foreach (var pair in wordFrequency)
            {
                if (pair.Value == 1 && count < 5)
                {
                    mostUncommonWords[pair.Key] = pair.Value;
                    count++;
                }
            }
            Console.Write("Five most uncommon words: ");
            foreach (var entry in mostUncommonWords)
            {
    
                Console.Write($"{entry.Key}: {entry.Value}, ");
            }
            Console.WriteLine();
        }



        static string RemovePunctuation(string input)
        {
            string cleanText = Regex.Replace(input, @"[^\p{L}\s-]", "");

            cleanText = cleanText.Replace("-", "");

            return cleanText;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string bookName = "Verblud.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), bookName);
            Thread t1 = new Thread(returnNumberOfWords);
            Thread t2 = new Thread(returnLongestWord);
            Thread t3 = new Thread(returnShortestWord);
            Thread t4 = new Thread(returnCommonWords);
            Thread t5 = new Thread(returnUncommonWords);
            t1.Start(filePath);
            t2.Start(filePath);
            t3.Start(filePath);
            t4.Start(filePath);
            t5.Start(filePath);
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine("Elapsed time: " + ts.ToString("mm\\:ss\\.ff"));
            Console.ReadLine();
        }
    }
}