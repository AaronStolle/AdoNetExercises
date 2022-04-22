using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

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
            /*Console.Write("Enter SectionID: ");
            var sectionID = int.Parse(Console.ReadLine());
            GetRoster(sectionID);*/
            /*var s = new Subject();
            InsertSubject(s);*/
            AddTeacher();
        }

        private static void AddTeacher()
        {
            using(var db = AppDbContext.GetDbContext())
            {
                var teacher = new Teacher()
                {
                    FirstName = "Aaron",
                    LastName = "Stolle"
                };
                var existingTeachers = db.Teacher.Where(t => t.LastName == teacher.LastName).ToList();

                db.Teacher.Add(teacher);
                db.SaveChanges();
                Console.WriteLine($"New Teacher added and has ID of {teacher.TeacherID}");
            }         
        }

        static DALResponse InsertSubject(Subject s)
        {
            var response = new DALResponse();

            using (var connection = new SqlConnection(connection_string))
            {
                Console.Write("Enter the new subject new: ");
                var subjectName = Console.ReadLine();
                                
                try
                {
                    connection.Open();
                    var checkSQL = "SELECT * from Subject WHERE SubjectName = @subjectName;";

                    var checkCommand = new SqlCommand(checkSQL, connection);
                    checkCommand.Parameters.AddWithValue("@subjectName", subjectName);

                    using (var reader = checkCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.Success = false;
                            response.Message = $"Subject {subjectName} already exists";
                        }
                        if(!reader.Read())
                        {
                            
                            var insertSubjectSQL = @"INSERT into Subject(SubjectName) VALUES (@subjectName);" + "SELECT SCOPE_IDENTITY()";

                            var insertSubjectCommand = new SqlCommand(insertSubjectSQL, connection);
                            insertSubjectCommand.Parameters.AddWithValue("@subjectName", subjectName);

                            reader.Close();

                            int SubjectID = Convert.ToInt32(insertSubjectCommand.ExecuteScalar());

                            response.Success = true;
                            response.Message = $"Subject {SubjectID} created";
                        }                                
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine(response.Message);
                return response;
            }
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

        static SectionRosterView GetRoster(int sectionID)
        {                        
            var roster = new SectionRosterView();

            roster.rosterItems = new List<RosterItem>();

            using(var connection = new SqlConnection(connection_string))
            {
                var sql1 = @"SELECT s.SectionID,
c.CourseName, sem.StartDate, sem.EndDate, t.LastName + ', ' + t.FirstName Teacher from Section s 
join Course c on s.CourseID = c.CourseID
join Semester sem on s.SemesterID = sem.SemesterID
join Teacher t on s.TeacherID = t.TeacherID
where s.SectionID = @sectionID";

                var sql2 = @"SELECT st.StudentID,
st.LastName + ', ' + st.FirstName Student,
sr.CurrentGrade Grade from Student st 
JOIN SectionRoster sr on st.StudentID = sr.StudentID
JOIN Section s on sr.SectionID = s.SectionID
where s.SectionID = @sectionID
order by s.SectionID, st.LastName";

                var command1 = new SqlCommand(sql1, connection);
                command1.Parameters.AddWithValue("@sectionID", sectionID);

                var command2 = new SqlCommand(sql2, connection);
                command2.Parameters.AddWithValue("@sectionID", sectionID);


                try
                {
                    connection.Open();

                    using (var reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            roster.SectionID = (int)reader["SectionID"];
                            roster.CourseName = reader["CourseName"].ToString();
                            roster.StartDate = (DateTime)reader["StartDate"];
                            roster.EndDate = (DateTime)reader["EndDate"];
                            roster.TeacherName = reader["Teacher"].ToString();
                        }
                    }

                    Console.WriteLine(@$"SectionID: {roster.SectionID}
Course: {roster.CourseName}
Start Date: {roster.StartDate}
End Date: {roster.EndDate}
Teacher: {roster.TeacherName}
ID    Name            Grade");
                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new RosterItem();
                            row.StudentId = (int)reader["StudentID"];
                            row.StudentName = reader["Student"].ToString();
                            
                            if(reader["Grade"] == DBNull.Value)
                            {
                                row.StudentGrade = "N/A";
                            }
                            else
                            {
                                row.StudentGrade = reader["Grade"].ToString();
                            }
                            roster.rosterItems.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                foreach (var r in roster.rosterItems)
                {
                    Console.WriteLine($"{r.StudentId,-2} {r.StudentName, 3} {r.StudentGrade, 10}");
                }
                return roster;
            }
        }
        
    }
}