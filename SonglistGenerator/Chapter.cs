using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SonglistGenerator
{
    class Chapter : IDiskLocationRepresentation
    {
        string masterFileContent;

        public Chapter(string folder)
        {
            this.Path = folder;            
        }

        public void Initialize()
        {            
            this.masterFileContent = File.ReadAllText(System.IO.Path.Combine(this.Path, Program.chapterMasterFile));
            this.UseArtists = masterFileContent.Contains("\\Zespoltrue") && masterFileContent.Contains("\\Zespolfalse");
            this.FolderName = new DirectoryInfo(this.Path).Name;
            this.ChapterName = Regex.Match(masterFileContent, @"(?<=\\chapter{).*?(?=})").Value;
        }

        public string Path { get; private set; }

        /// <summary>
        /// Defines whether \Zespoltrue and \Zespolfalse sections are added to master.tex.
        /// </summary>
        public bool UseArtists { get; private set; }

        /// <summary>
        /// Name of subfolder which contains master.tex file.
        /// </summary>
        public string FolderName { get; private set; }

        /// <summary>
        /// Name of chapter read from master.tex file.
        /// </summary>
        public string ChapterName { get; private set; }

        public List<Song> Songs { get; } = new List<Song>();
    }
}
