using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExhibitCatalog _catalog;
        public MainWindow()
        {
            InitializeComponent();
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(documentsPath, "exhibits.json");
            _catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), filePath);
            ExhibitsDataGrid.ItemsSource = _catalog.Exhibits;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newExhibit = new Exhibit(
                    NameTextBox.Text,
                    AuthorTextBox.Text,
                    int.Parse(CountRequestTextBox.Text)
                );

                _catalog.AddExhibit(newExhibit);
                NameTextBox.Clear();
                AuthorTextBox.Clear();
                CountRequestTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExhibitsDataGrid.SelectedItem is Exhibit selectedExhibit)
            {
                _catalog.RemoveExhibit(selectedExhibit);
            }
        }

        private void PopularButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.FilterPopularExhibits();
        }
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortByCount();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await _catalog.SaveAsync();
            MessageBox.Show("Каталог успішно збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            await _catalog.LoadAsync();

            ExhibitsDataGrid.ItemsSource = null;
            ExhibitsDataGrid.ItemsSource = _catalog.Exhibits;
            MessageBox.Show("Каталог успішно завантажено.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}