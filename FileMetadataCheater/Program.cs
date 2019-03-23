using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FileMetadataCheater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "File Metadata Changer [v1.1]";
            Console.Clear();
            Console.WriteLine("Wprowadź ścieżkę do katalogu z plikami do zmodyfikowania:");
            var dir = Console.ReadLine().Replace("\"", null);

            Console.Write("\r\nWprowadź minimalną liczbę minut od edycji poprzedniego pliku: ");
            var minimalMinutes = int.Parse(Console.ReadLine());
            Console.Write("\r\nWprowadź maksymalną liczbę minut od edycji poprzedniego pliku: ");
            var maximumMinutes = int.Parse(Console.ReadLine());

            Console.Write("\r\nWprowadź liczbę dni, o ile ma zostać przesunięta data pliku (może być 0): ");
            int daysToSkip = 0;

            try
            {
                daysToSkip = int.Parse(Console.ReadLine());
            }

            catch
            {
                daysToSkip = 0;
            }

            Console.WriteLine("\r\n\r\nJeżeli chcesz aby zostały zebrane tylko pliki z specyficznym rozszerzeniem, wprowadź te rozszerzenia");
            Console.WriteLine("rozdzielając je przecinkami.");
            Console.WriteLine("Jeżeli chcesz aby zostały zebrane wszystkie pliki, po prostu wciśnij ENTER");
            Console.Write("Rozszerzenia: ");
            var extensions = Console.ReadLine();

            /*Console.WriteLine("\r\n\r\nWybierz tryb pracy programu:\r\n");
            Console.WriteLine("1) Automatyczny - wszystkie pliki zmodyfikowane w ciągu ostatnich 24h");
            Console.WriteLine("2) Manualny - każdy plik który ma zostać zmodyfikowany zaczyna się znakiem #");

            var locker = true;*/
            var fileModelList = new List<FileModel>();
            /*while (locker == true)
            {
                var choice = Console.ReadKey().KeyChar;
                switch (choice)
                {
                    case '1':*/
            Console.Write("\r\n\r\nSprzed ilu godzin zebrać pliki: ");
            var hoursLimit = int.Parse(Console.ReadLine());
            fileModelList = GetAllFiles(dir, true, extensions, hoursLimit);
            /*locker = false;
            break;

        case '2':
            fileModelList = GetAllFiles(dir, false, extensions);
            locker = false;
            break;
    }
}*/

            //Program engine
            Console.Clear();
            var startTime = fileModelList.ElementAt(0).modifyDate;
            var listSize = fileModelList.Count();

            for (var i = 0; i < listSize; i++)
            {
                var beforeEditTime = fileModelList.ElementAt(i).fileInfo.LastWriteTime;
                startTime = fileModelList.ElementAt(i).SetModifyDate(minimalMinutes, maximumMinutes, startTime);

                if (daysToSkip != 0)
                {
                    var beforeDaysManipulating = fileModelList.ElementAt(i).modifyDate;
                    fileModelList.ElementAt(i).AddDaysToFile(daysToSkip);
                    Console.WriteLine(fileModelList.ElementAt(i).fileInfo.Name + " || " + beforeDaysManipulating.ToShortDateString() + " >> " + fileModelList.ElementAt(i).modifyDate.ToShortDateString());
                }

                Console.WriteLine(fileModelList.ElementAt(i).fileInfo.Name + " || " + beforeEditTime.ToLongTimeString() + " > " + startTime.ToLongTimeString() + "\r\n");
            }

            Console.WriteLine("\r\nWciśnij enter aby jeszcze raz wykonać program...");
            Console.ReadLine();
            Main(new string[] { "" });
        }

        private static List<FileModel> GetAllFiles(string startDir, bool automatic, string extensions, int hoursFromNow)
        {
            var extArr = new string[0];
            if (!String.IsNullOrEmpty(extensions))
            {
                extensions = extensions.Replace(" ", null);
                extArr = extensions.Split(',');

                for (var i = 0; i < extArr.Length; i++)
                    extArr[i] = "*." + extArr[i];
            }

            else
                extArr = new string[] { "*.*" };

            var fileModels = new List<FileModel>();

            foreach (string pattern in extArr)
            {
                var files = Directory.GetFileSystemEntries(startDir, pattern, SearchOption.AllDirectories);

                if (automatic == true)
                {
                    foreach (var e in files)
                    {
                        var file = new FileModel(new FileInfo(e));
                        if ((DateTime.Now - file.modifyDate).TotalHours <= hoursFromNow)
                            fileModels.Add(file);
                    }
                }
            }

            return fileModels.OrderByDescending(x => x.modifyDate).ToList();
        }
    }
}
