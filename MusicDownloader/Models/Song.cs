using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MusicDownloader.Common;

namespace MusicDownloader.Models
{
    public class Song : NotifyPropertyChanged
    {
        public string Url { get; set; }
        public string Name { get; set; }

        private string _status = STATUS.PENDING;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }
}
