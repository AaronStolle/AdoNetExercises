using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AdoNetExercises
{
    class Program
    {
        private static string connection_string =
            "Server=localhost;Database=SimpleSchool;User Id=sa;Password=BadPass123;";

        static void Main(string[] args)
        {
            // DisplayTeacherList();
            // DisplayRoomList();
            // GenerateGUID();
            // DisplaySemesterCourses(1);
            // DisplaySectionCount(1);
            // DMLQueries();
            //TransactionExample();

        }

        static void GenerateGUID()
        {
            Guid g = Guid.NewGuid();
            Console.WriteLine(g);

        }

        static void DisplayTeacherList()
        {
            // 1 instantiate connection
            using (var connection = new SqlConnection(connection_string))
            {
                var sql = "SELECT * FROM Teacher";

                // 2 instantiate command, give it SQL and the connection to use
                var command = new SqlCommand(sql, connection);

                try
                {
                    // 3 open connection
                    connection.Open();

                    // 4 execute command, use ExecuteReader() for SELECT with multiple rows
                    using (var reader = command.ExecuteReader())
                    {
                        // 5 Loop reader
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["TeacherID"]} : {reader["LastName"]}, {reader["FirstName"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void DisplayRoomList()
        {
            var rooms = new List<BuildingRoom>();

            // 1 instantiate connection
            using (var connection = new SqlConnection(connection_string))
            {
                var sql = "SELECT r.BuildingID, r.RoomID, r.RoomNumber, r.Description " +
                    "FROM Room r " +
                    "INNER JOIN Building b ON r.BuildingID = b.BuildingID";

                // 2 instantiate command, give it SQL and the connection to use
                var command = new SqlCommand(sql, connection);

                try
                {
                    // 3 open connection
                    connection.Open();

                    // 4 execute command, use ExecuteReader() for SELECT with multiple rows
                    using (var reader = command.ExecuteReader())
                    {
                        // 5 Loop reader
                        while (reader.Read())
                        {
                            // parse row data into new object
                            var row = new BuildingRoom();

                            row.BuildingID = (int)reader["BuildingID"];
                            row.RoomID = (int)reader["RoomID"];
                            row.RoomNumber = (int)reader["RoomNumber"];

                            if (reader["Description"] != DBNull.Value)
                            {
                                row.Description = reader["Description"].ToString();
                            }

                            // add room to collection
                            rooms.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                foreach (var room in rooms)
                {
                    Console.WriteLine($"{room.BuildingID} : {room.RoomNumber} {room.Description}");
                }
            }
        }

        static void DisplaySemesterCourses(int semesterId)
        {
            var sections = new List<SectionDetail>();

            // 1 instantiate connection
            using (var connection = new SqlConnection(connection_string))
            {
                var sql = "SELECT s.SectionID, t.FirstName, t.LastName, c.CourseName, p.PeriodName, " +
                            "p.StartTime, p.EndTime, b.BuildingName, r.RoomNumber " +
                        "FROM Section s " +
                            "INNER JOIN Teacher t ON s.Teacherid = t.Teacherid " +
                            "INNER JOIN Room r ON s.Roomid = r.RoomID " +
                            "INNER JOIN Building b ON r.Buildingid = b.Buildingid " +
                            "INNER JOIN Period p ON s.Periodid = p.PeriodID " +
                            "INNER JOIN Course c ON s.Courseid = c.Courseid " +
                        "WHERE SemesterID = @SemesterID" +
                        " ORDER BY s.PeriodID, LastName";

                // 2 instantiate command, give it SQL and the connection to use
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SemesterID", semesterId);

                try
                {
                    // 3 open connection
                    connection.Open();

                    // 4 execute command, use ExecuteReader() for SELECT with multiple rows
                    using (var reader = command.ExecuteReader())
                    {
                        // 5 Loop reader
                        while (reader.Read())
                        {
                            // parse row data into new object
                            var row = new SectionDetail();

                            row.SectionID = (int)reader["SectionID"];
                            row.TeacherName = $"{reader["LastName"]}, {reader["FirstName"]}";
                            row.CourseName = reader["CourseName"].ToString();
                            row.RoomInformation = $"{reader["BuildingName"]} {reader["RoomNumber"]}";
                            row.StartTime = (TimeSpan)reader["StartTime"];
                            row.EndTime = (TimeSpan)reader["EndTime"];
                            row.PeriodName = reader["PeriodName"].ToString();

                            // add row to collection
                            sections.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                foreach (var s in sections)
                {
                    Console.WriteLine($"{s.SectionID,2} {s.TeacherName,-25} {s.CourseName,-15} {s.PeriodName} " +
                        $"{s.StartTime} - {s.EndTime} {s.RoomInformation}");
                }
            }
        }

        static void DisplaySectionCount(int semesterID)
        {
            using (var connection = new SqlConnection(connection_string))
            {
                var sql = "SELECT COUNT(SectionID) FROM Section WHERE SemesterID=@SemesterID";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SemesterID", semesterID);

                try
                {

                    connection.Open();
                    int sectionCount = (int)command.ExecuteScalar();

                    Console.WriteLine($"There are {sectionCount} sections in semester {semesterID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void DMLQueries()
        {
            using (var connection = new SqlConnection(connection_string))
            {
                var insertSQL = "INSERT INTO Student (FirstName, LastName, ClassYear) " +
                    "VALUES (@FirstName, @LastName, @ClassYear)";

                var updateSQL = "UPDATE Student SET " +
                    "ClassYear = @ClassYear " +
                    "WHERE LastName = 'DMLTest'";

                var deleteSQL = "DELETE FROM Student WHERE LastName = 'DMLTEST'";

                var insertCommand = new SqlCommand(insertSQL, connection);
                insertCommand.Parameters.AddWithValue("@FirstName", "New");
                insertCommand.Parameters.AddWithValue("@LastName", "DMLTest");
                insertCommand.Parameters.AddWithValue("@ClassYear", "0000");

                var updateCommand = new SqlCommand(updateSQL, connection);
                updateCommand.Parameters.AddWithValue("@ClassYear", "9999");

                var deleteCommand = new SqlCommand(deleteSQL, connection);


                try
                {
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                    updateCommand.ExecuteNonQuery();
                    deleteCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void TransactionExample()
        {
            using (var connection = new SqlConnection(connection_string))
            {
                var buildingSQL = "INSERT INTO Building (BuildingName) " +
                    "VALUES (@BuildingName); " +
                    "SELECT SCOPE_IDENTITY()";

                var roomSQL = "INSERT INTO Room (BuildingID, RoomNumber, Description) " +
                    "VALUES (@BuildingID, @RoomNumber, @Description)";


                try
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    try
                    {
                        var command1 = new SqlCommand(buildingSQL, connection, transaction);
                        command1.Parameters.AddWithValue("@BuildingName", "Arts Complex");

                        int buildingID = Convert.ToInt32(command1.ExecuteScalar());

                        var command2 = new SqlCommand(roomSQL, connection, transaction);
                        command2.Parameters.AddWithValue("@BuildingID", buildingID);
                        command2.Parameters.AddWithValue("@RoomNumber", 100);
                        command2.Parameters.AddWithValue("@Description", "Dance Studio");
                        command2.ExecuteNonQuery();

                        // this will fail, room number cannot be null
                        var command3 = new SqlCommand(roomSQL, connection, transaction);
                        command3.Parameters.AddWithValue("@BuildingID", buildingID);
                        command3.Parameters.AddWithValue("@RoomNumber", DBNull.Value);
                        // command3.Parameters.AddWithValue("@RoomNumber", 101);
                        command3.Parameters.AddWithValue("@Description", DBNull.Value);

                        command3.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
}