using System;
using System.IO;
using System.Reflection;

namespace tempclean
{
    internal class Program
    {
        static void Main(string[] args)
        {
            titleCw();
            // Paths
            //string userPath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"));
            //string path = $"C:\\Users\\{userPath}\\AppData\\Local\\Temp";

            //string path = Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), Environment.UserName, "example");

            string pathUser = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                pathUser = Directory.GetParent(pathUser).ToString();
            }

            string pathTemp1 = $"{pathUser}\\AppData\\Local\\Temp";
            string pathFetch = $"C:\\Windows\\prefetch";

            string[] files = Directory.GetFiles(pathTemp1);
            int fileCount = files.Length;

            string[] folders = Directory.GetDirectories(pathTemp1);
            int folderCount = folders.Length;

            int count = folderCount + fileCount;
            int deletedF = 0;
            int deletedFl = 0;
            int deleted = 0;

            //bool work = true;

            //while (work)
            //{
            Console.Title = $"temp_cleaner | 0 / {count}";
            Console.WriteLine(@"Found [{0}] files and [{1}] folders in [{2}].

Press ENTER to delete them.", fileCount, folderCount, pathTemp1);
            Console.ReadKey();

            try
            {
                foreach (string entry in Directory.EnumerateFileSystemEntries(pathTemp1))
                {
                    try
                    {

                        if (File.Exists(entry) && !Path.GetExtension(entry).Equals(".tmp", StringComparison.OrdinalIgnoreCase))
                        {
                            File.SetAttributes(entry, FileAttributes.Normal);
                            File.Delete(entry);
                        }

                        if (File.Exists(entry))
                        {
                            using (FileStream stream = new FileStream(entry, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                            {
                                File.Delete(entry);
                                deletedF++;
                                deleted = deletedF + deletedFl;

                                Console.Title = $"temp_cleaner | {deleted} / {count}";

                            }
                        }
                        else if (Directory.Exists(entry))
                        {
                            Directory.Delete(entry, true);
                            deletedFl++;
                            deleted = deletedF + deletedFl;

                            Console.Title = $"temp_cleaner | {deleted} / {count}";
                        }

                    }

                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Not enough perms to delete file: " + entry);
                    }

                    catch (IOException)
                    {
                        Console.WriteLine("File" + entry + " is being used by another process, it cannot be deleted.");
                    }

                    //Console.WriteLine($"Deleted {deleted} / {fileCount}");
                }
                deleted = deletedF + deletedFl;
                Console.Clear();
                titleCw();
                Console.WriteLine(@"
Deleted:

[{0}] Files
[{1}] Folders

[{2}] Total

---------------------------------------------------------------------------------------------------------

Press ENTER to close application.", deletedF, deletedFl, deleted);
                Console.ReadKey();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error with deleting files or folders: " + e.Message);
            }
        }

        static void titleCw()
        {
            Console.WriteLine(@"
    ████████╗███████╗███╗░░░███╗██████╗░  ░█████╗░██╗░░░░░███████╗░█████╗░███╗░░██╗███████╗██████╗░
    ╚══██╔══╝██╔════╝████╗░████║██╔══██╗  ██╔══██╗██║░░░░░██╔════╝██╔══██╗████╗░██║██╔════╝██╔══██╗
    ░░░██║░░░█████╗░░██╔████╔██║██████╔╝  ██║░░╚═╝██║░░░░░█████╗░░███████║██╔██╗██║█████╗░░██████╔╝
    ░░░██║░░░██╔══╝░░██║╚██╔╝██║██╔═══╝░  ██║░░██╗██║░░░░░██╔══╝░░██╔══██║██║╚████║██╔══╝░░██╔══██╗
    ░░░██║░░░███████╗██║░╚═╝░██║██║░░░░░  ╚█████╔╝███████╗███████╗██║░░██║██║░╚███║███████╗██║░░██║
    ░░░╚═╝░░░╚══════╝╚═╝░░░░░╚═╝╚═╝░░░░░  ░╚════╝░╚══════╝╚══════╝╚═╝░░╚═╝╚═╝░░╚══╝╚══════╝╚═╝░░╚═╝

---------------------------------------------------------------------------------------------------------

                              >>> github.com/Ondra9071/temp_cleaner <<<

---------------------------------------------------------------------------------------------------------
");
            }

        }
    }
//}
