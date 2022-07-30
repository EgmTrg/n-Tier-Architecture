using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Project.Common
{
    public class ORMBase<ET, OT> : IORM<ET> where ET : class, new()
                                            where OT : class, new()
    {
        private static OT _current;

        public static OT Current
        {
            get {
                if (_current == null) {
                    _current = new OT();
                }
                return _current;
            }
        }

        private Type ET_type => typeof(ET);
        public Table TableAttributes
        {
            get {
                var attributes = ET_type.GetCustomAttributes(typeof(Table), false);
                if (attributes != null && attributes.Any()) {
                    Table tbl = (Table)attributes[0];
                    return tbl;
                }
                return null;
            }
        }

        public Result<bool> Delete(ET entity) {
            throw new NotImplementedException();
        }

        public Result<bool> Insert(ET entity) {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Tools.Connection;

            string query = "Insert into";
            query += string.Format($" {TableAttributes.TableName}");
            string values = " values(";

            PropertyInfo[] properties = ET_type.GetProperties();
            foreach (PropertyInfo prop in properties) {
                if (prop.Name == TableAttributes.IdentityColumn) {
                    continue;
                }

                object value = prop.GetValue(entity);
                if (value == null) continue;
                query += string.Format($"{prop.Name},");
                values += string.Format($"@{prop.Name},");
                cmd.Parameters.AddWithValue(string.Format("@p{0},", prop.Name), value);
            }

            return cmd.ExecQuery();
        }

        public Result<List<ET>> Select() {
            Type type = typeof(ET);

            string query = "Select * from ";

            var att = type.GetCustomAttributes(typeof(Table), false);
            if (att != null && att.Any()) {
                Table tbl = (Table)att[0];
                query += tbl.TableName;
            }

            SqlDataAdapter adp = new SqlDataAdapter(query, Tools.Connection);
            return adp.ToList<ET>();
        }

        public Result<bool> Update(ET entity) {
            throw new NotImplementedException();
        }
    }
}
