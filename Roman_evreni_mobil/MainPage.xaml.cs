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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Aktif_evren.Mevcut != null)
                Evren_adi_label.Text = Aktif_evren.Mevcut.Evren_adi;
        }

        private async void On_geri_clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void On_kayit_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Yapay_zeka_page());
        }

        private async void On_karakterler_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Karakterlerpage());
        }

        private async void On_mekanlar_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Mekanlarpage());
        }

        private async void On_olaylar_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Olaylarpage());
        }

        private async void On_asistan_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Yapay_zeka_page());
        }
    }
}