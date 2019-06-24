using System;
using CarAdvertiser.Hubs;
using CarAdvertiser.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace CarAdvertiser.Helpers
{
    public static class MessageHelpers
    {
        public static string GetAllUnreadMessages(int userId)
        {
            using (SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CarAdvertiser"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT [Id],SenderId,ReceiverId,MessageContent,IsRead FROM [dbo].[Messages] WHERE IsRead <> 1 AND ReceiverId=@receiverId", connection))
                {
                    cmd.Parameters.AddWithValue("@receiverId", userId);
                    cmd.Notification = null;

                    SqlDependency dependency=new SqlDependency(cmd);
                    dependency.OnChange += Dependency_OnChange;

                    if (connection.State == ConnectionState.Closed) connection.Open();

                    List<MessageViewModel> messages = new List<MessageViewModel>();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            messages.Add(new MessageViewModel
                            {
                                MessageId = (int)dr["Id"],
                                SenderId = (int)dr["SenderId"],
                                ReceiverId = (int)dr["ReceiverId"],
                                Message = dr["MessageContent"].ToString(),
                                IsRead = (bool)dr["IsRead"]
                            });
                        }
                        dr.Close();
                    }
                    
                    if (connection.State == ConnectionState.Open) connection.Close();

                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(messages);

                    return json;
                }
            }
        }

        public static string GetAllUndeletedMessages(int userId)
        {
            using (SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CarAdvertiser"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT [Id],SenderId,ReceiverId,MessageContent,IsRead,CreatedDate,CreateUser FROM [dbo].[Messages] WHERE IsDeleted <> 1 AND ReceiverId=@receiverId", connection))
                {
                    cmd.Parameters.AddWithValue("@receiverId", userId);
                    cmd.Notification = null;

                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += Dependency_OnChange;

                    if (connection.State == ConnectionState.Closed) connection.Open();

                    List<MessageViewModel> messages = new List<MessageViewModel>();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            messages.Add(new MessageViewModel
                            {
                                MessageId = (int)dr["Id"],
                                SenderId = (int)dr["SenderId"],
                                ReceiverId = (int)dr["ReceiverId"],
                                Message = dr["MessageContent"].ToString(),
                                IsRead = (bool)dr["IsRead"],
                                SenderName = dr["CreateUser"].ToString(),
                                ReceivedDate = ((DateTime)dr["CreatedDate"]).ToString("d")
                            });
                        }
                        dr.Close();
                    }

                    if (connection.State == ConnectionState.Open) connection.Close();

                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(messages);

                    return json;
                }
            }
        }

        private static void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                CarAdvertiserHub.Show();
            }
        }
    }
}