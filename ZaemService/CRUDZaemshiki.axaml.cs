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

public partial class CRUDZaemshiki : Window
{
    private List<zaemshik> Client;
    private zaemshik CurrentClient;
    public CRUDZaemshiki(zaemshik currentClient, List<zaemshik> client)
    {
        InitializeComponent();
        CurrentClient = currentClient;
        this.DataContext = currentClient;
        Client = client;
    }
    private MySqlConnection conn;
    private string connStr = "server=localhost;database=ZaemiService;port=3306;User Id=root;password=Qwerty_123456";
    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var zaemsh = Client.FirstOrDefault(x => x.id == CurrentClient.id);
        if (zaemsh == null)
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string add = "INSERT INTO zaemshik VALUES (" + Convert.ToInt32(id.Text) + ", '" + familiya.Text + "', '" + imya.Text + "', '" + otchestvo.Text + "', " + Convert.ToInt32(pasport.Text) + ", '" + telephone.Text + "', '" + adres_raboti.Text + "', '" + zdanie_raboti.Text + "');";
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
                string upd = "UPDATE zaemshik SET familiya = '" + familiya.Text + "', imya = '" +  imya.Text + "', otchestvo = '" + otchestvo.Text + "', pasport = '" + Convert.ToInt32(pasport.Text) + "', telephone = '" + telephone.Text + "', adres_raboti = '" + adres_raboti.Text + "', zdanie_raboti = '" + zdanie_raboti.Text + "' WHERE id = " + Convert.ToInt32(id.Text) + ";";
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
        ZaemService.Zaemshik back = new ZaemService.Zaemshik();
        this.Close();
        back.Show(); 
    }
}