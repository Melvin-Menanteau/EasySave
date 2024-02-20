using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySaveUI.Model
{
    public enum SaveType
    {
        COMPLETE,
        DIFFERENTIAL
    }

    public enum SaveState
    {
        ERROR = -1,
        NOT_STARTED = 0,
        IN_PROGRESS = 1,
        FINISHED = 2
    }

    class Save
    {
        private static int _idCounter = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public SaveType SaveType { get; set; }
        public int TotalFilesToCopy { get; set; }
        public int TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public SaveState State { get; set; }
    }
}
