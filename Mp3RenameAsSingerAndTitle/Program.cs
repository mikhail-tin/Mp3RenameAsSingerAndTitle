using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Mp3RenameAsSingerAndTitle
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<Byte[], Int32, Int32, String> decodeSequence = (a, b, c) => Encoding.Default.GetString(a, b, c).Replace("\0", "").Trim();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mp3").Select(Path.GetFullPath);
            foreach (var file in files)
            {
                var b = new byte[128];
                FileStream fs = new FileStream(file, FileMode.Open);
                fs.Seek(-128, SeekOrigin.End);
                fs.Read(b, 0, 128);
                if (decodeSequence(b, 0, 3) != "TAG")
                {
                    Console.WriteLine("No TAG for {0}", new FileInfo(file).Name);
                    continue;
                }
                var title = decodeSequence(b, 3, 30);
                var singer = decodeSequence(b, 33, 30);
                fs.Close();

                var fi = new FileInfo(file);
                var newName = singer + " - " + title + ".mp3"; 
                File.Move(file, Path.Combine(fi.DirectoryName, newName));
                Console.WriteLine("{0} => {1} {2}", fi.Name, newName, Environment.NewLine);
            }
            Play();
            Console.WriteLine("Press any key for exit");
            Console.ReadKey();
        }

        static void Play()
        {
            Console.Beep(700, 400);Console.Beep(800, 200); Console.Beep(600, 400);Console.Beep(700, 200);
            Console.Beep(600, 100);Console.Beep(700, 100);Console.Beep(600, 100);Console.Beep(700, 100);
            Console.Beep(700, 300);Console.Beep(800, 400);     
        }
    }
}
