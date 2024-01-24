using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.IO;
using System.Windows;


namespace ZaemService;

public partial class Zaemi : Window
{
    public Zaemi()
    {
        InitializeComponent();
        string fullTable = "SELECT Zaemi.id, Zaemshik.familiya, Dogovor.data_podpisaniya, Sotrudniki.famil FROM Zaemi JOIN Zaemshik ON Zaemi.zaemshik = Zaemshik.id JOIN Dogovor ON Zaemi.dogovor = Dogovor.id JOIN Sotrudniki ON Zaemi.sotrudnik = Sotrudniki.id";
        ShowTable(fullTable);
        CmbFill();
    }
    
    private List<zaemi> zaems;
    private List<sotrudniki> sots;
    private string connStr = "server=localhost;database=ZaemiService;port=3306;User Id=root;password=Qwerty_123456";
    private MySqlConnection conn;
    
    public void ShowTable(string sql)
    {
        zaems = new List<zaemi>();
        conn = new MySqlConnection(connStr);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var Client = new zaemi()
            {
                id = reader.GetInt32("id"),
                familiya  = reader.GetString("familiya"),
                data_podpisaniya = reader.GetString("data_podpisaniya"),
                famil  = reader.GetString("famil"),
            };
            zaems.Add(Client);
        }
        conn.Close();
        DataGrid.ItemsSource = zaems;
    }
    
    private void Search(object? sender, TextChangedEventArgs e)
    {
        var client = zaems;
        client = client.Where(x => x.familiya.Contains(SearchFIO.Text)).ToList();
        DataGrid.ItemsSource = client;
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        ZaemService.Choise back = new ZaemService.Choise();
        Close();
        back.Show(); 
    }

    private void Reset_OnClick(object? sender, RoutedEventArgs e)
    {
        string fullTable = "SELECT Zaemi.id, Zaemshik.familiya, Dogovor.data_podpisaniya, Sotrudniki.famil FROM Zaemi JOIN Zaemshik ON Zaemi.zaemshik = Zaemshik.id JOIN Dogovor ON Zaemi.dogovor = Dogovor.id JOIN Sotrudniki ON Zaemi.sotrudnik = Sotrudniki.id;";
        ShowTable(fullTable);
        SearchFIO.Text = string.Empty;
    }

    private void SotFioCmb(object? sender, SelectionChangedEventArgs e)
    {
        var cmb = (ComboBox)sender;
        var Sotrud = cmb.SelectedItem as sotrudniki;
        var filterSot = zaems
            .Where(x => x.famil == Sotrud.famil)
            .ToList();
        DataGrid.ItemsSource = filterSot;
    }
    
    public void CmbFill()
    {
        sots = new List<sotrudniki>();
        conn = new MySqlConnection(connStr);
        conn.Open();
        MySqlCommand command = new MySqlCommand("select sotrudniki.id, sotrudniki.famil, sotrudniki.name, sotrudniki.otchestv, dolzhnosti.nazvanie from sotrudniki join dolzhnosti on sotrudniki.dolzhnost = dolzhnosti.id;", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentSotrudniki = new sotrudniki()
            {
                id = reader.GetInt32("id"),
                famil = reader.GetString("famil"),
                name = reader.GetString("name"),
                otchestv = reader.GetString("otchestv"),
                nazvanie = reader.GetString("nazvanie")
            };
            sots.Add(currentSotrudniki);
        }
        conn.Close();
        var sotcmb = this.Find<ComboBox>(name: "CmbSot");
        sotcmb.ItemsSource = sots;
    }
    
    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            zaemi currentZaem = DataGrid.SelectedItem as zaemi;
            if (currentZaem == null)
            {
                return;
            }
            conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "DELETE FROM Zaemi WHERE id = " + currentZaem.id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            zaems.Remove(currentZaem);
            ShowTable("SELECT Zaemi.id, Zaemshik.familiya, Dogovor.data_podpisaniya, Sotrudniki.famil FROM Zaemi JOIN Zaemshik ON Zaemi.zaemshik = Zaemshik.id JOIN Dogovor ON Zaemi.dogovor = Dogovor.id JOIN Sotrudniki ON Zaemi.sotrudnik = Sotrudniki.id");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void AddData(object? sender, RoutedEventArgs e)
    {
        zaemi newZaem = new zaemi();
        ZaemService.CrudZaemi add = new ZaemService.CrudZaemi(newZaem, zaems);
        add.Show();
        this.Close();
    }

    private void Edit(object? sender, RoutedEventArgs e)
    {
        zaemi currentZaem = DataGrid.SelectedItem as zaemi;
        if (currentZaem == null)
            return;
        ZaemService.CrudZaemi edit = new  ZaemService.CrudZaemi(currentZaem, zaems);
        edit.Show();
        this.Close();
    }
    
    private void exportButton_Click(object sender, RoutedEventArgs e)
    {
        string outputFile = @"C:\Users\VanoP\Desktop\print\otchet.xlsx";
        string query = "SELECT Zaemi.id, Zaemshik.familiya, Dogovor.data_podpisaniya, Sotrudniki.famil FROM Zaemi JOIN Zaemshik ON Zaemi.zaemshik = Zaemshik.id JOIN Dogovor ON Zaemi.dogovor = Dogovor.id JOIN Sotrudniki ON Zaemi.sotrudnik = Sotrudniki.id";
        MySqlCommand command = new MySqlCommand(query, conn);
        conn.Open();
        MySqlDataReader dataReader = command.ExecuteReader();
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Заемы");
                int row = 1;
                
                for (int i = 1; i <= dataReader.FieldCount; i++)
                {
                    worksheet.Cells[row, i].Value = dataReader.GetName(i - 1);
                }

                while (dataReader.Read())
                {
                    row++;
                    for (int i = 1; i <= dataReader.FieldCount; i++)
                    {
                        worksheet.Cells[row, i].Value = dataReader[i - 1];
                    }
                }

                excelPackage.SaveAs(new FileInfo(outputFile));
            }
            dataReader.Close();
            conn.Close();
        Console.WriteLine("Данные успешно экспортированы в Excel файл.");
    } 
}