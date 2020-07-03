using System.Collections.Generic;
using System.IO;

namespace SonglistGenerator
{
    class Program
    {
        const string latexFileExtension = ".tex";
        public const string chapterMasterFile = "master" + latexFileExtension;
        public const string songbookMainFile = "main" + latexFileExtension;
        public const string latexFileFilter = "*" + latexFileExtension;

        static void Main(string[] args)
        {            
            var logger = new Logger();
            logger.WriteLine("Hello World!");

            var songRepositoryFolder = args[0];
            logger.WriteLine($"Program will generate list of songs from {songRepositoryFolder}");

            var chapters = new List<Chapter>();       
            var folders = Directory.GetDirectories(songRepositoryFolder);
            foreach (var folder in folders)
            {
                if (!File.Exists(Path.Combine(folder, chapterMasterFile)))
                {
                    logger.WriteLine($"Folder {folder} does not cotain {chapterMasterFile}, ignoring");
                    continue;
                }

                var chapter = new Chapter(folder);
                chapters.Add(chapter);
            }
            logger.WriteLine($"Found {chapters.Count} chapters.");
                         
            foreach (var chapter in chapters)
            {
                var latexFilesInsideChapter = Directory.GetFiles(chapter.Path, latexFileFilter);

                foreach (var latexFilePath in latexFilesInsideChapter)
                {
                    if (Path.GetFileName(latexFilePath) == chapterMasterFile)
                    {
                        // Ignore chapter master file
                        continue;
                    }

                    var song = new Song(latexFilePath);
                    chapter.Songs.Add(song);
                }

                logger.WriteLine($"Found {chapter.Songs.Count} songs in chapter {chapter.FolderName}");
            }

            foreach (var chapter in chapters)
            {
                chapter.Initialize();
                logger.WriteLine($"   Chapter \"{chapter.ChapterName}\" is located in folder \"{chapter.FolderName}\", UseArtists: {chapter.UseArtists}");
                foreach (var song in chapter.Songs)
                {
                    song.Initialize();
                    logger.WriteLine($"      Song");
                }
            }
        }
    }
}
