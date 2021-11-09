using System;
using System.IO;
using System.Threading;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\testDeleteFiles";
            int minutesAfterUsed = 30;
            long totalSize = 0;
            long currentTotalSize = 0;
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
                        CalculateTotalSize(dirs,files,out totalSize);
                        Console.WriteLine($"Исходный размер папки: {totalSize} байт");
                        DeleteAfterNoUsed(dirs, files,in minutesAfterUsed);
                        Thread.Sleep(3000); // задержка нужна для того чтобы GC успел удалить все не нужные объекты
                        dirs = dr.GetDirectories();
                        files = dr.GetFiles();
                        CalculateTotalSize(dirs, files, out currentTotalSize);
                        Console.WriteLine($"Освобождено: {totalSize - currentTotalSize} байт");
                        Console.WriteLine($"Текущий размер папки: {currentTotalSize} байт");
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


        internal static void DeleteAfterNoUsed(DirectoryInfo[] dirs, FileInfo[] files, in int minutesAfterUsed)
        {
            foreach (FileInfo file in files)
            {
                if (DateTime.Now.Subtract(file.LastAccessTime).TotalMinutes > minutesAfterUsed)
                {
                    file.Delete();
                }
            }

            foreach (DirectoryInfo dir in dirs)
            {
                if (DateTime.Now.Subtract(dir.LastAccessTime).TotalMinutes > minutesAfterUsed)
                {
                    DeleteAllFilesAndDirectories(dir);
                }
            }
        }

        internal static void CalculateTotalSize(DirectoryInfo[] dirs, FileInfo[] files,out long totalSize)
        {
            totalSize = 0;
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }

            foreach (DirectoryInfo dir in dirs)
            {
                totalSize += CalculateSizeAllFilesAndDirectories(dir);
            }
        }

        internal static void DeleteAllFilesAndDirectories(DirectoryInfo drInfo)
        {
            DirectoryInfo[] dirs = drInfo.GetDirectories();
            FileInfo[] files = drInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in dirs)
            {
                DeleteAllFilesAndDirectories(dir);
            }

            // без этого цикла программа не успевает удалять каталоги большой вложенности и появляется исключение,поэтому перед удалением следует проверить количество вложенных каталогов
            while (drInfo.GetDirectories().Length != 0 || drInfo.GetFiles().Length != 0) ;
            drInfo.Delete();

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
