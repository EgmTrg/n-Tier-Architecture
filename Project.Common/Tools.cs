using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Project.Common
{
    public static class Tools
    {
        const string connectionString = "Server=EGEMEN-PC;Database=NORTHWND;Trusted_Connection=True;";

        private static SqlConnection _connection;

        public static SqlConnection Connection
        {
            get {
                if (_connection == null) {
                    _connection = new SqlConnection(connectionString);
                }
                return _connection;
            }
            set { _connection = value; }
        }

        public static Result<List<ET>> ToList<ET>(this SqlDataAdapter dataTable) where ET : class, new() {
            try {
                DataTable dt = new DataTable();
                dataTable.Fill(dt);
                Type type = typeof(ET);
                List<ET> list = new List<ET>();
                PropertyInfo[] properties = type.GetProperties();

                foreach (DataRow dr in dt.Rows) {
                    ET tip = new ET();
                    foreach (PropertyInfo pi in properties) {
                        object value = dr[pi.Name];
                        if (value != null) {
                            pi.SetValue(tip, value);
                        }
                    }
                    list.Add(tip);
                }
                return new Result<List<ET>> {
                    IsSuccess = true,
                    Message = "Islem Basarili",
                    Data = list
                };
            }
            catch (Exception ex) {
                return new Result<List<ET>> {
                    IsSuccess = false,
                    Message = "Islem Basarisiz! " + ex.Message,
                    Data = null
                };
            }
        }

        public static Result<bool> ExecQuery(this SqlCommand command) {
            try {
                if (command.Connection.State != ConnectionState.Open) {
                    command.Connection.Open();
                    int result = command.ExecuteNonQuery();
                    return new Result<bool> {
                        IsSuccess = true,
                        Message = "Islem Basarili!",
                        Data = result > 0
                    };
                }
            }
            catch (Exception ex) {
                return new Result<bool> {
                    IsSuccess = false,
                    Message = "Hata! " + ex.Message
                };
            }
            finally {
                if (command.Connection.State != ConnectionState.Closed) {
                    command.Connection.Close();
                }
            }
            return new Result<bool> {
                IsSuccess = false,
                Message = "Hata!",
            };
        }
    }
}
