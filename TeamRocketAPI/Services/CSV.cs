using IronXL;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

namespace TeamRocketAPI.Services
{
    /// <summary>
    /// Class to load the CSV data into the database when the service starts
    /// as soon the service has stopped it will delete all registers inserted
    /// </summary>
    public class CSV : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private readonly string fileName = "pokemon.csv";
        public CSV(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            EmptyPokemonTableSQLite();
            LoadCSVSqlite();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            EmptyPokemonTable();

            return Task.CompletedTask;
        }

        public void LoadCSVSqlite()
        {
            var path = $@"{env.ContentRootPath}\wwwroot\\{fileName}";


            WorkBook wb = WorkBook.LoadCSV(path, fileFormat: ExcelFileFormat.XLSX, ListDelimiter: ",");
            WorkSheet ws = wb.DefaultWorkSheet;
            DataTable dt = ws.ToDataTable(true);

            string conString = configuration.GetConnectionString("defaultConnection");
            

            using (SqliteConnection con = new SqliteConnection(conString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    var command = con.CreateCommand();
                    command.CommandText =
                    @"
                    INSERT INTO POKEMON(Name,TipeOne,TipeTwo,
                    Total,HP,Attack,Defense,SpAtk,SpDef,Speed,Generation,
                    Legendary)
                    VALUES ($Name,$TipeOne,$TipeTwo,$Total,$HP,$Attack,
                    $Defense,$SpAtk,$SpDef,$Speed,$Generation,$Legendary)
                    ";

                    var parId = command.CreateParameter();
                    var parName = command.CreateParameter();
                    var parTipeOne = command.CreateParameter();
                    var parTipeTwo = command.CreateParameter();
                    var parTotal = command.CreateParameter();
                    var parHP = command.CreateParameter();
                    var parAttack = command.CreateParameter();
                    var parDefense = command.CreateParameter();
                    var parSpAtk = command.CreateParameter();
                    var parSpDef = command.CreateParameter();
                    var parSpeed = command.CreateParameter();
                    var parGeneration = command.CreateParameter();
                    var parLegendary = command.CreateParameter();

                    //parId.ParameterName = "$Id";
                    parName.ParameterName = "$Name";
                    parTipeOne.ParameterName = "$TipeOne";
                    parTipeTwo.ParameterName = "$TipeTwo";
                    parTotal.ParameterName = "$Total";
                    parHP.ParameterName = "$HP";
                    parAttack.ParameterName = "$Attack";
                    parDefense.ParameterName = "$Defense";
                    parSpAtk.ParameterName = "$SpAtk";
                    parSpDef.ParameterName = "$SpDef";
                    parSpeed.ParameterName = "$Speed";
                    parGeneration.ParameterName = "$Generation";
                    parLegendary.ParameterName = "$Legendary";

                    //command.Parameters.Add(parId);
                    command.Parameters.Add(parName);
                    command.Parameters.Add(parTipeOne);
                    command.Parameters.Add(parTipeTwo);
                    command.Parameters.Add(parTotal);
                    command.Parameters.Add(parHP);
                    command.Parameters.Add(parAttack);
                    command.Parameters.Add(parDefense);
                    command.Parameters.Add(parSpAtk);
                    command.Parameters.Add(parSpDef);
                    command.Parameters.Add(parSpeed);
                    command.Parameters.Add(parGeneration);
                    command.Parameters.Add(parLegendary);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //parId.Value = dt.Rows[i]["#"].ToString();
                        parName.Value = dt.Rows[i]["Name"].ToString();
                        parTipeOne.Value = dt.Rows[i]["Type 1"].ToString();
                        parTipeTwo.Value = dt.Rows[i]["Type 2"].ToString();
                        parTotal.Value = dt.Rows[i]["Total"].ToString();
                        parHP.Value = dt.Rows[i]["HP"].ToString();
                        parAttack.Value = dt.Rows[i]["Attack"].ToString();
                        parDefense.Value = dt.Rows[i]["Defense"].ToString();
                        parSpAtk.Value = dt.Rows[i]["Sp. Atk"].ToString();
                        parSpDef.Value = dt.Rows[i]["Sp. Def"].ToString();
                        parSpeed.Value = dt.Rows[i]["Speed"].ToString();
                        parGeneration.Value = dt.Rows[i]["Generation"].ToString();
                        parLegendary.Value = dt.Rows[i]["Legendary"].ToString();

                        command.ExecuteNonQuery();

                    }

                    transaction.Commit();
                    con.Close();

                }

                    
            }

        }

        //load data in Pokemon database table from CSV
        public void LoadCSVSqlServer()
        {
            var path = $@"{env.ContentRootPath}\wwwroot\\{fileName}";
           
            
            WorkBook wb = WorkBook.LoadCSV(path, fileFormat: ExcelFileFormat.XLSX, ListDelimiter: ",");
            WorkSheet ws = wb.DefaultWorkSheet;
            DataTable dt = ws.ToDataTable(true);

            string conString = configuration.GetConnectionString("defaultConnection");
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo.Pokemon";
                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
        }

        //delete data from Pokemon table
        public void EmptyPokemonTable()
        {

            string conString = configuration.GetConnectionString("defaultConnection");
            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = "TRUNCATE TABLE dbo.Pokemon";
                using (SqlCommand sqlCommand = new SqlCommand(query,con))
                {
                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void EmptyPokemonTableSQLite()
        {

            string conString = configuration.GetConnectionString("defaultConnection");

            using (SqliteConnection con = new SqliteConnection(conString))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    var command = con.CreateCommand();
                    command.CommandText = @"DELETE FROM Pokemon";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }

            }

            
        }
    }
}
