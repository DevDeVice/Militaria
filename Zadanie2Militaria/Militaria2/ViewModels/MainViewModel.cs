using Militaria2.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Product> Products { get; set; }
    public ICommand LoadProductsCommand { get; set; }

    public MainViewModel()
    {
        Products = new ObservableCollection<Product>();
        LoadProductsCommand = new RelayCommand(LoadProducts);
    }

    private void LoadProducts()
    {
        var products = ProductParser.ParseAllSuppliers();
        Products.Clear();
        foreach (var product in products)
        {
            Products.Add(product);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}