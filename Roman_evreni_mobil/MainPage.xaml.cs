using System;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil
{
    public partial class Mainpage : ContentPage
    {
        public Mainpage()
        {
            InitializeComponent();
        }
        

        private async void On_kayit_clicked(object sender, EventArgs e)
        {
            // Shell yapisi ile kayit sayfasina gidiyoruz
            await Shell.Current.GoToAsync("Kayitpage"); 
        }

        private async void On_karakterler_clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Karakterlerpage");
        }

        private async void On_mekanlar_clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Mekanlarpage");
        }

        private async void On_olaylar_clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Olaylarpage");
        }

        private void On_asistan_clicked(object sender, EventArgs e)
        {
            // Asistan kodlari buraya gelecek
        }
    }
}