using System.IO;

namespace SonglistGenerator
{
    class Program
    {
        const string latexFileExtension = ".tex";
        public const string ChapterMasterFile = "master" + latexFileExtension;
        public const string SongbookMainFile = "main" + latexFileExtension;
        public const string LatexFileFilter = "*" + latexFileExtension;

        static void Main(string[] args)
        {            
            var logger = new Logger();
            var songlist = new Songlist(logger);
            logger.WriteLine("Hello World!");

            var songRepositoryFolder = args[0];
            logger.WriteLine($"Program will generate list of songs from {songRepositoryFolder}");

            var folders = Directory.GetDirectories(songRepositoryFolder);
            foreach (var folder in folders)
            {
                if (!File.Exists(Path.Combine(folder, ChapterMasterFile)))
                {
                    logger.WriteLine($"Folder {folder} does not cotain {ChapterMasterFile}, ignoring");
                    continue;
                }

                var chapter = new Chapter(folder);
                songlist.Add(chapter);
            }

            logger.WriteLine($"Found {songlist.NumberOfChapters} chapters.");

            songlist.ReadAllSongs();
            songlist.Initialize();

            logger.WriteLine("----- NEW MAIN.TEX -----");
            logger.WriteLine(songlist.NewMainFile());
        }
    }
}
