using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System;

namespace ZaemService;

public partial class CrudZaemi : Window
{
    private List<zaemi> Zaem;
    private zaemi CurrentZaem;
    public CrudZaemi(zaemi currentZaem, List<zaemi> zaems)
    {
        InitializeComponent();
        CurrentZaem = currentZaem;
        this.DataContext = currentZaem;
        Zaem = zaems;
    }
    private MySqlConnection conn;
    private string connStr = "server=localhost;database=ZaemiService;port=3306;User Id=root;password=Qwerty_123456";
    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var zaems = Zaem.FirstOrDefault(x => x.id == CurrentZaem.id);
        if (zaems == null)
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string add = "INSERT INTO Zaemi VALUES (" + Convert.ToInt32(id.Text) + ", '" + zaemFio.Text + "', " + datzaem.Text + ", " + Convert.ToInt32(sot.Text) + ");";
                MySqlCommand cmd = new MySqlCommand(add, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
            }
        }
        else
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string upd = "UPDATE Zaemi SET zaemshik = '" + Convert.ToInt32(zaemFio.Text) + "', dogovor = '" +  Convert.ToInt32(datzaem.Text) + "', sotrudnik = '" + Convert.ToInt32(sot.Text) + "' WHERE id = " + Convert.ToInt32(id.Text) + ";";
                MySqlCommand cmd = new MySqlCommand(upd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.Write("Error" + exception);
            }
        }
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        ZaemService.Zaemi back = new ZaemService.Zaemi();
        this.Close();
        back.Show(); 
    }
}