using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Not_detay_page : ContentPage
{
    private Not_kaydi _not;
    private System.Timers.Timer _otomatik_kayit;
    private bool _onizleme_modu = false;
    private Stack<string> _geri_al_stack = new();
    private Stack<string> _yeniden_yap_stack = new();
    private int _numarali_sayac = 1;

    public Not_detay_page(Not_kaydi not)
    {
        InitializeComponent();
        _not = not;

        _otomatik_kayit = new System.Timers.Timer(3000);
        _otomatik_kayit.AutoReset = false;
        _otomatik_kayit.Elapsed += async (s, e) =>
        {
            await MainThread.InvokeOnMainThreadAsync(() => Kaydet());
        };

        Not_baslik_label.Text = _not.Baslik;
        Editor_icerik.Text = _not.Icerik;
        Kelime_sayacini_guncelle(_not.Icerik);
        Son_kayit_label.Text = _not.Son_duzenleme != default
            ? $"Son kayıt: {_not.Son_duzenleme:dd.MM.yyyy HH:mm}"
            : "";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Ana_kutu.FadeTo(1, 300);
    }

    private void On_mod_degistir_clicked(object sender, EventArgs e)
    {
        _onizleme_modu = !_onizleme_modu;
        if (_onizleme_modu)
        {
            Duzenle_scroll.IsVisible = false;
            Onizleme_scroll.IsVisible = true;
            Arac_cubugu_scroll.IsVisible = false;
            Mod_label.Text = "✏️";
            Mod_gostergesi.Text = "👁 Önizleme";
            Markdown_onizle(Editor_icerik.Text ?? "");
        }
        else
        {
            Duzenle_scroll.IsVisible = true;
            Onizleme_scroll.IsVisible = false;
            Arac_cubugu_scroll.IsVisible = true;
            Mod_label.Text = "👁";
            Mod_gostergesi.Text = "✏️ Düzenleme";
        }
    }

    private void Markdown_onizle(string metin)
    {
        Onizleme_kutusu.Children.Clear();
        if (string.IsNullOrWhiteSpace(metin)) return;

        var satirlar = metin.Split('\n');
        foreach (var satir in satirlar)
        {
            var s = satir.TrimEnd();
            if (s.StartsWith("# "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[2..], 28, FontAttributes.Bold, "#f5f5f5"));
            else if (s.StartsWith("## "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[3..], 22, FontAttributes.Bold, "#f5f5f5"));
            else if (s.StartsWith("> "))
                Onizleme_kutusu.Children.Add(Alinti_olustur(s[2..]));
            else if (s == "---")
                Onizleme_kutusu.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#333"), Margin = new Thickness(0, 8) });
            else if (s.StartsWith("- ") || s.StartsWith("• "))
                Onizleme_kutusu.Children.Add(Etiket_olustur("• " + s[2..], 16, FontAttributes.None, "#f5f5f5"));
            else if (s.StartsWith("☐ ") || s.StartsWith("☑ "))
                Onizleme_kutusu.Children.Add(Gorev_olustur(s));
            else if (s.StartsWith(":::kirmizi "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[11..], 16, FontAttributes.None, "#ef4444"));
            else if (s.StartsWith(":::mavi "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[8..], 16, FontAttributes.None, "#3b82f6"));
            else if (s.StartsWith(":::yesil "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[9..], 16, FontAttributes.None, "#22c55e"));
            else if (s.StartsWith(":::sari "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[8..], 16, FontAttributes.None, "#eab308"));
            else if (s.StartsWith(":::mor "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[7..], 16, FontAttributes.None, "#a855f7"));
            else if (s.StartsWith(":::kucuk "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[9..], 12, FontAttributes.None, "#d4d4d4"));
            else if (s.StartsWith(":::buyuk "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[9..], 22, FontAttributes.None, "#f5f5f5"));
            else if (s.StartsWith(":::orta "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[8..], 16, FontAttributes.None, "#f5f5f5", TextAlignment.Center));
            else if (s.StartsWith(":::sag "))
                Onizleme_kutusu.Children.Add(Etiket_olustur(s[7..], 16, FontAttributes.None, "#f5f5f5", TextAlignment.End));
            else if (string.IsNullOrWhiteSpace(s))
                Onizleme_kutusu.Children.Add(new BoxView { HeightRequest = 8 });
            else
                Onizleme_kutusu.Children.Add(Etiket_olustur(s.Replace("**", "").Replace("*", "").Replace("__", ""), 16, FontAttributes.None, "#f5f5f5"));
        }
    }

    private Label Etiket_olustur(string metin, double boyut, FontAttributes stil, string renk, TextAlignment hizalama = TextAlignment.Start)
    {
        return new Label { Text = metin, FontSize = boyut, FontAttributes = stil, TextColor = Color.FromArgb(renk), FontFamily = "Serif", HorizontalTextAlignment = hizalama };
    }

    private Grid Alinti_olustur(string metin)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition { Width = 4 }, new ColumnDefinition { Width = GridLength.Star } } };
        grid.Add(new BoxView { Color = Color.FromArgb("#525252"), WidthRequest = 4 }, 0, 0);
        grid.Add(new Label { Text = metin, TextColor = Color.FromArgb("#a3a3a3"), FontSize = 15, FontFamily = "Serif", FontAttributes = FontAttributes.Italic, Padding = new Thickness(10, 0) }, 1, 0);
        return grid;
    }

    private Label Gorev_olustur(string satir)
    {
        return new Label { Text = satir, FontSize = 16, TextColor = satir.StartsWith("☑") ? Color.FromArgb("#22c55e") : Color.FromArgb("#f5f5f5"), FontFamily = "Serif" };
    }

    private void On_metin_degisti(object sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue)
        {
            _geri_al_stack.Push(e.OldTextValue ?? "");
            _yeniden_yap_stack.Clear();
        }
        Kelime_sayacini_guncelle(e.NewTextValue);
        _otomatik_kayit.Stop();
        _otomatik_kayit.Start();
    }

    private void On_geri_al_clicked(object sender, EventArgs e)
    {
        if (_geri_al_stack.Count > 0)
        {
            _yeniden_yap_stack.Push(Editor_icerik.Text ?? "");
            Editor_icerik.Text = _geri_al_stack.Pop();
        }
    }

    private void On_yeniden_yap_clicked(object sender, EventArgs e)
    {
        if (_yeniden_yap_stack.Count > 0)
        {
            _geri_al_stack.Push(Editor_icerik.Text ?? "");
            Editor_icerik.Text = _yeniden_yap_stack.Pop();
        }
    }

    private void Sarmalayici_ekle(string on_ek, string son_ek)
    {
        Editor_icerik.Text = (Editor_icerik.Text ?? "") + $"{on_ek}buraya yaz{son_ek}";
    }

    private void Satir_basi_ekle(string on_ek)
    {
        Editor_icerik.Text = (Editor_icerik.Text ?? "") + $"\n{on_ek}";
    }

    private void On_kalin_clicked(object sender, EventArgs e) => Sarmalayici_ekle("**", "**");
    private void On_italik_clicked(object sender, EventArgs e) => Sarmalayici_ekle("*", "*");
    private void On_alti_cizili_clicked(object sender, EventArgs e) => Sarmalayici_ekle("__", "__");
    private void On_h1_clicked(object sender, EventArgs e) => Satir_basi_ekle("# ");
    private void On_h2_clicked(object sender, EventArgs e) => Satir_basi_ekle("## ");
    private void On_kucuk_font_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::kucuk ");
    private void On_buyuk_font_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::buyuk ");
    private void On_sola_hizala_clicked(object sender, EventArgs e) { }
    private void On_ortaya_hizala_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::orta ");
    private void On_saga_hizala_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::sag ");
    private void On_madde_liste_clicked(object sender, EventArgs e) => Satir_basi_ekle("- ");
    private void On_numarali_liste_clicked(object sender, EventArgs e) { Satir_basi_ekle($"{_numarali_sayac}. "); _numarali_sayac++; }
    private void On_gorev_liste_clicked(object sender, EventArgs e) => Satir_basi_ekle("☐ ");
    private void On_alinti_clicked(object sender, EventArgs e) => Satir_basi_ekle("> ");
    private void On_ayrac_clicked(object sender, EventArgs e) => Satir_basi_ekle("---");
    private void On_tarih_clicked(object sender, EventArgs e) { Editor_icerik.Text = (Editor_icerik.Text ?? "") + $"\n📅 {DateTime.Now:dd MMMM yyyy, HH:mm}"; }
    private void On_renk_kirmizi_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::kirmizi ");
    private void On_renk_mavi_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::mavi ");
    private void On_renk_yesil_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::yesil ");
    private void On_renk_sari_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::sari ");
    private void On_renk_mor_clicked(object sender, EventArgs e) => Satir_basi_ekle(":::mor ");
    private void On_arkaplan_koyu_clicked(object sender, EventArgs e) => BackgroundColor = Color.FromArgb("#0a0a0a");
    private void On_arkaplan_krem_clicked(object sender, EventArgs e) => BackgroundColor = Color.FromArgb("#1c1510");
    private void On_arkaplan_lacivert_clicked(object sender, EventArgs e) => BackgroundColor = Color.FromArgb("#0f172a");

    private void Kelime_sayacini_guncelle(string metin)
    {
        if (string.IsNullOrWhiteSpace(metin)) { Kelime_sayaci.Text = "0 kelime"; return; }
        int kelime = metin.Trim().Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        Kelime_sayaci.Text = $"{kelime} kelime · {metin.Length} karakter";
    }

    private void Kaydet()
    {
        _not.Icerik = Editor_icerik.Text ?? "";
        _not.Son_duzenleme = DateTime.Now;
        Son_kayit_label.Text = $"Son kayıt: {_not.Son_duzenleme:dd.MM.yyyy HH:mm}";
        var tum_evrenler = Json_motoru.Yukle();
        var guncel = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null)
        {
            guncel.Notlar = Aktif_evren.Mevcut.Notlar;
            Json_motoru.Kaydet(tum_evrenler);
        }
    }

    private async void On_kaydet_clicked(object sender, EventArgs e)
    {
        Kaydet();
        await DisplayAlert("", "Kaydedildi ✓", "Tamam");
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        Kaydet();
        _otomatik_kayit.Stop();
        await Navigation.PopModalAsync();
    }
}