using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.ValidateRegistrProject;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using DantistLibrary;
using ControlDantist.Classes;
using ControlDantist.FilesRegistr;

namespace ControlDantist.Repository
{
    public class ProjectRegistrFilesRepository : IRepository<ProjectRegistrFiles> , ISelectId<ProjectRegistrFiles>
    {
        private DataClasses1DataContext dc;
        public ProjectRegistrFilesRepository(DataClasses1DataContext dc)
        {
            this.dc = dc;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает список проектов договоров.
        /// </summary>
        /// <param name="idHospital"></param>
        /// <returns></returns>
        public IEnumerable<ProjectRegistrFiles> SelectId(int idHospital)
        {
            return dc.ProjectRegistrFiles.Where(w => w.IdHospital == idHospital);
        }

        public string InsertBinaryFile(ProjectRegistrFiles item, string fileName, System.IO.FileStream fs)
        {

            // массив для хранения бинарных данных файла
            byte[] fDate;
            //using (System.IO.FileStream fs = new System.IO.FileStream(fileName, FileMode.Open))
            //{
            //    fDate = new byte[fs.Length];
            //    //fs.Read(imageData, 0, imageData.Length);

            //    fs.Read(fDate, 0, fDate.Length);
            //}

            fDate = new byte[fs.Length];
            fs.Read(fDate, 0, fDate.Length);

            string query = @"INSERT INTO [dbo].[ProjectRegistrFiles]
                                       ([Registr]
                                       ,[NumberLatter]
                                       ,[DateLetter]
                                       ,[IdHospital]
                                       ,[flagValidateRegistr])
                                 VALUES
                                       (" + fDate + " " +
                                       ",'" + item.NumberLatter.Trim() + "' " +
                                       ",'" + Время.Дата(item.DateLetter.Value.ToShortDateString()) + "' " +
                                       "," + item.IdHospital + " " +
                                       ",false)";

            return query;
        }

        public int InsertBinaryFIle(ProjectRegistrFiles item, byte[] fileData, SqlConnection con, SqlTransaction sqlTransaction)//, byte[] fileData)
        {

            SqlCommand command = new SqlCommand();
            command.Connection = con;

            command.Transaction = sqlTransaction;


            //command.CommandText = @"INSERT INTO [dbo].[ProjectRegistrFiles]
            //                       ([Registr]
            //                       ,[NumberLatter]
            //                       ,[DateLetter]
            //                       ,[IdHospital]
            //                       ,[flagValidateRegistr])
            //                 VALUES
            //                       (<Registr, varbinary(max),>
            //                       ,<NumberLatter, nvarchar(10),>
            //                       ,<DateLetter, date,>
            //                       ,<IdHospital, int,>
            //                       ,<flagValidateRegistr, bit,>)";

            command.CommandText = @"INSERT INTO ProjectRegistrFiles VALUES (@Registr, @NumberLatter, @DateLetter,@IdHospital,@flagValidateRegistr)";
            command.Parameters.Add("@Registr", SqlDbType.VarBinary);
            command.Parameters.Add("@NumberLatter", SqlDbType.NVarChar, 10);
            command.Parameters.Add("@DateLetter", SqlDbType.DateTime);
            command.Parameters.Add("@IdHospital", SqlDbType.Int);
            command.Parameters.Add("@flagValidateRegistr", SqlDbType.Bit);

            // массив для хранения бинарных данных файла
            //byte[] fDate;
            //using (System.IO.FileStream fs = new System.IO.FileStream(fileName, FileMode.Open))
            //{
            //    fDate = new byte[fs.Length];
            //    //fs.Read(imageData, 0, imageData.Length);

            //    fs.Read(fDate, 0, fDate.Length);

            //}

            // передаем данные в команду через параметры
            command.Parameters["@Registr"].Value = fileData;
            command.Parameters["@NumberLatter"].Value = item.NumberLatter.Trim();
            command.Parameters["@DateLetter"].Value = item.DateLetter;
            command.Parameters["@IdHospital"].Value = item.IdHospital;
            command.Parameters["@flagValidateRegistr"].Value = false;

            command.ExecuteNonQuery();

            command.CommandText = "SELECT @@IDENTITY";
            return Convert.ToInt32(command.ExecuteScalar());
        }


        public void Insert(ProjectRegistrFiles item)
        {

            //// Установим уровни изоляции транзакций.
            //var option = new System.Transactions.TransactionOptions();
            //option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

            //// Добавим льготника и адрес в БД.
            //// Внесём данные в таблицу в едино транзакции.
            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
            //{
            //    try
            //    {
            //        dc.ProjectRegistrFiles.InsertOnSubmit(item);

            //        dc.SubmitChanges();

            //        scope.Complete();
            //    }
            //    catch
            //    {
            //        throw new Exception("Запись файла реестра проектов договоров не произведена");
            //    }
            //}

        }

        public void Insert(ProjectRegistrFiles item, string fileName, string newNameFIlePostCopy)
        {
            //// Установим уровни изоляции транзакций.
            //var option = new System.Transactions.TransactionOptions();
            //option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

            //// Добавим льготника и адрес в БД.
            //// Внесём данные в таблицу в едино транзакции.
            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
            //{
            try
            {

                FileRegistrCopy fileRegistrCopy = new FileRegistrCopy();
                bool flagCopy = fileRegistrCopy.FIleRegistrToServer(fileName, item.RegistrFileName,newNameFIlePostCopy);

                if (flagCopy == false)
                {
                    throw new Exception("Не удалось сохранить файл реестра на сервере");
                }

                dc.ProjectRegistrFiles.InsertOnSubmit(item);

                dc.SubmitChanges();

                //scope.Complete();
            }
            catch
            {
                throw new Exception("Запись файла реестра проектов договоров не произведена");
            }
        }


        /// <summary>
        /// Прочитать файл из БД.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectRegistrFiles> Select(int id)
        {
            return dc.ProjectRegistrFiles.Where(w => w.IdProjectRegistr == id);
        }
        public Dictionary<string, Unload> SelectFiles(int id)
        {
            Dictionary<string, Unload> unload = null;

            var registr = dc.ProjectRegistrFiles.Where(w => w.IdProjectRegistr == id).FirstOrDefault();

            if (registr != null)
            {

                string fileName = @"\\10.159.102.100\DantistRepositoryFiles\" + registr.RegistrFileName.Trim();

                using (FileStream fstream = File.Open(fileName, FileMode.Open))
                {
                    //
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    // Получим из файла словарь с договорами.
                    unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);
                }
            }

            return unload;

        }

        /// <summary>
        /// Поиск файла реестра в базе данных.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ProjectRegistrFiles Find(ProjectRegistrFiles item)
        {
            //bool flagFind = false;

            var contract = dc.ProjectRegistrFiles.Where(w => w.IdHospital == item.IdHospital && w.NumberLatter.ToLower().Trim() == item.NumberLatter.ToLower().Trim() && w.DateLetter == item.DateLetter).FirstOrDefault();

            //if(contract != null)
            //{
            //    flagFind = true;
            //}

            return contract;
        }

        public void Update(ProjectRegistrFiles item)
        {
            dc.ProjectRegistrFiles.Where(w => w.IdHospital == item.IdHospital && w.NumberLatter.ToLower().Trim() == item.NumberLatter.ToLower().Trim() && w.DateLetter == item.DateLetter);

            dc.SubmitChanges();
        }

        /// <summary>
        /// Возвращает реестр проектов договоров.
        /// </summary>
        /// <param name="numberLetter"></param>
        /// <param name="dateTimeLetter"></param>
        /// <param name="idHospital"></param>
        /// <returns></returns>
        public IQueryable<RegistrProject> GetRegistr(string numberLetter, DateTime dateTimeLetter, int idHospital)
        {
            return dc.ProjectRegistrFiles.Where(w => w.NumberLatter.Trim() == numberLetter.Trim() && w.DateLetter == dateTimeLetter && w.DateLetter.Value.Year == DateTime.Today.Year && w.IdHospital == idHospital).Select(w => new RegistrProject { IdProjectRegistr = w.IdProjectRegistr, DataLetter = (DateTime)w.DateLetter, NumberLetter = w.NumberLatter, StatusValide = (bool)w.flagValidateRegistr.DoBool() });//.FirstOrDefault();
        }

        public IQueryable<RegistrProject> GetRegistr(int idHospital)
        {
            return dc.ProjectRegistrFiles.Where(w=>w.DateLetter.Value.Year == DateTime.Today.Year && w.IdHospital == idHospital && w.flagValidateRegistr == null).Select(w => new RegistrProject { IdProjectRegistr = w.IdProjectRegistr, DataLetter = (DateTime)w.DateLetter, NumberLetter = w.NumberLatter, StatusValide = (bool)w.flagValidateRegistr.DoBool() }).OrderBy(w=>w.DataLetter).ThenBy(w=>w.NumberLetter);
        }
    }
 }



