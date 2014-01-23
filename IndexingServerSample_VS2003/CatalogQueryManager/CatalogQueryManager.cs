using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CatalogQueryManager
{
	/// <summary>
	/// class which can be used to query a Indexing Server Catalog
	/// </summary>
	public class CatalogQuery
	{
		/// <summary>
		/// makes sure no instance of this class can be created
		/// </summary>
		private CatalogQuery()
		{ 
		}

		// - the OLEDB data provider to use for searching the Indexing Server is MSIDXS;
		// - when we connect to a specific indexing catalog then you specify the data
		//   source part of the connection string and set it to the indexing catalog name
		const string ProviderConnectionString = "Provider=MSIDXS;";
		const string DataSourceConnectionString = " Data Source=\"{0}\";";

		// form clause and dot-notation character
		const string FormClause = " FROM ";
		const string DotNotation = ".";

		/// <summary>
		/// query a local indexing catalog and return the resultset as a data reader
		/// </summary>
		/// <param name="CatalogName">the catalog to query</param>
		/// <param name="QueryString">the query to execute</param>
		/// <returns>return the data reader with the resultset</returns>
		public static IDataReader LocalQuery(string CatalogName, string QueryString)
		{
			string ConnectionString = ProviderConnectionString + String.Format(DataSourceConnectionString, CatalogName);

			// perform the query and return result
			return Query(ConnectionString, QueryString);
		}

		/// <summary>
		/// query a remote indexing catalog and return the resultset as a data reader
		/// </summary>
		/// <param name="RemoteMachineName">remote machine where indexing server resides</param>
		/// <param name="CatalogName">the catalog to query</param>
		/// <param name="QueryString">the query to execute</param>
		/// <returns>return the data reader with the resultset</returns>
		public static IDataReader RemoteQuery(string RemoteMachineName, string CatalogName, string QueryString)
		{
			// replace the FORM clause with a FORM remote_machine_name.catalog_name..from_clause, 
			// for example FROM SCOPE() against the remote machine enterpriseminds and the catalog
			// web becomes FROM enterpriseminds.web.SCOPE(); this allows to query remote indexing catalogs
			QueryString = QueryString.Replace(FormClause, FormClause + RemoteMachineName + DotNotation + CatalogName + DotNotation + DotNotation);

			// perform the query and return result
			return Query(ProviderConnectionString, QueryString);
		}

		/// <summary>
		/// query a indexing catalog (conenction string determines if local or remote) and return
		/// the resultset as a data reader
		/// </summary>
		/// <param name="ConnectionString">the indexing catalog connection string to use</param>
		/// <param name="QueryString">the query to execute</param>
		/// <returns>return the data reader with the resultset</returns>
		public static IDataReader Query(string ConnectionString, string QueryString)
		{
			// get a OLEDB connection object and set the connection string
			OleDbConnection Connection = new OleDbConnection();
			Connection.ConnectionString = ConnectionString;

			// set the query string to execute
			IDbCommand Command = Connection.CreateCommand();
			Command.CommandText = QueryString;
			Command.CommandType = CommandType.Text;

			// open the data connection
			Connection.Open();

			// execute the query and return the data reader; when it gets closed it
			// also closes the connection object
			return Command.ExecuteReader();
		}
	}
}
