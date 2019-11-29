using MusicDownloader.Models;
using MusicDownloader.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace MusicDownloader.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        public ICommand LoadPlayListCommand { get { return new RelayCommand<object>(LoadListAction); } }
        public ICommand DownLoadPlayListCommand { get { return new RelayCommand<object>(DownLoadListAction); } }

        private string _playlist;
        public string Playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Song> ListSong { get; set; }

        public MainViewModel()
        {
            ListSong = new ObservableCollection<Song>();

        }

        private void LoadListAction(object obj)
        {

            ListSong.Clear();
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };


            HtmlDocument document = htmlWeb.Load(_playlist);

            var scriptJson = document.GetElementbyId("jwplayer-0").NextSibling;

            var text = scriptJson.InnerText;
            int a = text.Length - 3;

            var subText = text.Substring(279 + 11, a - 279 - 12);

            var result = JsonConvert.DeserializeObject<List<TestList>>(subText);

            foreach (var song in result)
            {
                ListSong.Add(new Song
                {
                    Name = song.title,
                    Url = song.sources[0].file,
                });
            }
        }


        private void DownLoadListAction(object obj)
        {
            Thread t = new Thread(Download);
            t.Start();

        }

        private void Download()
        {
            foreach (var song in ListSong)
            {
                song.Status = STATUS.DOWNLOADING;
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(song.Url, @"D:\" + song.Name + ".mp3");

                }
                song.Status = STATUS.DOWNLOADED;
            }
        }

    }

    public class TestList
    {
        public string title { get; set; }

        public List<FileList> sources { get; set; }

        public string image { get; set; }
    }

    public class FileList
    {
        public string file { get; set; }
    }
}
