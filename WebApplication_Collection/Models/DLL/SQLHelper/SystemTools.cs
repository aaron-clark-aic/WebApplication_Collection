using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
namespace SQLHelper
{
	public class SystemTools
	{
		public static DataTable ConvertDataReaderToDataTable(SqlDataReader dataReader)
		{
			DataTable table = new DataTable();
			DataTable schemaTable = dataReader.GetSchemaTable();
			DataTable result;
			try
			{
				foreach (DataRow row in schemaTable.Rows)
				{
					DataColumn column = new DataColumn {
						DataType = row.GetType(),
						ColumnName = row[0].ToString()
					};
					table.Columns.Add(column);
				}
				while (dataReader.Read())
				{
					DataRow row2 = table.NewRow();
					for (int i = 0; i < schemaTable.Rows.Count; i++)
					{
						row2[i] = dataReader[i].ToString();
					}
					table.Rows.Add(row2);
				}
				schemaTable = null;
				dataReader.Close();
				result = table;
			}
			catch (Exception ex)
			{
				SystemError.SystemLog(ex.Message);
				throw new Exception(ex.Message, ex);
			}
			return result;
		}
	}
}
