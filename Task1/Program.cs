using System;
using System.IO;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\testDeleteFiles";
            int minutesAfterUsed = 30;
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
                            if (DateTime.Now.Subtract(file.LastAccessTime).TotalMinutes > minutesAfterUsed)
                            {
                                Console.WriteLine("Файл " + file.Name + " удален");
                                file.Delete();
                            }
                        }

                        foreach (DirectoryInfo dir in dirs)
                        {
                            if (DateTime.Now.Subtract(dir.LastAccessTime).TotalMinutes > minutesAfterUsed)
                            {

                                Console.WriteLine("Каталог '" + dir.Name + "'` удален");
                                //Directory.Delete(dir.FullName, true);
                                DeleteAllFilesAndDirectories(dir);
                            }
                        }

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
            while (drInfo.GetDirectories().Length != 0 || drInfo.GetFiles().Length != 0);
            drInfo.Delete();

        }
    }

    
}