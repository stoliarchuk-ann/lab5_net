using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Threading.Tasks;

namespace lab5
{
    public class ExhibitCatalog
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;
        public ObservableCollection<Exhibit> Exhibits { get; }
        private List<Exhibit> _allExhibits;

        public ExhibitCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;
            _allExhibits = new List<Exhibit>();
            Exhibits = new ObservableCollection<Exhibit>();
        }

        public void AddExhibit(Exhibit exhibit)
        {
            Exhibits.Add(exhibit);
            _allExhibits.Add(exhibit);
        } 

        public void RemoveExhibit(Exhibit exhibit)
        {
            Exhibits.Remove(exhibit);
            _allExhibits.Remove(exhibit);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allExhibits);
        }

        public void SortByCount()
        {
            var sortedList = new List<Exhibit>(Exhibits);
            sortedList.Sort(new ExhibitCountComparer());

            Exhibits.Clear();
            foreach (var item in sortedList)
            {
                Exhibits.Add(item);
            }
        }

        public void FilterPopularExhibits()
        {
            List<Exhibit> popular = new List<Exhibit>();
            for (int i = 0; i < _allExhibits.Count; i++)
            {
                if (_allExhibits[i].Count > 3) {
                    popular.Add(_allExhibits[i]);
                }
            }
            UpdateVisibleCollection(popular);
        }

        private void UpdateVisibleCollection(List<Exhibit> newList)
        {
            Exhibits.Clear();
            foreach (var exhibit in newList)
            {
                Exhibits.Add(exhibit);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allExhibits);
        }

        public async Task LoadAsync()
        {
            var loadedExhibits = await _fileLoader.LoadAsync<Exhibit>(_filePath);
            if (loadedExhibits != null)
            {
                _allExhibits = loadedExhibits;
                UpdateVisibleCollection(_allExhibits);
            }
        }
    }
}
