using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace TrainDespetcher
{
     class SQLConnector
    {
        public Train[] arr;
        string host = "server134.hosting.reg.ru";
        
        string database = "u1055215_traininfo";
        string username = "u1055215_admin";
        string password = "6G2w8C0z";
        MySqlConnection sqlConnection;
        string connectionString;


        public SQLConnector()
        {
            connectionString = "Database=" + database + ";Data Source=" + host + ";User Id=" + username + ";Password=" + password;

        }
       
        public async Task selectAll()
        {
            if(sqlConnection == null)
            {
                sqlConnection = new MySqlConnection(connectionString);
                
            }
            
            await sqlConnection.OpenAsync();
            List<Train> trains = new List<Train>();
            DbDataReader sqlData = null;
            
            MySqlCommand command = new MySqlCommand("SELECT * FROM info", sqlConnection);

            try
            {
                
                sqlData = await command.ExecuteReaderAsync();
                int i = 0;

                while (await sqlData.ReadAsync())
                {
                    trains.Add(new Train(Convert.ToString(sqlData["trainNumber"]),
                        Convert.ToString(sqlData["arriveStation"]),
                        Convert.ToString(sqlData["startTime"]),
                        Convert.ToString(sqlData["wayTime"]),
                        Convert.ToInt32(sqlData["tickets"]))
                        );
                                                              
                    i++;
                }
                arr = new Train[trains.Count];
                int q = 0;

                foreach(Train a in trains)
                {
                    arr[q] = a;
                    q++;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlData != null)
                {
                    sqlData.Close();
                    
                }
                sqlConnection.Close();
            }
        }
        public async void insert(string number, string city, string start, string way, int ticket)
        {
            sqlConnection = new MySqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            
            DateTime dt;
            DateTime.TryParse(start,
                   CultureInfo.CreateSpecificCulture("en-US"),
                   DateTimeStyles.None,
                   out dt);
            
            MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO info (id, trainNumber, arriveStation, startTime, wayTime, tickets) " +
                "VALUES (@id, @trainNumber, @arriveStation,  @startTime, @wayTime, @tickets)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("id", arr.Length + 1);
            sqlCommand.Parameters.AddWithValue("trainNumber", number);
            sqlCommand.Parameters.AddWithValue("arriveStation", city);
            sqlCommand.Parameters.AddWithValue("startTime", dt);
            sqlCommand.Parameters.AddWithValue("wayTime", way);
            sqlCommand.Parameters.AddWithValue("tickets", ticket);
            await sqlCommand.ExecuteNonQueryAsync();

            sqlConnection.Close();
            
        }
        public async void update(Train curr, string number, string city, string start, string way, int ticket)
        {
            sqlConnection = new MySqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            DateTime dt;
            DateTime.TryParse(start,
                   CultureInfo.CreateSpecificCulture("en-US"),
                   DateTimeStyles.None,
                   out dt);
            MySqlCommand sqlCommand = new MySqlCommand("UPDATE info SET `trainNumber` = @trainNumber," +
                " `arriveStation` = @arriveStation," +
                " `startTime` = @startTime, `wayTime` = @wayTime, `tickets` = @tickets " +
                "WHERE `id` = @id", sqlConnection);
            int num = 0;
            Train train = Train.searchTrainByNumber(arr, curr.Number);
            for (int i = 0; i < arr.Length; i++)
            {
                if (train.Number == arr[i].Number)
                {
                    num = i + 1;
                    break;
                }
            }
            
            sqlCommand.Parameters.AddWithValue("id", num);
            sqlCommand.Parameters.AddWithValue("trainNumber", number);
            sqlCommand.Parameters.AddWithValue("arriveStation", city);
            sqlCommand.Parameters.AddWithValue("startTime", dt);
            sqlCommand.Parameters.AddWithValue("wayTime", way);
            sqlCommand.Parameters.AddWithValue("tickets", ticket);
            
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            
        }
        public void exit()
        {
            sqlConnection.Close();
        }

    }
}
