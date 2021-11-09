using System;
using System.IO;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\testDeleteFiles";
            long totalSize = 0;
            Console.WriteLine("Введите путь до каталога:");
            path = Console.ReadLine();
            try
            {
                if (path.Trim() != "")
                {
                    DirectoryInfo dr = new DirectoryInfo(path);
                    if (dr.Exists)
                    {
                        Console.WriteLine("Указанный каталог существует");
                        DirectoryInfo[] dirs = dr.GetDirectories();
                        FileInfo[] files = dr.GetFiles();

                        foreach (FileInfo file in files)
                        {
                            totalSize += file.Length;
                        }

                        foreach (DirectoryInfo dir in dirs)
                        {
                            totalSize += CalculateSizeAllFilesAndDirectories(dir);
                        }

                        Console.WriteLine($"Размер указанного каталога составляет: {totalSize}");
                    }
                    else
                    {
                        Console.WriteLine("Указанного каталога не существует");
                    }
                }
                else Console.WriteLine("Не указан путь");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        internal static long CalculateSizeAllFilesAndDirectories(DirectoryInfo drInfo)
        {
            DirectoryInfo[] dirs = drInfo.GetDirectories();
            FileInfo[] files = drInfo.GetFiles();
            long totalSize = 0;
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }
            foreach (DirectoryInfo dir in dirs)
            {
                totalSize += CalculateSizeAllFilesAndDirectories(dir);
            }

            return totalSize;

        }
    }

}
