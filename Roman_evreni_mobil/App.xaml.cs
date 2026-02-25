using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Uygulama artik animasyonlu giris sayfasindan baslayacak
        MainPage = new Giris_sayfasi();
    }
}