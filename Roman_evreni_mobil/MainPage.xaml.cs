    using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.IO;
using System.Collections.Generic;

namespace Roman_evreni_mobil
{
    public partial class Mainpage : ContentPage
    {
        public Mainpage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
{
    base.OnAppearing();
    if (Aktif_evren.Mevcut != null)
    {
        Evren_adi_label.Text = Aktif_evren.Mevcut.Evren_adi;
        Karakter_btn.Text = $"Karakterler ({Aktif_evren.Mevcut.Karakterler.Count})";
        Mekan_btn.Text = $"Mekanlar ({Aktif_evren.Mevcut.Mekanlar.Count})";
        Olay_btn.Text = $"Olay orgusu ({Aktif_evren.Mevcut.Olaylar.Count})";
        Roman_btn.Text = $"Romanlar ({Aktif_evren.Mevcut.Romanlar.Count})";
        Not_btn.Text = $"Notlar ({Aktif_evren.Mevcut.Notlar.Count})";
    }
    await Ana_kutu.FadeTo(1, 300);
}

        private void On_arama_degisti(object sender, TextChangedEventArgs e)
        {
            string arama = e.NewTextValue?.Trim().ToLower() ?? "";

            if (string.IsNullOrEmpty(arama))
            {
                Arama_scroll.IsVisible = false;
                Arama_sonuclari.Children.Clear();
                return;
            }

            Arama_sonuclari.Children.Clear();
            bool sonuc_var = false;

            if (Aktif_evren.Mevcut == null) return;

            // Karakterler
            foreach (var k in Aktif_evren.Mevcut.Karakterler.Where(x => x.Ad.ToLower().Contains(arama)))
            {
                Sonuc_ekle("👤 " + k.Ad, "Karakter", async () =>
                {
                    await Navigation.PushModalAsync(new Karakter_detay_page(k));
                });
                sonuc_var = true;
            }

            // Mekanlar
            foreach (var m in Aktif_evren.Mevcut.Mekanlar.Where(x => x.Ad.ToLower().Contains(arama)))
            {
               Sonuc_ekle("🏰 " + m.Ad, "Mekan", async () =>
               {
                   await Navigation.PushModalAsync(new Mekan_detay_page(m));
               });
                sonuc_var = true;
            }

            // Olaylar
            foreach (var o in Aktif_evren.Mevcut.Olaylar.Where(x => x.Ad.ToLower().Contains(arama)))
            {
                Sonuc_ekle("⚡ " + o.Ad, "Olay", async () =>
                {
                    await Navigation.PushModalAsync(new Olay_detay_page(o));
                });
                sonuc_var = true;
            }

            // Romanlar
            foreach (var r in Aktif_evren.Mevcut.Romanlar.Where(x => x.Ad.ToLower().Contains(arama)))
            {
                Sonuc_ekle("📖 " + r.Ad, "Roman", async () =>
                {
                    await Navigation.PushModalAsync(new Roman_detay_page(r));
                });
                sonuc_var = true;
            }

            // Notlar
            foreach (var n in Aktif_evren.Mevcut.Notlar.Where(x => x.Baslik.ToLower().Contains(arama)))
            {
                Sonuc_ekle("📝 " + n.Baslik, "Not", async () =>
                {
                    await Navigation.PushModalAsync(new Not_detay_page(n));
                });
                sonuc_var = true;
            }

            if (!sonuc_var)
                Sonuc_ekle("Sonuç bulunamadı", "", null);

            Arama_scroll.IsVisible = true;
        }

        private void Sonuc_ekle(string baslik, string tur, Func<System.Threading.Tasks.Task>? tikla)
        {
            var satir = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                Padding = new Thickness(0, 12)
            };

            satir.Add(new Label
            {
                Text = baslik,
                TextColor = Color.FromArgb("#f5f5f5"),
                FontSize = 16,
                FontFamily = "Serif",
                VerticalOptions = LayoutOptions.Center
            }, 0, 0);

            if (!string.IsNullOrEmpty(tur))
            {
                satir.Add(new Label
                {
                    Text = tur,
                    TextColor = Color.FromArgb("#525252"),
                    FontSize = 12,
                    VerticalOptions = LayoutOptions.Center
                }, 1, 0);
            }

            if (tikla != null)
            {
                var tap = new TapGestureRecognizer();
                tap.Tapped += async (s, e) =>
                {
                    Entry_arama.Text = "";
                    Arama_scroll.IsVisible = false;
                    await tikla();
                };
                satir.GestureRecognizers.Add(tap);
            }

            Arama_sonuclari.Children.Add(satir);
            Arama_sonuclari.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#1f1f1f") });
        }

        private async void On_yedekle_clicked(object sender, EventArgs e)
{
    try
    {
        var tum_evrenler = Json_motoru.Yukle();
        var json = System.Text.Json.JsonSerializer.Serialize(tum_evrenler, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        
        string dosya_adi = $"roman_evreni_yedek_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        string yol = Path.Combine(FileSystem.AppDataDirectory, dosya_adi);
        await File.WriteAllTextAsync(yol, json);

        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Roman Evreni Yedek",
            File = new ShareFile(yol)
        });
    }
    catch (Exception ex)
    {
        await DisplayAlert("Hata", ex.Message, "Tamam");
    }
}

private async void On_geri_yukle_clicked(object sender, EventArgs e)
{
    try
    {
        var sonuc = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Yedek dosyasını seç"
        });

        if (sonuc == null) return;

        using var stream = await sonuc.OpenReadAsync();
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        var evrenler = System.Text.Json.JsonSerializer.Deserialize<List<Evren_verisi>>(json);
        if (evrenler == null || evrenler.Count == 0)
        {
            await DisplayAlert("Hata", "Geçersiz yedek dosyası.", "Tamam");
            return;
        }

        bool onay = await DisplayAlert("Uyarı", $"{evrenler.Count} evren yüklenecek. Mevcut veriler silinecek. Devam edilsin mi?", "Evet", "Hayır");
        if (!onay) return;

        Json_motoru.Kaydet(evrenler);
        await DisplayAlert("", "Yedek başarıyla geri yüklendi ✓", "Tamam");
    }
    catch (Exception ex)
    {
        await DisplayAlert("Hata", ex.Message, "Tamam");
    }
}

        private async void On_geri_clicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
        private async void On_kayit_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Yapay_zeka_page());
        private async void On_karakterler_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Karakterlerpage());
        private async void On_mekanlar_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Mekanlarpage());
        private async void On_olaylar_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Olaylarpage());
        private async void On_romanlar_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Romanlarpage());
        private async void On_notlar_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Notlarpage());
        private async void On_asistan_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Yapay_zeka_page());
        private async void On_favoriler_clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Favorilerpage());
    }
}  