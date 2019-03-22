using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMetadataCheater
{
    class FileModel
    {
        public string filePath { get; private set; }
        public DateTime modifyDate
        {
            get
            {
                return fileInfo.LastWriteTime;
            }
        }
        public FileInfo fileInfo { get; private set; }

        public FileModel(FileInfo _fileInfo)
        {
            filePath = _fileInfo.FullName;
            fileInfo = _fileInfo;
        }

        public DateTime SetModifyDate(int minutes, DateTime fromTime)
        {
            fileInfo.LastWriteTime = fromTime.AddMinutes(minutes);
            return fileInfo.LastWriteTime;
        }

        public void SetModifyDate(DateTime dateTime)
        {
            fileInfo.LastWriteTime = dateTime;
        }

        public DateTime SetModifyDate(int minMinutes, int maxMinutes, DateTime fromTime)
        {
            var random = new Random();
            return SetModifyDate(random.Next(minMinutes, minMinutes) * (-1), fromTime);
        }
    }
}
