using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace SQLHelper
{
    public class SQLHelper : IDisposable
    {
        private SqlConnection myConnection;
        private static readonly string RETURNVALUE = "RETURNVALUE";
        private string cfgName;
        private static string defaultCfg = "SQLCONNECTIONSTRING";

        public static string DefaultCfg
        {
            get
            {
                return defaultCfg;
            }
            set
            {
                defaultCfg = value;
            }
        }

        public SQLHelper() : this(DefaultCfg) { }

        public SQLHelper(string cfgName)
        {
            this.cfgName = cfgName;
        }

        private void Open()
        {
            if (this.myConnection == null)
            {
                try
                {
                    //在.Net 2.0中, ConfigurationSettings已由ConfigurationManager取代
                    this.myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[cfgName].ConnectionString);
                }
                catch (Exception e)
                {
                    this.myConnection = new SqlConnection(ConfigurationManager.AppSettings[cfgName]);
                }
            }
            if (this.myConnection.State == ConnectionState.Closed)
            {
                try
                {
                    this.myConnection.Open();
                }
                catch (Exception ex)
                {
                    SystemError.SystemLog(ex.Message);
                    throw;
                }
            }
        }
        public void Close()
        {
            if (this.myConnection != null && this.myConnection.State == ConnectionState.Open)
            {
                if (this.transaction != null)
                {
                    this.transaction.Dispose();
                    this.transaction = null;
                }
                this.myConnection.Close();
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Internal variable which checks if Dispose has already been called
        /// </summary>
        private Boolean disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                //TODO: Managed cleanup code here, while managed refs still valid
                if (this.transaction != null)
                {
                    this.transaction.Dispose();
                    this.transaction = null;
                }
                if (this.myConnection != null)
                {
                    this.myConnection.Dispose();
                    this.myConnection = null;
                }
            }
            //TODO: Unmanaged cleanup code here

            disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Call the private Dispose(bool) helper and indicate 
            // that we are explicitly disposing
            this.Dispose(true);

            // Tell the garbage collector that the object doesn't require any
            // cleanup when collected since Dispose was called explicitly.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The destructor for the class.
        /// </summary>
        ~SQLHelper()
        {
            this.Dispose(false);
        }


        #endregion
        
        public int RunProc(string procName)
        {
            SqlCommand sqlCommand = this.CreateCommand(procName, null);
            try
            {
                sqlCommand.CommandTimeout = 240;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                throw;
            }
            finally
            {
                if (this.transaction == null)
                    this.Close();
            }

            return (int)sqlCommand.Parameters[RETURNVALUE].Value;
        }
        public int RunProc(string procName, SqlParameter[] prams)
        {
            SqlCommand sqlCommand = this.CreateCommand(procName, prams);
            try
            {
                sqlCommand.CommandTimeout = 240;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                throw;
            }
            finally
            {
                if (this.transaction == null)
                    this.Close();
            }
            return (int)sqlCommand.Parameters[RETURNVALUE].Value;
            //return sqlCommand.ExecuteNonQuery();
        }
        public void RunProc(string procName, out SqlDataReader dataReader)
        {
            SqlCommand sqlCommand = this.CreateCommand(procName, null);
            dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public void RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader)
        {
            SqlCommand sqlCommand = this.CreateCommand(procName, prams);
            dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
        private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            this.Open();
            SqlCommand sqlCommand = new SqlCommand(procName, this.myConnection);
            if (this.transaction != null)
            {
                sqlCommand.Transaction = this.transaction;
            }
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (prams != null)
            {
                for (int i = 0; i < prams.Length; i++)
                {
                    SqlParameter sqlParameter = prams[i];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            sqlCommand.Parameters.Add(new SqlParameter(RETURNVALUE, SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return sqlCommand;
        }
        public SqlParameter CreateParam(string ParamName, SqlDbType DbType, int Size, ParameterDirection Direction, object Value)
        {
            SqlParameter sqlParameter;
            if (Size > 0)
            {
                sqlParameter = new SqlParameter(ParamName, DbType, Size);
            }
            else
            {
                sqlParameter = new SqlParameter(ParamName, DbType);
            }
            sqlParameter.Direction = Direction;
            if (Direction != ParameterDirection.Output || Value != null)
            {
                sqlParameter.Value = Value;
            }
            return sqlParameter;
        }
        public SqlParameter CreateInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return this.CreateParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }
        public SqlParameter CreateOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return this.CreateParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }
        public SqlParameter CreateReturnParam(string ParamName, SqlDbType DbType, int Size)
        {
            return this.CreateParam(ParamName, DbType, Size, ParameterDirection.ReturnValue, null);
        }

        private SqlTransaction transaction;
        /// <summary>
        /// 将随后的所有操作作为数据库事务执行
        /// </summary>
        /// <returns>表示新事务的对象</returns>
        public SqlTransaction BeginTransaction()
        {
            this.Open();
            this.transaction = this.myConnection.BeginTransaction();
            return this.transaction;
        }
    }
}
