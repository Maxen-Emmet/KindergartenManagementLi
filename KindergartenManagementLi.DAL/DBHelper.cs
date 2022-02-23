using System;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// 数据库操作工具类
/// </summary>
public class DBHelper
{
    #region 数据库连接字符串
    private static string connectionString = @"Data Source=LIPENG\SQLEXPRESS;Initial Catalog=MobileManager;Persist Security Info=True;User ID=sa;Password=sa";

    #endregion

    #region 与数据库建立连接
    private static SqlConnection conn;
    /// <summary>
    /// 数据库连接对象
    /// </summary>
    public static SqlConnection Conn
    {
        get
        {
            if (conn == null)
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
            }
            else if (conn.State == ConnectionState.Broken)
            {//如果处于停止状态则先关闭后打开
                conn.Close();
                conn.Open();
            }
            else if (conn.State == ConnectionState.Closed)
            {//如果处于关闭状态则直接打开
                conn.Open();
            }

            return conn;
        }
    }
    #endregion

    #region 关闭数据库连接 
    /// <summary>
    /// 关闭数据库连接
    /// 调用方法：DBHelper.CloseConnection();
    /// </summary>
    public static void CloseConnection()
    {
        if (Conn == null) return;
        //如果处于打开或者停止状态则关闭
        if (Conn.State == ConnectionState.Open || Conn.State == ConnectionState.Broken)
        {
            Conn.Close();//关闭
        }

    }
    #endregion

    #region 增 删 改 操作方法
    /// <summary>
    /// 增 删 改 操作方法
    /// 调用方法：int row = DBHelper.ExecuteNonQuery(sql);
    /// </summary>
    /// <param name="sql">要执行的sql命令</param>
    /// <returns>int 受影响的行数</returns>
    public static int ExecuteNonQuery(string sql)
    {
        int row = 0;
        try
        {
            //创建执行对象
            SqlCommand cmd = new SqlCommand(sql, Conn);
            //开始执行，并返回结果
            row = cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            row = -1;//表示发生异常
            throw new Exception("异常信息:" + e.Message);
        }
        finally
        {
            CloseConnection();//关闭
        }
        return row;//返回结果
    }
    #endregion

    #region 查询操作
    
    #region 查询返回单个值
    /// <summary>
    /// 查询返回单个值
    /// 可以用于新增返回最新的自增列 SELECT @@Identity;
    /// int result = DBHelper.ExecuteScalar(sql);
    /// </summary>
    /// <param name="sql">要执行的sql命令</param>
    /// <returns>int 返回结果: 为结果集中的第一行的第一列</returns>
    public static int ExecuteScalar(string sql)
    {
        int result = 0;
        try
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception e)
        {
            result = -1;//表示发生异常
            throw new Exception("异常信息:" + e.Message);
        }
        finally
        {
            CloseConnection();
        }
        return result;
    }
    #endregion

    #region 查询（连接式，始终保持连接状态）
    /// <summary>
    /// 查询 SqlDataReader（连接式，始终保持数据库连接对象的状态）
    /// 调用方法：SqlDataReader reader = DBHelper.GetDataReader(sql);
    /// </summary>
    /// <param name="sql">要执行的sql命令</param>
    /// <returns>SqlDataReader</returns>
    public static SqlDataReader GetDataReader(string sql)
    {
        SqlDataReader reader = null;
        try
        {
            //创建执行对象
            SqlCommand cmd = new SqlCommand(sql, Conn);
            //开始执行，并返回结果
            reader = cmd.ExecuteReader();
        }
        catch (Exception e)
        {
            throw new Exception("异常信息:" + e.Message);
        }
        return reader;
    }
    #endregion

    #region 查询 DataTable（断开式）
    /// <summary>
    /// 查询（断开式）
    /// 调用方法：DataTable dt = DBHelper.GetTable(sql);
    /// </summary>
    /// <param name="sql">要执行的sql语句</param>
    /// <returns>DataTable</returns>
    public static DataTable GetTable(string sql)
    {
        //1.创建内存中数据表对象
        DataTable table = new DataTable();
        //2.创建适配器
        SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, Conn);
        //3.填充数据到数据表对象
        dataAdapter.Fill(table);
        //4.返回数据表
        return table;
    }
    #endregion

    #region 查询 DataSet（断开式）
    /// <summary>
    /// 查询（断开式，可查询多个表数据信息）
    /// 调用方法：DataSet dataSet = DBHelper.GetDataSet(sql);
    /// </summary>
    /// <param name="sql">要执行的sql语句</param>
    /// <returns>DataSet</returns>
    public static DataSet GetDataSet(string sql)
    {
        //1.创建内存中数据集对象
        DataSet dataSet = new DataSet();
        //2.创建适配器
        SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, Conn);
        //3.填充数据到数据集
        dataAdapter.Fill(dataSet);
        //4.返回数据集
        return dataSet;
    }
    #endregion 

    #endregion

}