using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJob1.CheckSenzor
{
    class DataSql
    {
        public void CheckDataBase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();


            cmd.CommandText = "Select * from Senzors where ClientId != 1";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            List<int> ClientId = new List<int>();
            try
            {
                sqlConnection1.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Data is accessible through the DataReader object here.
                    while (reader.Read())
                    {
                        ClientId.Add(Convert.ToInt32(reader.GetValue(0)));

                    }
                }
            }
            catch (SqlException sql)
            {
                Console.WriteLine("The sql syntax has the folowing error :" + sql.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Other exception :" + e.Message);
            }
            if (ClientId.Count() > 0)
            {



                List<DateTime> reset = new List<DateTime>();
                foreach (var Senzor in ClientId)
                {
                    int BateryLevel = 0;
                    DateTime reseting = new DateTime();
                    cmd.CommandText = "Select MAX(ResetDate),BateryLevel from Resetings where SenzorsId =" + Senzor + "group by BateryLevel";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;


                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Data is accessible through the DataReader object here.
                            while (reader.Read())
                            {
                                reseting = Convert.ToDateTime(reader.GetValue(0));
                                BateryLevel = Convert.ToInt32(reader.GetValue(1));
                            }
                        }
                    }
                    catch (SqlException sql)
                    {
                        Console.WriteLine("The sql syntax has the folowing error :" + sql.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Other exception :" + e.Message);
                    }
                    if (reseting != null)
                    {
                        TimeSpan diference = DateTime.Now - reseting;
                        if (diference.Seconds > Int32.Parse("1") && BateryLevel >1)
                        {
                            SqlCommand cmd2 = new SqlCommand();
                            DateTime DateInsert = DateTime.Now;
                            int BateryInsert = BateryLevel - 1;


                            cmd2.CommandText = "Insert into Resetings(ResetDate,BateryLevel,HasChecked,SenzorsId,Clients_id) values ('" + DateTime.Now + "'," + (BateryLevel-1) + ",'false',"+ Senzor + ",null) ";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = sqlConnection1;

                            cmd2.Parameters.AddWithValue("@ResetDate", DateInsert);
                            cmd2.Parameters.AddWithValue("@BateryLevel", BateryInsert);
                            cmd2.Parameters.AddWithValue("@HasChecked", false);
                            cmd2.Parameters.AddWithValue("@SenzorsId", Senzor);
                            int recordsAffected = cmd2.ExecuteNonQuery();

                            Console.WriteLine(recordsAffected);
                            Console.WriteLine("Succes trying to write");
                        }
                    }


                    cmd.CommandText = "Select id,BateryLevel from Senzors where ClientId != 1";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;
                    
                    Dictionary<int, int> data = new Dictionary<int, int>(); 
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Data is accessible through the DataReader object here.
                        while (reader.Read())
                        {
                            data.Add(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)));
                        }
                    }
                    foreach(var key in data.Keys)
                    {
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.CommandText = "Update Senzors Set BateryLevel = " + (data[key]-1) + "where id = "+ key;
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = sqlConnection1;

                      

                        int recordsAffected = cmd1.ExecuteNonQuery();

                        Console.WriteLine(recordsAffected);
                        Console.WriteLine("Succes decerementing the batery");

                    }

                }







            }
        }


    }
}
