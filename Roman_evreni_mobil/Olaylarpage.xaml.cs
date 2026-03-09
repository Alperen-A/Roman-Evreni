using Microsoft.Maui.Controls;
using System;

namespace Roman_evreni_mobil
{
    public partial class Olaylarpage : ContentPage
    {
        public Olaylarpage()
        {
            InitializeComponent();
        }
        private async void On_geri_clicked(object sender, EventArgs e)
        {
    // Sayfa modal olarak acildigi icin, popmodalasync ile kapatiyoruz
            await Navigation.PopModalAsync();
        }
        private void On_olay_ekle_clicked(object sender, EventArgs e)
        {
            var olay_adi = Entry_olay_adi.Text;

            if (!string.IsNullOrWhiteSpace(olay_adi))
            {
                var yeni_olay = new Label
                {
                    Text = olay_adi,
                    TextColor = Color.FromArgb("#d4d4d4"),
                    FontSize = 18,
                    Padding = new Thickness(0, 10)
                };

                Olay_listesi.Children.Add(yeni_olay);
                Entry_olay_adi.Text = string.Empty;
            }
        }
    }
}